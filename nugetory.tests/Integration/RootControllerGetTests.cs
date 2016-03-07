using System;
using System.Net;
using nugetory.Controllers.Helpers;
using nugetory.tests.Support;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class RootControllerGetTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

        [Category("nugetory.Integration.RootControllerGet"), Test, Timeout(1000)]
        public void RootControllerGetTest()
        {
            string response;
            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl, "GET", out response);

            Assert.AreEqual(HttpStatusCode.OK, responseCode);
            Assert.AreEqual(WorkspaceRoot.GetWorkspaceContent(InvokeUrl), response);
        }
    }
}
