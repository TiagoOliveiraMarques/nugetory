using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;

namespace nugetory.Controllers
{
    public class PackageDownloadController : ApiController
    {
        public HttpResponseMessage Get(string id, string version)
        {
            return DownloadPackage.Download(Request, id, version);
        }
    }
}
