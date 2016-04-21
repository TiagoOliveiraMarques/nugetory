using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using nugetory.Controllers.Helpers;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Endpoint;
using nugetory.tests.Support;
using nugetory.Tools;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class SearchControllerGetTests
    {
        private const string BaseUrl = "http://localhost:9000";
        private const string InvokeUrl = BaseUrl + "/api/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;

            PackageDAO = SetUp.Manager.DataManager.PackageDAO;
            FileStore = SetUp.Manager.DataManager.FileStore;
            LoadData.UploadAllPackages(PackageDAO, InvokeUrl);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            List<Package> packages = PackageDAO.Read(p => true).Result;
            packages.ForEach(p =>
            {
                PackageDAO.Delete(p.Id);
                FileStore.DeleteFile(p.Id);
            });
        }

        public static PackageDAO PackageDAO { get; set; }
        public static IFileStore FileStore { get; set; }

        [Category("nugetory.Integration.SearchControllerGet"), Test, Timeout(1000)]
        public void SearchControllerGetTest()
        {
            string responseData;
            string targetFramework = "";

            Package pkgV003 = PackageDAO.Read(NugetSamplePackages.nugetoryV003.Title,
                                              NugetSamplePackages.nugetoryV003.Version).Result;

            if (pkgV003.FrameworkAssemblies != null && pkgV003.FrameworkAssemblies.Any())
                targetFramework = pkgV003.FrameworkAssemblies[0].TargetFramework;
            string url = InvokeUrl + GetQuery("IsLatestVersion", "", pkgV003.Title, targetFramework,
                                              "false", "0", "1");
            HttpClient.Invoke(url, "GET", out responseData);

            string result = GetSearchResult(new List<Package> {pkgV003});

            Assert.AreEqual(result, responseData);
        }

        #region Test Tools

        private static string GetSearchResult(List<Package> packages)
        {
            Uri uri = new Uri(BaseUrl);
            string baseUri = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
            string apiUri = baseUri + "/api/v2/";

            DateTime updated = packages.Any()
                                   ? packages.OrderByDescending(p => p.DateUpdated).First().DateUpdated
                                   : DateTime.UtcNow;

            SeachFeed.feed feed = new SeachFeed.feed
            {
                @base = apiUri,
                count = packages.Count,
                updated = updated,
                link = new SeachFeed.feedLink(apiUri + "Packages"),
                entry = packages.Select(p => Search.GetPackageEntry(p, uri)).ToArray()
            };

            XmlSerializer serializer = new XmlSerializer(typeof(SeachFeed.feed));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, feed);
            ms.Position = 0;
            return new StreamReader(ms).ReadToEnd();
        }

        private static string GetQuery(string filter, string orderBy, string searchTerm, string targetFramework,
                                       string includePrerelease, string skip, string stop)
        {
            Func<string, string, string> appendParam = (query, param) =>
            {
                if (query == "/Search()")
                    query += "?";

                if (string.IsNullOrWhiteSpace(query))
                    return query + param;

                return query + "&" + param;
            };

            string result = "/Search()";

            if (!string.IsNullOrWhiteSpace(filter))
                result = appendParam(result, "$filter=" + filter);
            if (!string.IsNullOrWhiteSpace(orderBy))
                result = appendParam(result, "$orderby=" + orderBy);
            if (!string.IsNullOrWhiteSpace(searchTerm))
                result = appendParam(result, "searchTerm=" + searchTerm);
            if (!string.IsNullOrWhiteSpace(targetFramework))
                result = appendParam(result, "targetFramework=" + targetFramework);
            if (!string.IsNullOrWhiteSpace(includePrerelease))
                result = appendParam(result, "includePrerelease=" + includePrerelease);
            if (!string.IsNullOrWhiteSpace(skip))
                result = appendParam(result, "$skip=" + skip);
            if (!string.IsNullOrWhiteSpace(stop))
                result = appendParam(result, "$stop=" + stop);

            return result;
        }

        #endregion
    }
}
