using nugetory.Configuration;
using nugetory.Controllers.Helpers;
using nugetory.Data;
using nugetory.Endpoint;
using nugetory.Logging;

namespace nugetory
{
    public class Manager
    {
        private readonly ILogger _log;

        public Store ConfigurationStore { get; private set; }
        private DataManager DataManager { get; set; }

        public Manager()
        {
            _log = LogFactory.Instance.GetLogger(GetType());

            _log.Submit(LogLevel.Info, "Setting up configuration store");

            ConfigurationStore = new Store();

            _log.Submit(LogLevel.Info, "Configuration store setup finished");

            _log.Submit(LogLevel.Info, "Setting up Data Manager");

            DataManager = new DataManager(ConfigurationStore);

            _log.Submit(LogLevel.Info, "Data Manager setup finished");
        }

        public void Start()
        {
            _log.Submit(LogLevel.Info, "Starting configuration store");

            ConfigurationStore.Start();

            _log.Submit(LogLevel.Info, "Configuration store started ");

            _log.Submit(LogLevel.Info, "Starting Data Manager");

            DataManager.Start();

            _log.Submit(LogLevel.Info, "Data Manager started");

            _log.Submit(LogLevel.Info, "Setting up environment");

            UploadPackage.SetUploadDirectory(ConfigurationStore.DatabaseUploadDirectory.Value);

            // inject
            UploadPackage.PackageDAO = DataManager.PackageDAO;
            DownloadPackage.PackageDAO = DataManager.PackageDAO;
            DownloadPackage.FileStore = DataManager.FileStore;
            PackageDetails.PackageDAO = DataManager.PackageDAO;
            PackageDetails.FileStore = DataManager.FileStore;
            DeletePackage.PackageDAO = DataManager.PackageDAO;
            DeletePackage.FileStore = DataManager.FileStore;
            FindPackage.PackageDAO = DataManager.PackageDAO;

            _log.Submit(LogLevel.Info, "Environment setup finished");

            _log.Submit(LogLevel.Info, "Starting Nuget server");

            OwinHost.Start(ConfigurationStore.ServerPort.Value, ConfigurationStore.ApiKey.Value);

            _log.Submit(LogLevel.Info, "Nuget server started");
        }

        public void Stop()
        {
            _log.Submit(LogLevel.Info, "Stopping Nuget server");

            OwinHost.Stop();

            _log.Submit(LogLevel.Info, "Nuget server stopped");

            _log.Submit(LogLevel.Info, "Stopping Data Manager");

            DataManager.Stop();

            _log.Submit(LogLevel.Info, "Data Manager stopped");
        }
    }
}
