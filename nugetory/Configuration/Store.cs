using nugetory.Logging;

namespace nugetory.Configuration
{
    public class Store
    {
        private readonly ILogger _log;

        private ConfigurationParser ConfigurationParser { get; set; }

        public string ConfigurationFilename { get; private set; }

        public ConfigItemInt LoggingLevel { get; private set; }

        public ConfigItemInt ServerPort { get; private set; }

        public ConfigItemString ApiKey { get; private set; }

        public ConfigItemString DatabaseFile { get; private set; }
        public ConfigItemString DatabaseUploadDirectory { get; private set; }
        public ConfigItemString DatabasePackagesDirectory { get; private set; }

        public Store()
        {
            _log = LogFactory.Instance.GetLogger(GetType());
        }

        public void Start()
        {
            _log.Submit(LogLevel.Info, "Initializing configuration store");

            ConfigurationParser = Loader.Initialize();
            if (ConfigurationParser != null)
            {
                ConfigurationFilename = ConfigurationParser.INIFilePath;
                _log.Submit(LogLevel.Info, "Using configuration file " + ConfigurationFilename);
            }

            LoggingLevel = new ConfigItemInt(ConfigurationParser, ConfigDefaults.LoggingLevel);

            ServerPort = new ConfigItemInt(ConfigurationParser, ConfigDefaults.ServerPort);

            ApiKey = new ConfigItemString(ConfigurationParser, ConfigDefaults.ApiKey);

            DatabaseFile = new ConfigItemString(ConfigurationParser, ConfigDefaults.DatabaseFile);
            DatabaseUploadDirectory = new ConfigItemString(ConfigurationParser, ConfigDefaults.DatabaseUploadDirectory);
            DatabasePackagesDirectory = new ConfigItemString(ConfigurationParser,
                                                             ConfigDefaults.DatabasePackagesDirectory);

            _log.Submit(LogLevel.Info, "Configuration store initialized");
        }
    }
}
