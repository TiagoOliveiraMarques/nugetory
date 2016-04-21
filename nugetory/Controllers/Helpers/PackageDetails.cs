using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Exceptions;
using nugetory.Tools;

namespace nugetory.Controllers.Helpers
{
    public static class PackageDetails
    {
        public static PackageDAO PackageDAO { get; set; }
        public static IFileStore FileStore { get; set; }

        public static HttpResponseMessage BuildResponse(HttpRequestMessage request, string id, string version)
        {
            Package package = PackageDAO.Read(id, version).Result;
            if (package == null)
                throw new PackageNotFoundException();

            HttpResponseMessage res = request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(GetDetails(request, package));

            res.Content.Headers.Remove("Content-Type");
            res.Content.Headers.Add("Content-Type", "application/atom+xml;type=entry;charset=utf-8");
            res.Headers.Add("DataServiceVersion", "2.0;");

            return res;
        }

        private static string GetDetails(HttpRequestMessage request, Package package)
        {
            entry entry = GetPackageEntry(package, request.RequestUri);

            XmlSerializer serializer = new XmlSerializer(typeof(entry));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, entry);
            ms.Position = 0;
            string result = new StreamReader(ms).ReadToEnd();
            return result;
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
            
            entry entry = new entry
            {
                @base = apiUri,
                id = WebUtility.HtmlDecode(uri.ToString()),
                category = new entryCategory(),
                link = new[]
                {
                    new entryLink("edit", "Packages(Id='" + package.Title + "',Version='" + package.Title + "')"),
                    new entryLink("edit-media",
                                  "Packages(Id='" + package.Title + "',Version='" + package.Title + "')/$value")
                },
                title = new entryTitle(package.Title),
                summary = new entrySummary(package.Description),
                updated = package.DateUpdated.ToString("O"),
                author = new entryAuthor(package.Authors),
                content = new entryContent(apiUri + "package/" + package.Title + "/" + package.Version),
                properties = new properties
                {
                    Version = package.Version,
                    NormalizedVersion = package.Version,
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
                    Published = new Published(package.DateCreated.ToString("o")),
                    PackageHash = package.PackageHash,
                    PackageHashAlgorithm = "SHA512",
                    PackageSize = new PackageSize(Convert.ToUInt32(package.PackageSize)),
                    ProjectUrl = package.ProjectUrl,
                    ReportAbuseUrl = baseUri + "/package/ReportAbuse/" + package.Title + "/" + package.Title,
                    ReleaseNotes = package.ReleaseNotes,
                    RequireLicenseAcceptance = new RequireLicenseAcceptance(package.RequireLicenseAcceptance),
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
