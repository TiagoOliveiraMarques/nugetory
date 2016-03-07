using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using nugetory.Controllers.Helpers;

namespace nugetory.Controllers
{
    public class PackageController : ApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            return WorkspaceRoot.GetWorkspace(Request);
        }
    }
}
