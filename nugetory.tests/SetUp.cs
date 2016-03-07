using nugetory.Data;
using NUnit.Framework;

namespace nugetory.tests
{
    [SetUpFixture]
    public class SetUp
    {
        internal static Manager Manager;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Logging.LogFactory.ForceConsoleMode = true;
            DataManager.DataInMemory = true;

            // start nugetory server
            Manager = new Manager();

            Manager.Start();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // stop nugetory server
            Manager.Stop();
        }
    }
}
