using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using nugetory.Data.DAO;
using nugetory.Exceptions;

namespace nugetory.Controllers.Helpers
{
    public static class UploadPackage
    {
        public static PackageDAO PackageDAO { get; set; }

        public static async Task<HttpResponseMessage> Process(HttpRequestMessage request)
        {
            // Check if the request contains multipart/form-data.
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = Configuration.UploadDirectory;
            MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                MultipartFileData file = provider.FileData.FirstOrDefault();
                if (provider.FileData.Count != 1 || file == null)
                    throw new InvalidOperationException();
                if (file.Headers.ContentDisposition.FileName != "\"package\"")
                    throw new InvalidOperationException();

                PackageDAO.ProcessPackage(file.LocalFileName);

                return request.CreateResponse(HttpStatusCode.Created);
            }
            catch (AlreadyExistsException e)
            {
                return request.CreateErrorResponse(HttpStatusCode.Conflict, e);
            }
            catch (InvalidOperationException e)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            catch (Exception e)
            {
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
