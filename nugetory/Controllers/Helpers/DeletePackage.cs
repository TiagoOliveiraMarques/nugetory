using System;
using System.Net;
using System.Net.Http;
using nugetory.Data.DAO;
using nugetory.Data.Entities;
using nugetory.Data.File;
using nugetory.Exceptions;

namespace nugetory.Controllers.Helpers
{
    public static class DeletePackage
    {
        public static PackageDAO PackageDAO { get; set; }
        public static IFileStore FileStore { get; set; }

        public static HttpResponseMessage Delete(HttpRequestMessage request, string id, string version)
        {
            Package package = PackageDAO.Read(id, version).Result;
            if (package == null)
                throw new PackageNotFoundException();
            if (!FileStore.DeleteFile(package.Id))
                throw new InternalServerErrorException();
            if (!PackageDAO.Delete(package.Id).Result)
                throw new InternalServerErrorException();

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
