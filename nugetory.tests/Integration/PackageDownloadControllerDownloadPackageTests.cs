using System.Collections.Generic;
using System.IO;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Endpoint;
using nugetory.tests.Support;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class PackageDownloadControllerDownloadPackageTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

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

        [Category("nugetory.Integration.PackageDownloadControllerGet"), Test, Timeout(1000)]
        public void PackageDownloadControllerGetTest()
        {
            const string suffix = "/package/{0}/{1}";
            string responseData;

            Package pkgV000 = PackageDAO.Read(NugetSamplePackages.nugetoryV000.Title,
                                              NugetSamplePackages.nugetoryV000.Version).Result;
            string pkgV000File;

            using (StreamReader sr = new StreamReader(FileStore.GetFile(pkgV000.Id, pkgV000.PackageHash)))
                pkgV000File = sr.ReadToEnd();

            string url = InvokeUrl + string.Format(suffix, pkgV000.Title, pkgV000.Version);
            HttpClient.Invoke(url, "GET", out responseData);

            Assert.AreEqual(pkgV000File, responseData);

            Package pkgV003 = PackageDAO.Read(NugetSamplePackages.nugetoryV003.Title,
                                              NugetSamplePackages.nugetoryV003.Version).Result;
            string pkgV003File;

            using (StreamReader sr = new StreamReader(FileStore.GetFile(pkgV003.Id, pkgV003.PackageHash)))
                pkgV003File = sr.ReadToEnd();

            url = InvokeUrl + string.Format(suffix, pkgV003.Title, pkgV003.Version);
            HttpClient.Invoke(url, "GET", out responseData);

            Assert.AreEqual(pkgV003File, responseData);
        }
    }
}
