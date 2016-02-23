using nugetory.Tools;

namespace nugetory.Configuration
{
    internal class ConfigDefaults
    {
        // --- LOGGING

        public static ConfigItemDefaults<int> LoggingLevel = new ConfigItemDefaults<int>("LOGGING", "LEVEL", 5);

        // --- SERVER

        public static ConfigItemDefaults<int> ServerPort = new ConfigItemDefaults<int>("SERVER", "PORT", 9000);

        // --- API KEY

        public static ConfigItemDefaults<string> ApiKey = new ConfigItemDefaults<string>("API", "KEY", null);

        // --- DATABASE

        private static readonly string DefaultDatabaseDirectory = OSEnvironment.IsUnix
                                                                      ? "/srv/nugetory/"
                                                                      : @"C:\ProgramData\nugetory\";

        public static ConfigItemDefaults<string> DatabaseFile = new ConfigItemDefaults<string>("DATABASE", "FILE",
                                                                                               DefaultDatabaseDirectory +
                                                                                               "config.json");

        public static ConfigItemDefaults<string> DatabaseUploadDirectory = new ConfigItemDefaults<string>("DATABASE",
                                                                                                          "UPLOAD",
                                                                                                          DefaultDatabaseDirectory +
                                                                                                          "upload");

        public static ConfigItemDefaults<string> DatabasePackagesDirectory = new ConfigItemDefaults<string>("DATABASE",
                                                                                                            "PACKAGES",
                                                                                                            DefaultDatabaseDirectory +
                                                                                                            "packages");
    }
}
