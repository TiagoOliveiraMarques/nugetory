using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class PackageDetailsController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof(PackageDetailsController));

        [AllowAnonymous]
        public HttpResponseMessage Get(string id, string version)
        {
            Log.Submit(LogLevel.Debug, "GET request received");
            HttpResponseMessage response = PackageDetails.BuildResponse(Request, id, version);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }

        public HttpResponseMessage Delete(string id, string version)
        {
            Log.Submit(LogLevel.Debug, "DELETE request received");
            HttpResponseMessage response = DeletePackage.Delete(Request, id, version);
            Log.Submit(LogLevel.Debug, "DELETE response ready");
            return response;
        }
    }
}
