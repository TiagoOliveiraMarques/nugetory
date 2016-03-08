using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class PackageController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof(PackageController));

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            Log.Submit(LogLevel.Debug, "GET request received");
            HttpResponseMessage response = WorkspaceRoot.GetWorkspace(Request);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }
    }
}
