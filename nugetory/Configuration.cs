using System.IO;
using nugetory.Tools;

namespace nugetory
{
    public class Configuration
    {
        static Configuration()
        {
            Port = 9000;

            if (OSEnvironment.IsUnix)
            {
                const string baseDirectory = "/srv/nugetory";

                if (!Directory.Exists(baseDirectory))
                    Directory.CreateDirectory(baseDirectory);

                UploadDirectory = "/upload/";
                PackagesDirectory = "/packages/";
                ConfigurationFile = "/config.json";
            }
            else
            {
                const string baseDirectory = @"C:\ProgramData\nugetory";

                if (!Directory.Exists(baseDirectory))
                    Directory.CreateDirectory(baseDirectory);

                UploadDirectory = baseDirectory + @"\upload\";
                PackagesDirectory = baseDirectory + @"\packages\";
                ConfigurationFile = baseDirectory + @"\config.json";
            }
        }

        public static int Port { get; set; }
        public static string ConfigurationFile { get; set; }
        public static string UploadDirectory { get; set; }
        public static string PackagesDirectory { get; set; }
    }
}
