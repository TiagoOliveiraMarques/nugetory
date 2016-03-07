using nugetory.Data;
using NUnit.Framework;

namespace nugetory.tests
{
    [SetUpFixture]
    public class SetUp
    {
        private static Manager _manager;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Logging.LogFactory.ForceConsoleMode = true;
            DataManager.DataInMemory = true;

            // start nugetory server
            _manager = new Manager();

            _manager.Start();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // stop nugetory server
            _manager.Stop();
        }
    }
}
