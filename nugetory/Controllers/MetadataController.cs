using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;
using nugetory.Logging;

namespace nugetory.Controllers
{
    public class MetadataController : ApiController
    {
        private static readonly ILogger Log = LogFactory.Instance.GetLogger(typeof (MetadataController));

        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            Log.Submit(LogLevel.Debug, "GET request received");
            HttpResponseMessage response = Metadata.GetMetadata(Request);
            Log.Submit(LogLevel.Debug, "GET response ready");
            return response;
        }
    }
}
