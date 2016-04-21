using System;
using System.Collections.Generic;
using System.IO;
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
    public class PackageDetailsControllerDeleteTests
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
            LoadAllPackages();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ClearAllPackages();
        }

        private static void LoadAllPackages()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;

            LoadData.UploadAllPackages(PackageDAO, InvokeUrl);
        }
        private static void ClearAllPackages()
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

        [Category("nugetory.Integration.PackageDetailsControllerDelete"), Test, Timeout(1000)]
        public void PackageDetailsControllerDeleteNoApiKeySuccessTest()
        {
            ClearAllPackages();
            LoadAllPackages();

            long originalCount = PackageDAO.Count().Result;

            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;

            const string suffix = "/Packages(Id='{0}',Version='{1}')";

            Package pkgV000 = PackageDAO.Read(NugetSamplePackages.nugetoryV000.Title,
                                              NugetSamplePackages.nugetoryV000.Version).Result;

            string urlSuffix = string.Format(suffix, pkgV000.Title, pkgV000.Version);
            string url = InvokeUrl + urlSuffix;
            HttpStatusCode resultCode = HttpClient.Invoke(url, "DELETE");

            Assert.AreEqual(HttpStatusCode.OK, resultCode);
            Assert.AreEqual(originalCount - 1, PackageDAO.Count().Result);
        }

        [Category("nugetory.Integration.PackageDetailsControllerDelete"), Test]//, Timeout(1000)]
        public void PackageDetailsControllerDeleteApiKeyFailTest()
        {
            ClearAllPackages();
            LoadAllPackages();
            
            long originalCount = PackageDAO.Count().Result;

            ValidateAuthenticationAttribute.ApiKey = Path.GetRandomFileName().Replace(".", "");
            HttpClient.ApiKey = null;

            const string suffix = "/Packages(Id='{0}',Version='{1}')";

            Package pkgV000 = PackageDAO.Read(NugetSamplePackages.nugetoryV000.Title,
                                              NugetSamplePackages.nugetoryV000.Version).Result;

            string urlSuffix = string.Format(suffix, pkgV000.Title, pkgV000.Version);
            string url = InvokeUrl + urlSuffix;
            HttpStatusCode resultCode = HttpClient.Invoke(url, "DELETE");

            Assert.AreEqual(HttpStatusCode.Unauthorized, resultCode);
            Assert.AreEqual(originalCount, PackageDAO.Count().Result);
            
            HttpClient.ApiKey = Path.GetRandomFileName().Replace(".", "");
            Assert.AreNotEqual(ValidateAuthenticationAttribute.ApiKey, HttpClient.ApiKey);

            resultCode = HttpClient.Invoke(url, "DELETE");

            Assert.AreEqual(HttpStatusCode.Unauthorized, resultCode);
            Assert.AreEqual(originalCount, PackageDAO.Count().Result);
        }

        [Category("nugetory.Integration.PackageDetailsControllerDelete"), Test, Timeout(1000)]
        public void PackageDetailsControllerDeleteApiKeySuccessTest()
        {
            ClearAllPackages();
            LoadAllPackages();
            
            long originalCount = PackageDAO.Count().Result;

            ValidateAuthenticationAttribute.ApiKey = Path.GetRandomFileName().Replace(".", "");
            HttpClient.ApiKey = ValidateAuthenticationAttribute.ApiKey;

            const string suffix = "/Packages(Id='{0}',Version='{1}')";

            Package pkgV000 = PackageDAO.Read(NugetSamplePackages.nugetoryV000.Title,
                                              NugetSamplePackages.nugetoryV000.Version).Result;

            string urlSuffix = string.Format(suffix, pkgV000.Title, pkgV000.Version);
            string url = InvokeUrl + urlSuffix;
            HttpStatusCode resultCode = HttpClient.Invoke(url, "DELETE");

            Assert.AreEqual(HttpStatusCode.OK, resultCode);
            Assert.AreEqual(originalCount - 1, PackageDAO.Count().Result);
        }

    }
}
