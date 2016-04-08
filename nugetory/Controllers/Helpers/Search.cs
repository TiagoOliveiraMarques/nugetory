using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Tools;

namespace nugetory.Controllers.Helpers
{
    public static class Search
    {
        public static PackageDAO PackageDAO { get; set; }

        public static HttpResponseMessage Invoke(HttpRequestMessage request, string filter, string orderby, bool desc,
                                                 string searchTerm, string targetFramework, bool includePrerelease,
                                                 int skip, int stop)
        {
            Uri uri = request.RequestUri;
            bool filterIsLatestVersion = filter == "IsLatestVersion";

            List<Package> packages = ReadPackages(filterIsLatestVersion, orderby, desc, searchTerm, targetFramework,
                includePrerelease, skip, stop);

            DateTime updated = packages.Any()
                ? packages.OrderByDescending(p => p.DateUpdated).First().DateUpdated
                : DateTime.UtcNow;

            string baseUri = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
            string apiUri = baseUri + "/api/v2/";

            SeachFeed.feed feed = new SeachFeed.feed
            {
                @base = apiUri,
                count = packages.Count,
                updated = updated,
                link = new SeachFeed.feedLink(apiUri + "Packages"),
                entry = packages.Select(p => GetPackageEntry(p, uri)).ToArray()
            };

            XmlSerializer serializer = new XmlSerializer(typeof(SeachFeed.feed));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, feed);
            ms.Position = 0;
            string result = new StreamReader(ms).ReadToEnd();

            HttpResponseMessage res = request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(result);

            res.Content.Headers.Remove("Content-Type");
            res.Content.Headers.Add("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
            res.Headers.Add("DataServiceVersion", "2.0;");

            return res;
        }

        private static List<Package> ReadPackages(bool filterIsLatestVersion, string orderby, bool desc,
                                                  string searchTerm, string targetFramework, bool includePrerelease,
                                                  int skip, int stop)
        {
            if (searchTerm != null)
                searchTerm = searchTerm.ToLowerInvariant();
            if (targetFramework != null)
                targetFramework = targetFramework.ToLowerInvariant();
            List<Package> packages;
            if (!string.IsNullOrWhiteSpace(searchTerm) && !string.IsNullOrWhiteSpace(targetFramework))
            {
                packages = PackageDAO.Read(p =>
                                           {
                                               if (filterIsLatestVersion && !p.LatestVersion)
                                                   return false;
                                               if (!includePrerelease && p.IsPrerelease)
                                                   return false;
                                               if (p.FrameworkAssemblies != null)
                                                   return p.Title.ToLowerInvariant().Contains(searchTerm) &&
                                                          p.FrameworkAssemblies.Any(
                                                              f => f.TargetFramework.ToLower() == targetFramework);
                                               return p.Title.ToLowerInvariant().Contains(searchTerm);
                                           }).Result;
            }
            else if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                packages = PackageDAO.Read(p =>
                                           {
                                               if (filterIsLatestVersion && !p.LatestVersion)
                                                   return false;
                                               if (!includePrerelease && p.IsPrerelease)
                                                   return false;
                                               return p.Title.ToLowerInvariant().Contains(searchTerm);
                                           }).Result;
            }
            else if (!string.IsNullOrWhiteSpace(targetFramework))
            {
                packages = PackageDAO.Read(p =>
                                           {
                                               if (filterIsLatestVersion && !p.LatestVersion)
                                                   return false;
                                               if (!includePrerelease && p.IsPrerelease)
                                                   return false;
                                               return p.FrameworkAssemblies != null
                                                   ? p.FrameworkAssemblies.Any(
                                                       f => f.TargetFramework.ToLower() == targetFramework)
                                                   : p.Title.ToLowerInvariant().Contains(searchTerm);
                                           }).Result;
            }
            else
            {
                packages = PackageDAO.Read(p =>
                                           {
                                               if (filterIsLatestVersion && !p.LatestVersion)
                                                   return false;
                                               if (!includePrerelease && p.IsPrerelease)
                                                   return false;
                                               return true;
                                           }).Result;
            }

            if (skip > 0 && stop > 0)
                packages = packages.Skip(skip).Take(stop).ToList();
            else if (skip > 0)
                packages = packages.Skip(skip).ToList();
            else if (stop > 0)
                packages = packages.Take(stop).ToList();

            return packages;
        }

        public static entry GetPackageEntry(Package package, Uri uri)
        {
            if (package == null)
                return null;

            string baseUri = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
            string apiUri = baseUri + "/api/v2/";

            string dependencies = package.Dependencies != null
                ? string.Join("|", package.Dependencies.Select(d => d.Id + ":" + d.Version))
                : "";

            string id = apiUri + "Packages(Id='" + package.Title + "',Version='" + package.Version + "')";

            entry entry = new entry
            {
                id = id,
                category = new entryCategory("NuGetGallery.OData.V2FeedPackage"),
                link = new[]
                {
                    new entryLink("edit", id),
                    new entryLink("self", id)
                },
                title = new entryTitle(package.Title),
                summary = new entrySummary(package.Description),
                updated = package.DateUpdated.ToString("o"),
                author = new entryAuthor(package.Authors),
                content = new entryContent(apiUri + "package/" + package.Title + "/" + package.Version),
                properties = new properties
                {
                    Id = package.PackageTitle,
                    Version = package.Version,
                    NormalizedVersion = package.Version,
                    Authors = package.Authors,
                    Copyright = package.Copyright,
                    Created = new Created(package.DateCreated.ToString("o")),
                    Dependencies = dependencies,
                    Description = package.Description,
                    DownloadCount = new DownloadCount(1),
                    GalleryDetailsUrl = baseUri + "/packages/" + package.Title + "/" + package.Title,
                    IconUrl = package.IconUrl,
                    IsLatestVersion = new IsLatestVersion(package.LatestVersion),
                    IsAbsoluteLatestVersion = new IsAbsoluteLatestVersion(package.LatestVersion),
                    IsPrerelease = new IsPrerelease(package.IsPrerelease),
                    Language = package.Language,
                    LastUpdated = new LastUpdated(package.DateCreated.ToString("o")),
                    Published = new Published(package.DateCreated.ToString("o")),
                    PackageHash = package.PackageHash,
                    PackageHashAlgorithm = "SHA512",
                    PackageSize = new PackageSize(Convert.ToUInt32(package.PackageSize)),
                    ProjectUrl = package.ProjectUrl,
                    ReportAbuseUrl = baseUri + "/package/ReportAbuse/" + package.Title + "/" + package.Title,
                    ReleaseNotes = package.ReleaseNotes,
                    RequireLicenseAcceptance = new RequireLicenseAcceptance(package.RequireLicenseAcceptance),
                    Summary = package.Description,
                    Tags = string.Join(" ", package.Tags),
                    Title = package.PackageTitle,
                    VersionDownloadCount = new VersionDownloadCount(0),
                    MinClientVersion = new MinClientVersion(),
                    LastEdited = new LastEdited(),
                    LicenseUrl = package.LicenseUrl,
                    LicenseNames = new LicenseNames(),
                    LicenseReportUrl = new LicenseReportUrl()
                }
            };

            return entry;
        }
    }
}
