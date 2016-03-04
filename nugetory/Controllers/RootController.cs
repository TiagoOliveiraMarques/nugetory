using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using nugetory.Controllers.Helpers;

namespace nugetory.Controllers
{
    public class RootController : ApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            return WorkspaceRoot.GetWorkspace(Request);
        }

        public HttpResponseMessage Put()
        {
            return UploadPackage.Process(Request);
        }
    }
}
