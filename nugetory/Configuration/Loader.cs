using System.IO;
using System.Reflection;
using nugetory.Logging;
using nugetory.Tools;

namespace nugetory.Configuration
{
    public class Loader
    {
        private static readonly ILogger Log;

        internal const string ConfigurationFilename = "nugetory.ini";
        internal const string ConfigurationFolderUnix = @"/etc/nugetory/";
        internal const string ConfigurationFolderWindows = @"C:\ProgramData\nugetory\";
        internal static string ConfigurationFilenameUnix = ConfigurationFolderUnix + ConfigurationFilename;
        internal static string ConfigurationFilenameWindows = ConfigurationFolderWindows + ConfigurationFilename;

        static Loader()
        {
            Log = LogFactory.Instance.GetLogger();
        }

        public static ConfigurationParser Initialize()
        {
            Log.Submit(LogLevel.Info, "Initializing Configuration Manager");

            string configurationFilename = GetConfigurationFilename(OSEnvironment.IsUnix);

            if (configurationFilename == null)
            {
                Log.Submit(LogLevel.Info, "No valid configuration file found!");
                return null;
            }

            ConfigurationParser cm = new ConfigurationParser(configurationFilename);

            Log.Submit(LogLevel.Info,
                        "Configuration Manager initialized with configuration file " + configurationFilename);

            return cm;
        }

        internal static string GetConfigurationFilename(bool unix)
        {
            string filename;

            if (unix)
            {
                Log.Submit(LogLevel.Info, "System is unix, trying configuration file " + ConfigurationFilenameUnix);
                filename = ConfigurationFilenameUnix;
            }
            else
            {
                Log.Submit(LogLevel.Info,
                            "System is windows, trying configuration file " + ConfigurationFilenameWindows);
                filename = ConfigurationFilenameWindows;
            }

            if (!File.Exists(filename))
            {
                Log.Submit(LogLevel.Info,
                            "System configuration file not found, using configuration file in assembly location");
                filename = GetAssemblyConfigurationFilename();
            }

            return File.Exists(filename) ? filename : null;
        }

        internal static string GetAssemblyConfigurationFilename()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(assembly) + Path.DirectorySeparatorChar + ConfigurationFilename;
        }
    }
}
