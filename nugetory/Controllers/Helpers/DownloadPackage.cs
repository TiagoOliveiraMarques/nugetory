using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Exceptions;

namespace nugetory.Controllers.Helpers
{
    public static class DownloadPackage
    {
        public static PackageDAO PackageDAO { get; set; }
        public static IFileStore FileStore { get; set; }

        public static HttpResponseMessage Download(HttpRequestMessage request, string id, string version)
        {
            Package package = PackageDAO.Read(id, version).Result;
            if (package == null)
                throw new PackageNotFoundException();
            Stream fileStream = FileStore.GetFile(package.Id, package.PackageHash);

            HttpResponseMessage res = request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StreamContent(fileStream);

            res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            res.Content.Headers.ContentMD5 = Convert.FromBase64String(package.PackageHash);

            return res;
        }
    }
}
