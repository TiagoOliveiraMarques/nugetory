using System;
using System.IO;
using System.Linq;
using System.Net;
using nugetory.Controllers.Helpers;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Endpoint;
using nugetory.tests.Support;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class RootControllerUploadPackageTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            PackageDAO = SetUp.Manager.DataManager.PackageDAO;
        }

        public static PackageDAO PackageDAO { get; set; }

        [Category("nugetory.Integration.RootControllerUploadPackage"), Test, Timeout(1000)]
        public void RootControllerUploadPackageNoApiKeySuccessTest()
        {
            ValidateAuthenticationAttribute.ApiKey = null;

            NugetPackage package = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(package.Base64Post);

            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, package.ContentType,
                package.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(1, PackageDAO.Count().Result);
            Package pkg = PackageDAO.Read().Result.FirstOrDefault();
            Assert.NotNull(pkg);
            Assert.AreEqual("nugetory", pkg.Title);
            Assert.AreEqual("0.0.0", pkg.Version);

            package = NugetSamplePackages.nugetoryV001;
            request = Convert.FromBase64String(package.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, package.ContentType, package.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(2, PackageDAO.Count().Result);
            Package pkg1 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == "0.0.0");
            Package pkg2 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == "0.0.1");
            Assert.NotNull(pkg1);
            Assert.NotNull(pkg2);
            Assert.AreEqual("nugetory", pkg1.Title);
            Assert.AreEqual("nugetory", pkg2.Title);
            Assert.AreEqual("0.0.0", pkg1.Version);
            Assert.AreEqual("0.0.1", pkg2.Version);

            PackageDAO.Delete(pkg1.Id);
            PackageDAO.FileStore.DeleteFile(pkg1.Id);
            PackageDAO.Delete(pkg2.Id);
            PackageDAO.FileStore.DeleteFile(pkg2.Id);
        }

        [Category("nugetory.Integration.RootControllerUploadPackage"), Test, Timeout(1000)]
        public void RootControllerUploadPackageApiKeySuccessTest()
        {
            ValidateAuthenticationAttribute.ApiKey = Path.GetRandomFileName().Replace(".", "");

            NugetPackage package = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(package.Base64Post);

            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, package.ContentType,
                package.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(1, PackageDAO.Count().Result);
            Package pkg = PackageDAO.Read().Result.FirstOrDefault();
            Assert.NotNull(pkg);
            Assert.AreEqual("nugetory", pkg.Title);
            Assert.AreEqual("0.0.0", pkg.Version);

            package = NugetSamplePackages.nugetoryV001;
            request = Convert.FromBase64String(package.Base64Post);

            responseCode = HttpClient.Invoke(InvokeUrl, "PUT", request, package.ContentType, package.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(2, PackageDAO.Count().Result);
            Package pkg1 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == "0.0.0");
            Package pkg2 = PackageDAO.Read().Result.FirstOrDefault(p => p.Version == "0.0.1");
            Assert.NotNull(pkg1);
            Assert.NotNull(pkg2);
            Assert.AreEqual("nugetory", pkg1.Title);
            Assert.AreEqual("nugetory", pkg2.Title);
            Assert.AreEqual("0.0.0", pkg1.Version);
            Assert.AreEqual("0.0.1", pkg2.Version);

            PackageDAO.Delete(pkg1.Id);
            PackageDAO.FileStore.DeleteFile(pkg1.Id);
            PackageDAO.Delete(pkg2.Id);
            PackageDAO.FileStore.DeleteFile(pkg2.Id);
        }
    }
}
