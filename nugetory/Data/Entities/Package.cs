using System;
using nugetory.Tools;

namespace nugetory.Data.Entities
{
    public class Package : IBaseEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string PackageTitle { get; set; }
        public string Version { get; set; }
        public string Authors { get; set; }
        public string Copyright { get; set; }
        public PackageDependency[] Dependencies { get; set; }
        public FrameworkAssembly[] FrameworkAssemblies { get; set; }
        public string Description { get; set; }
        public string PackageHash { get; set; }
        public long PackageSize { get; set; }
        public string IconUrl { get; set; }
        public bool IsPrerelease { get; set; }
        public bool LatestVersion { get; set; }
        public string LicenseUrl { get; set; }
        public string Owners { get; set; }
        public string PackageId { get; set; }
        public string ProjectUrl { get; set; }
        public string ReleaseNotes { get; set; }
        public string Language { get; set; }
        public bool RequireLicenseAcceptance { get; set; }
        public string[] Tags { get; set; }
        public string MinClientVersion { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Package()
        {
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateCreated;

            Dependencies = new PackageDependency[0];
            FrameworkAssemblies = new FrameworkAssembly[0];
        }

        public Package(package package, string hash, long size)
        {
            if (package == null || package.metadata == null)
                return;
            
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateCreated;

            Id = package.metadata.id.ToLowerInvariant() + "." + package.metadata.version.ToLowerInvariant();
            Title = package.metadata.id;
            PackageTitle = package.metadata.title;
            Version = package.metadata.version;
            Authors = package.metadata.authors;
            Copyright = package.metadata.copyright;
            Dependencies = PackageDependency.Process(package.metadata.dependencies);
            FrameworkAssemblies = FrameworkAssembly.Process(package.metadata.frameworkAssemblies);
            Description = package.metadata.description;
            PackageHash = hash;
            PackageSize = size;
            IconUrl = package.metadata.iconUrl;
            IsPrerelease = Version == null || Version.IndexOf('-') >= 0;
            LicenseUrl = package.metadata.licenseUrl;
            Owners = package.metadata.owners;
            PackageId = Id;
            ProjectUrl = package.metadata.projectUrl;
            ReleaseNotes = package.metadata.releaseNotes;
            Language = package.metadata.language;
            RequireLicenseAcceptance = package.metadata.requireLicenseAcceptance;
            Tags = package.metadata.tags != null ? package.metadata.tags.Split(' ') : new string[] {};
            MinClientVersion = package.metadata.minClientVersion;
        }

        public bool VersionHigherThan(Package entity)
        {
            if (string.IsNullOrWhiteSpace(Version))
                return false;
            if (string.IsNullOrWhiteSpace(entity.Version))
                return true;

            string[] currVersion = Version.Split('.', '-');
            string[] otherVersion = entity.Version.Split('.', '-');

            int maxSteps = Math.Max(currVersion.Length, otherVersion.Length);

            for (int i = 0; i < maxSteps; i++)
            {
                if (i >= currVersion.Length)
                    return false;
                if (i >= otherVersion.Length)
                    return true;

                int currVersionNum = -1;
                int otherVersionNum = -1;

                if (int.TryParse(currVersion[i], out currVersionNum) == false)
                    return false;
                if (int.TryParse(otherVersion[i], out otherVersionNum) == false)
                    return true;

                if (currVersionNum < otherVersionNum)
                    return false;
                if (otherVersionNum < currVersionNum)
                    return true;
            }

            return true;
        }
    }
}
