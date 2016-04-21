using System;
using System.Net;
using nugetory.Data.DAO;
using NUnit.Framework;

namespace nugetory.tests.Support
{
    internal static class LoadData
    {
        public static void UploadAllPackages(PackageDAO packageDAO, string invokeUrl)
        {
            NugetPackage packageV000 = NugetSamplePackages.nugetoryV000;
            byte[] request = Convert.FromBase64String(packageV000.Base64Post);

            HttpStatusCode responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, packageV000.ContentType,
                                                            packageV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(1, packageDAO.Count().Result);

            NugetPackage packageV001 = NugetSamplePackages.nugetoryV001;
            request = Convert.FromBase64String(packageV001.Base64Post);

            responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, packageV001.ContentType,
                                             packageV001.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(2, packageDAO.Count().Result);

            NugetPackage packageV002 = NugetSamplePackages.nugetoryV002;
            request = Convert.FromBase64String(packageV002.Base64Post);

            responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, packageV002.ContentType,
                                             packageV002.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(3, packageDAO.Count().Result);

            NugetPackage packageV003 = NugetSamplePackages.nugetoryV003;
            request = Convert.FromBase64String(packageV003.Base64Post);

            responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, packageV003.ContentType,
                                             packageV003.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(4, packageDAO.Count().Result);

            NugetPackage bookstoreV000 = NugetSamplePackages.bookstoreV000;
            request = Convert.FromBase64String(bookstoreV000.Base64Post);

            responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, bookstoreV000.ContentType,
                                             bookstoreV000.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(5, packageDAO.Count().Result);

            NugetPackage bookstoreV001 = NugetSamplePackages.bookstoreV001;
            request = Convert.FromBase64String(bookstoreV001.Base64Post);

            responseCode = HttpClient.Invoke(invokeUrl, "PUT", request, bookstoreV001.ContentType,
                                             bookstoreV001.ContentLength);

            Assert.AreEqual(HttpStatusCode.Created, responseCode);
            Assert.AreEqual(6, packageDAO.Count().Result);
        }
    }
}
