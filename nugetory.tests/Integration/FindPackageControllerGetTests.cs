using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class FindPackageControllerGetTests
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

        [Category("nugetory.Integration.FindPackageControllerGet"), Test]//, Timeout(1000)]
        public void FindPackageControllerGetTest()
        {
            string responseData;
            
            Package pkgV003 = PackageDAO.Read(NugetSamplePackages.nugetoryV003.Title,
                                              NugetSamplePackages.nugetoryV003.Version).Result;

            string url = InvokeUrl + GetQuery("IsLatestVersion", "", pkgV003.Title, "");
            HttpClient.Invoke(url, "GET", out responseData);

            string result = GetFindPackageResult(url, new List<Package> { pkgV003 }, true);

            Assert.AreEqual(result, responseData);
        }

        #region Test Tools

        private static string GetFindPackageResult(string url, List<Package> packages, bool latestVersion)
        {
            string apiUri = url.Replace("&", "&amp;");
            Uri uri = new Uri(apiUri);

            DateTime updated = packages.Any()
                                   ? packages.OrderByDescending(p => p.DateUpdated).First().DateUpdated
                                   : DateTime.UtcNow;

            entry[] entries = latestVersion
                ? new[] { PackageDetails.GetPackageEntry(packages.FirstOrDefault(p => p.LatestVersion), uri) }
                : packages.Select(p => PackageDetails.GetPackageEntry(p, uri)).ToArray();

            feed feed = new feed
            {
                @base = InvokeUrl + "/",
                id = WebUtility.HtmlDecode(uri.ToString()),
                title = new feedTitle("FindPackagesById"),
                updated = updated,
                link = new feedLink("FindPackagesById", "FindPackagesById"),
                entry = entries
            };

            XmlSerializer serializer = new XmlSerializer(typeof(feed));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, feed);
            ms.Position = 0;
            return new StreamReader(ms).ReadToEnd();
        }

        private static string GetQuery(string filter, string orderBy, string id, string top)
        {
            Func<string, string, string> appendParam = (query, param) =>
            {
                if (query == "/FindPackagesById()")
                    query += "?";

                if (string.IsNullOrWhiteSpace(query))
                    return query + param;

                return query + "&" + param;
            };

            string result = "/FindPackagesById()";

            if (!string.IsNullOrWhiteSpace(filter))
                result = appendParam(result, "$filter=" + filter);
            if (!string.IsNullOrWhiteSpace(orderBy))
                result = appendParam(result, "$orderby=" + orderBy);
            if (!string.IsNullOrWhiteSpace(id))
                result = appendParam(result, "id=" + id);
            if (!string.IsNullOrWhiteSpace(top))
                result = appendParam(result, "$top=" + top);

            return result;
        }

        #endregion
    }
}
