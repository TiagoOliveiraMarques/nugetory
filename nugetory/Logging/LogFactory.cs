using System;
using System.Collections.Generic;
using System.Diagnostics;
using nugetory.Configuration;

namespace nugetory.Logging
{
    public class LogFactory
    {
        public static Store ConfigurationStore { get; set; }

        public static bool ForceConsoleMode { get; set; }

        private LogLevel LogLevel { get; set; }

        private List<ILoggerService> LoggerServices { get; set; }

        public static readonly LogFactory Instance = new LogFactory();

        public static void SetupLogging(Store configStore)
        {
            ConfigurationStore = configStore;

            Instance.LogLevel = GetLogLevel(ConfigurationStore.LoggingLevel.Value);

            Instance.LoggerServices = CreateLoggerServices();
        }

        public ILogger GetLogger()
        {
            StackFrame frame = new StackFrame(1, false);

            Type t = frame.GetMethod() == null ? GetType() : frame.GetMethod().DeclaringType;

            return GetLogger(t);
        }

        public ILogger GetLogger(Type type)
        {
            return new Logger(type);
        }

        internal List<ILoggerService> GetLoggerServices()
        {
            if (LoggerServices != null)
                return LoggerServices;

            List<ILoggerService> services = CreateLoggerServices();

            if (ConfigurationStore != null)
                LoggerServices = services;

            return services;
        }

        private static List<ILoggerService> CreateLoggerServices()
        {
            return ForceConsoleMode ? GetLoggerConsole() : GetLoggerLog4Net();
        }

        private static List<ILoggerService> GetLoggerConsole()
        {
            return new List<ILoggerService>
            {
                new LoggerConsole()
            };
        }

        private static List<ILoggerService> GetLoggerLog4Net()
        {
            return new List<ILoggerService>
            {
                new Logger4Net()
            };
        }

        public LogLevel GetLogLevel()
        {
            return LogLevel;
        }

        private static LogLevel GetLogLevel(int level)
        {
            if (level <= 1)
                return LogLevel.Fatal;
            if (level >= 5)
                return LogLevel.Debug;

            switch (level)
            {
                case 1:
                    return LogLevel.Debug;
                case 2:
                    return LogLevel.Info;
                case 3:
                    return LogLevel.Warn;
                case 4:
                    return LogLevel.Error;
                case 5:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Debug;
            }
        }

        public LogLevel GetHigherLogLevel()
        {
            switch (GetLogLevel())
            {
                case LogLevel.Debug:
                    return LogLevel.Info;
                case LogLevel.Info:
                    return LogLevel.Warn;
                case LogLevel.Warn:
                    return LogLevel.Error;
                case LogLevel.Error:
                    return LogLevel.Fatal;
                case LogLevel.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Info;
            }
        }
    }
}
