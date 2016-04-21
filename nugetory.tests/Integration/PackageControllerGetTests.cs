using System.Net;
using nugetory.Controllers.Helpers;
using nugetory.Endpoint;
using nugetory.tests.Support;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class PackageControllerGetTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;
        }

        [Category("nugetory.Integration.PackageControllerGet"), Test, Timeout(1000)]
        public void PackageControllerGetTest()
        {
            string response;
            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl + "/package", "GET", out response);

            Assert.AreEqual(HttpStatusCode.OK, responseCode);
            Assert.AreEqual(WorkspaceRoot.GetWorkspaceContent(InvokeUrl + "/package"), response);
        }
    }
}
