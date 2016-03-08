using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class PackageDownloadController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof(PackageDownloadController));

        [AllowAnonymous]
        public HttpResponseMessage Get(string id, string version)
        {
            Log.Submit(LogLevel.Debug, "GET request received");
            HttpResponseMessage response = DownloadPackage.Download(Request, id, version);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }
    }
}
