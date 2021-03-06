﻿using System;
using System.Collections.Generic;
using System.IO;
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
    public class PackageDetailsControllerGetDetailsTests
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

        [Category("nugetory.Integration.PackageDetailsControllerGet"), Test, Timeout(1000)]
        public void PackageDetailsControllerGetTest()
        {
            const string suffix = "/Packages(Id='{0}',Version='{1}')";
            string responseData;

            Package pkgV000 = PackageDAO.Read(NugetSamplePackages.nugetoryV000.Title,
                                              NugetSamplePackages.nugetoryV000.Version).Result;

            string urlSuffix = string.Format(suffix, pkgV000.Title, pkgV000.Version);
            string url = InvokeUrl + urlSuffix;
            HttpClient.Invoke(url, "GET", out responseData);

            
            Uri uri = new Uri(InvokeUrl + EscapeSuffix(urlSuffix));
            string expected = GetDetailsResult(pkgV000, uri);

            Assert.AreEqual(expected, responseData);
        }

        private string EscapeSuffix(string urlSuffix)
        {
            return urlSuffix.Replace("=", "%3D").Replace(",", "%2C");
        }

        private static string GetDetailsResult(Package package, Uri uri)
        {
            entry entry = PackageDetails.GetPackageEntry(package, uri);

            XmlSerializer serializer = new XmlSerializer(typeof(entry));
            MemoryStream ms = new MemoryStream();
            serializer.Serialize(ms, entry);
            ms.Position = 0;
            string result = new StreamReader(ms).ReadToEnd();
            return result;
        }
    }
}
