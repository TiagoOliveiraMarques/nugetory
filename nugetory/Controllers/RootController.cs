using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class RootController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof(RootController));

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            Log.Submit(LogLevel.Debug, "GET request received");
            HttpResponseMessage response = WorkspaceRoot.GetWorkspace(Request);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }

        public HttpResponseMessage Put()
        {
            Log.Submit(LogLevel.Debug, "PUT request received");
            HttpResponseMessage response = UploadPackage.Process(Request);
            Log.Submit(LogLevel.Debug, "PUT response ready");
            return response;
        }
    }
}
