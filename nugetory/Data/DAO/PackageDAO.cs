using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Tools;

namespace nugetory.Data.DAO
{
    public class PackageDAO : BaseDAO<Package>
    {
        public IFileStore FileStore { get; set; }

        public PackageDAO(DB.ICollection<Package> entities, IFileStore fileStore)
            : base(entities)
        {
            FileStore = fileStore;
        }

        private string GetId(string title, string version)
        {
            return title.ToLowerInvariant() + "." + version.ToLowerInvariant();
        }

        public Task<Package> Read(string title, string version)
        {
            return base.Read(GetId(title, version));
        }

        public bool ProcessPackage(Stream fileStream)
        {
            MemoryStream memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            ZipArchive archive = new ZipArchive(memoryStream);

            ZipArchiveEntry file = archive.Entries.FirstOrDefault(entry => entry.FullName.EndsWith(".nuspec"));
            if (file == null)
                throw new InvalidOperationException("No nuspec in nupkg");

            string nuspecContent = new StreamReader(file.Open()).ReadToEnd();

            XmlSerializer ser = new XmlSerializer(typeof(package));

            using (StringReader sr = new StringReader(nuspecContent))
            {
                package nuspec = (package) ser.Deserialize(new NamespaceIgnorantXmlTextReader(sr));

                if (nuspec == null || nuspec.metadata == null)
                    throw new InvalidOperationException("Invalid nuspec");

                packageMetadata metadata = nuspec.metadata;

                if (string.IsNullOrWhiteSpace(metadata.id))
                    throw new InvalidOperationException("Invalid nuspec (missing id)");
                if (string.IsNullOrWhiteSpace(metadata.version))
                    throw new InvalidOperationException("Invalid nuspec (missing version)");

                string title = metadata.id;
                string version = metadata.version;

                Regex rgxId = new Regex(@"^[A-Za-z0-9\.\~\+_\-]+$");
                if (!rgxId.IsMatch(title))
                    throw new InvalidOperationException("Invalid id specified in nuspec");

                Regex rgxVersion = new Regex(@"^[0-9\.\-]+$");
                if (!rgxVersion.IsMatch(version))
                    throw new InvalidOperationException("Invalid version specified in nuspec");

                long size = memoryStream.Length;
                memoryStream.Position = 0;
                string checksum = FileStore.SaveFile(memoryStream, title + "." + version);

                Package package = new Package(nuspec, checksum, size);

                memoryStream.Close();
                archive.Dispose();
                fileStream.Close();
                return Create(package).Result;
            }
        }

        public override Task<bool> Create(Package entity)
        {
            if (entity.IsPrerelease == false)
            {
                List<Package> previousPackages = Read(e => e.Title == entity.Title).Result;

                if (previousPackages.Any(p => p.VersionHigherThan(entity)))
                {
                    entity.LatestVersion = false;
                }
                else
                {
                    entity.LatestVersion = true;

                    if (previousPackages.Any())
                    {
                        foreach (Package p in previousPackages.Where(p => p.LatestVersion == false))
                        {
                            p.LatestVersion = false;
                            Update(p).Wait();
                        }
                    }
                }
            }
            else if (entity.IsPrerelease && entity.LatestVersion)
            {
                return Task.FromResult(false);
            }
            else if (entity.IsPrerelease)
            {
                Package previousPackage = Read(GetId(entity.Title, entity.Version)).Result;

                if (previousPackage != null)
                    return Task.FromResult(false);
            }

            return base.Create(entity);
        }

        public override Task<bool> Update(Package entity)
        {
            string id = GetId(entity.Title, entity.Version);
            Package origEntity = Read(id).Result;

            if (origEntity == null)
                return Task.FromResult(false);
            if (origEntity.LatestVersion != entity.LatestVersion)
                return Task.FromResult(false);
            if (origEntity.IsPrerelease != entity.IsPrerelease)
                return Task.FromResult(false);
            if (origEntity.PackageHash != entity.PackageHash)
                return Task.FromResult(false);
            if (origEntity.PackageSize != entity.PackageSize)
                return Task.FromResult(false);
            if (origEntity.PackageId != entity.PackageId)
                return Task.FromResult(false);
            if (origEntity.DateCreated != entity.DateCreated)
                return Task.FromResult(false);

            entity.DateUpdated = DateTime.UtcNow;

            return base.Update(entity);
        }

        public override Task<bool> Delete(string id)
        {
            Package origEntity = Read(id).Result;

            if (origEntity == null)
                return Task.FromResult(false);

            if (origEntity.LatestVersion)
            {
                List<Package> previousPackages = Read(e => e.Title == origEntity.Title).Result;

                previousPackages.Remove(origEntity);

                if (previousPackages.Any())
                {
                    previousPackages.Sort(((p1, p2) => p1.VersionHigherThan(p2) ? 1 : -1));

                    Package latestPackage = previousPackages.First();

                    latestPackage.LatestVersion = true;
                    Update(latestPackage).Wait();
                }
            }

            return base.Delete(id);
        }
    }
}
