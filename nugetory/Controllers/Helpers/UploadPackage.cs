using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpMultipartParser;
using nugetory.Data.DAO;
using nugetory.Exceptions;
using nugetory.Logging;

namespace nugetory.Controllers.Helpers
{
    public static class UploadPackage
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof (UploadPackage));
        public static PackageDAO PackageDAO { get; set; }

        public static HttpResponseMessage Process(HttpRequestMessage request)
        {
            // Check if the request contains multipart/form-data.
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                Stream reqStream = request.Content.ReadAsStreamAsync().Result;
                MemoryStream tempStream = new MemoryStream();
                reqStream.CopyTo(tempStream);

                tempStream.Seek(0, SeekOrigin.End);
                Log.Submit(LogLevel.Debug, "Upload request has " + tempStream.Length + " bytes");
                tempStream.Position = 0;
                
                StreamContent streamContent = new StreamContent(tempStream);
                foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
                {
                    streamContent.Headers.Add(header.Key, header.Value);
                    Log.Submit(LogLevel.Debug, "Header " + header.Key + ": " + string.Join(",", header.Value));
                }

                MultipartFormDataParser parser = new MultipartFormDataParser(tempStream);

                // This illustrates how to get the file names.
                FilePart file = parser.Files.FirstOrDefault();
                if (parser.Files == null || parser.Files.Count != 1)
                    throw new InvalidOperationException();
                if (file == null || file.FileName != "package")
                    throw new InvalidOperationException();

                PackageDAO.ProcessPackage(file.Data);

                return request.CreateResponse(HttpStatusCode.Created);
            }
            catch (AlreadyExistsException e)
            {
                Log.SubmitException(e);
                return request.CreateErrorResponse(HttpStatusCode.Conflict, e);
            }
            catch (InvalidOperationException e)
            {
                Log.SubmitException(e);
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            catch (Exception e)
            {
                Log.SubmitException(e);
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
