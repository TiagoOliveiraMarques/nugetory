using System.Net.Http;
using System.Web.Http;
using nugetory.Controllers.Helpers;

namespace nugetory.Controllers
{
    public class PackageDetailsController : ApiController
    {
        public HttpResponseMessage Get(string id, string version)
        {
            return PackageDetails.BuildResponse(Request, id, version);
        }

        public HttpResponseMessage Delete(string id, string version)
        {
            return DeletePackage.Delete(Request, id, version);
        }
    }
}
