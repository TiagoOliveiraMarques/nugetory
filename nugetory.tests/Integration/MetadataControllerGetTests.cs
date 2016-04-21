using System.Net;
using nugetory.Controllers.Helpers;
using nugetory.Endpoint;
using nugetory.tests.Support;
using NUnit.Framework;

namespace nugetory.tests.Integration
{
    [TestFixture]
    public class MetadataControllerGetTests
    {
        private const string InvokeUrl = "http://localhost:9000/api/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ValidateAuthenticationAttribute.ApiKey = null;
            HttpClient.ApiKey = null;
        }

        [Category("nugetory.Integration.MetadataControllerGet"), Test, Timeout(1000)]
        public void MetadataControllerGetTest()
        {
            string response;
            HttpStatusCode responseCode = HttpClient.Invoke(InvokeUrl + "/$metadata", "GET", out response);

            Assert.AreEqual(HttpStatusCode.OK, responseCode);
            Assert.AreEqual(Metadata.GetMetadataContent(), response);
        }
    }
}
