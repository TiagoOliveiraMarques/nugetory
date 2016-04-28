using System;
using System.IO;
using System.Linq;
using System.Net;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Endpoint;
using nugetory.tests.Support;
using NUnit.Framework;
using System.Text;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class RootControllerPutTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;
            PackageDAO = SetUp.Manager.DataManager.PackageDAO;
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;
        }

        public static PackageDAO PackageDAO { get; set; }

        [Category("nugetory.Integration.RootControllerPut"), Test, Timeout(1000)]
        public void RootControllerPutNoApiKeySuccessTest()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;

            NugetPackage packageV000 = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(packageV000.Base64Post);

            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV000.ContentType,
                packageV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(1, PackageDAO.Count().Result);
            Package pkg = PackageDAO.Read().Result.FirstOrDefault();
            Assert.NotNull(pkg);
            Assert.AreEqual(packageV000.Title, pkg.Title);
            Assert.AreEqual(packageV000.Version, pkg.Version);
            Assert.IsTrue(pkg.LatestVersion);

            NugetPackage packageV001 = NugetSamplePackages.nugetoryV001;
            request = Convert.FromBase64String(packageV001.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV001.ContentType,
                packageV001.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(2, PackageDAO.Count().Result);
            Package pkgV000 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV000.Version);
            Package pkgV001 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV001.Version);
            Assert.NotNull(pkgV000);
            Assert.NotNull(pkgV001);
            Assert.AreEqual(packageV000.Title, pkgV000.Title);
            Assert.AreEqual(packageV001.Title, pkgV001.Title);
            Assert.AreEqual(packageV000.Version, pkgV000.Version);
            Assert.AreEqual(packageV001.Version, pkgV001.Version);
            Assert.IsFalse(pkgV000.LatestVersion);
            Assert.IsTrue(pkgV001.LatestVersion);

            PackageDAO.Delete(pkgV000.Id);
            PackageDAO.FileStore.DeleteFile(pkgV000.Id);
            PackageDAO.Delete(pkgV001.Id);
            PackageDAO.FileStore.DeleteFile(pkgV001.Id);
        }
        
        [Category("nugetory.Integration.RootControllerUploadPackage"), Test, Timeout(1000)]
        public void RootControllerPutNoApiKeyFailTest()
        {
            ValidateAuthenticationAttribute.ApiKey = Path.GetRandomFileName().Replace(".", "");
            HttpClient.ApiKey = null;

            NugetPackage packageV000 = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(packageV000.Base64Post);

            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV000.ContentType,
                packageV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Unauthorized, responseCode);
            Assert.AreEqual(0, PackageDAO.Count().Result);
        }
        
        [Category("nugetory.Integration.RootControllerUploadPackage"), Test, Timeout(1000)]
        public void RootControllerPutApiKeySuccessTest()
        {
            ValidateAuthenticationAttribute.ApiKey = Path.GetRandomFileName().Replace(".", "");
            HttpClient.ApiKey = ValidateAuthenticationAttribute.ApiKey;

            NugetPackage packageV000 = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(packageV000.Base64Post);
            
            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV000.ContentType,
                packageV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(1, PackageDAO.Count().Result);
            Package pkg = PackageDAO.Read().Result.FirstOrDefault();
            Assert.NotNull(pkg);
            Assert.AreEqual(packageV000.Title, pkg.Title);
            Assert.AreEqual(packageV000.Version, pkg.Version);
            Assert.IsTrue(pkg.LatestVersion);

            NugetPackage packageV001 = NugetSamplePackages.nugetoryV001;
            request = Convert.FromBase64String(packageV001.Base64Post);
            
            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV001.ContentType,
                packageV001.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(2, PackageDAO.Count().Result);
            Package pkgV000 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV000.Version);
            Package pkgV001 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV001.Version);
            
            Assert.NotNull(pkgV000);
            Assert.NotNull(pkgV001);
            Assert.AreEqual(packageV000.Title, pkgV000.Title);
            Assert.AreEqual(packageV001.Title, pkgV001.Title);
            Assert.AreEqual(packageV000.Version, pkgV000.Version);
            Assert.AreEqual(packageV001.Version, pkgV001.Version);
            Assert.IsFalse(pkgV000.LatestVersion);
            Assert.IsTrue(pkgV001.LatestVersion);

            NugetPackage packageV002 = NugetSamplePackages.nugetoryV002;
            request = Convert.FromBase64String(packageV002.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV002.ContentType,
                packageV002.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(3, PackageDAO.Count().Result);

            NugetPackage packageV003 = NugetSamplePackages.nugetoryV003;
            request = Convert.FromBase64String(packageV003.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, packageV003.ContentType,
                packageV003.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(4, PackageDAO.Count().Result);

            pkgV000 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV000.Version);
            pkgV001 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV001.Version);

            Package pkgV002 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV002.Version);
            Package pkgV003 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV003.Version);
            Assert.NotNull(pkgV000);
            Assert.NotNull(pkgV001);
            Assert.NotNull(pkgV002);
            Assert.NotNull(pkgV003);
            Assert.AreEqual(packageV000.Title, pkgV000.Title);
            Assert.AreEqual(packageV001.Title, pkgV001.Title);
            Assert.AreEqual(packageV002.Title, pkgV002.Title);
            Assert.AreEqual(packageV003.Title, pkgV003.Title);
            Assert.AreEqual(packageV000.Version, pkgV000.Version);
            Assert.AreEqual(packageV001.Version, pkgV001.Version);
            Assert.AreEqual(packageV002.Version, pkgV002.Version);
            Assert.AreEqual(packageV003.Version, pkgV003.Version);
            Assert.IsFalse(pkgV000.LatestVersion);
            Assert.IsFalse(pkgV001.LatestVersion);
            Assert.IsFalse(pkgV002.LatestVersion);
            Assert.IsTrue(pkgV003.LatestVersion);
            
            NugetPackage bookstoreV000 = NugetSamplePackages.bookstoreV000;
            request = Convert.FromBase64String(bookstoreV000.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, bookstoreV000.ContentType,
                bookstoreV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(5, PackageDAO.Count().Result);

            NugetPackage bookstoreV001 = NugetSamplePackages.bookstoreV001;
            request = Convert.FromBase64String(bookstoreV001.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, bookstoreV001.ContentType,
                bookstoreV001.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(6, PackageDAO.Count().Result);
            
            pkgV000 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV000.Version && p.Title == packageV000.Title);
            pkgV001 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV001.Version && p.Title == packageV001.Title);
            pkgV002 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV002.Version && p.Title == packageV002.Title);
            pkgV003 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == packageV003.Version && p.Title == packageV003.Title);
            Package bstoreV000 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == bookstoreV000.Version && p.Title == bookstoreV000.Title);
            Package bstoreV001 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == bookstoreV001.Version && p.Title == bookstoreV001.Title);
            Assert.NotNull(pkgV000);
            Assert.NotNull(pkgV001);
            Assert.NotNull(pkgV002);
            Assert.NotNull(pkgV003);
            Assert.NotNull(bstoreV000);
            Assert.NotNull(bstoreV001);
            Assert.AreEqual(packageV000.Title, pkgV000.Title);
            Assert.AreEqual(packageV001.Title, pkgV001.Title);
            Assert.AreEqual(packageV002.Title, pkgV002.Title);
            Assert.AreEqual(packageV003.Title, pkgV003.Title);
            Assert.AreEqual(bookstoreV000.Title, bstoreV000.Title);
            Assert.AreEqual(bookstoreV001.Title, bstoreV001.Title);
            Assert.AreEqual(packageV000.Version, pkgV000.Version);
            Assert.AreEqual(packageV001.Version, pkgV001.Version);
            Assert.AreEqual(packageV002.Version, pkgV002.Version);
            Assert.AreEqual(packageV003.Version, pkgV003.Version);
            Assert.AreEqual(bookstoreV000.Version, bstoreV000.Version);
            Assert.AreEqual(bookstoreV001.Version, bstoreV001.Version);
            Assert.IsFalse(pkgV000.LatestVersion);
            Assert.IsFalse(pkgV001.LatestVersion);
            Assert.IsFalse(pkgV002.LatestVersion);
            Assert.IsTrue(pkgV003.LatestVersion);
            Assert.IsFalse(bstoreV000.LatestVersion);
            Assert.IsTrue(bstoreV001.LatestVersion);

            PackageDAO.Delete(pkgV000.Id);
            PackageDAO.FileStore.DeleteFile(pkgV000.Id);
            PackageDAO.Delete(pkgV001.Id);
            PackageDAO.FileStore.DeleteFile(pkgV001.Id);
            PackageDAO.Delete(pkgV002.Id);
            PackageDAO.FileStore.DeleteFile(pkgV002.Id);
            PackageDAO.Delete(pkgV003.Id);
            PackageDAO.FileStore.DeleteFile(pkgV003.Id);
            PackageDAO.Delete(bstoreV000.Id);
            PackageDAO.FileStore.DeleteFile(bstoreV000.Id);
            PackageDAO.Delete(bstoreV001.Id);
            PackageDAO.FileStore.DeleteFile(bstoreV001.Id);
        }
    }
}
