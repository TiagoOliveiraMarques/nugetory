using System;
using System.Text;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using nugetory.Tools;
using Newtonsoft.Json;

namespace nugetory.Logging
{
    internal sealed class Logger4Net : ILoggerService
    {
        private static ILog _log;

        private const string LogsFolderUnix = @"/var/log/nugetory/";
        private const string LogsFolderWindows = @"C:\ProgramData\nugetory\logs\";

        private static readonly int Started;

        static Logger4Net()
        {
            if (Interlocked.CompareExchange(ref Started, 1, 0) == 1)
                return;

            Configure();
        }

        public static void Configure()
        {
            IAppender rollingFileAppender = GetRollingFileAppender(GetLogFolder());
            BasicConfigurator.Configure(rollingFileAppender);
            ((Hierarchy) LogManager.GetRepository()).Root.Level = Level.Debug;

            _log = LogManager.GetLogger(typeof(Logger4Net));
        }

        private static string GetLogFolder()
        {
            return OSEnvironment.IsUnix ? LogsFolderUnix : LogsFolderWindows;
        }

        private static IAppender GetRollingFileAppender(string logFolder)
        {
            PatternLayout layout = new PatternLayout("%date [%thread] %-5level %logger %message%newline");
            layout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender
            {
                File = logFolder,
                AppendToFile = true,
                DatePattern = "'nugetory'.yyyy-MM-dd.'log'",
                RollingStyle = RollingFileAppender.RollingMode.Date,
                StaticLogFileName = false,
                Encoding = Encoding.UTF8,
                Threshold = Level.Debug,
                Layout = layout,
                LockingModel = new FileAppender.MinimalLock()
            };

            appender.ActivateOptions();

            return appender;
        }

        public void Submit(Type type, LogLevel level, object obj, string context = null)
        {
            if (LogFactory.Instance.GetLogLevel() > level)
                return;

            try
            {
                if (context == null)
                    context = Guid.NewGuid().ToString();

                string message = obj as string ?? JsonConvert.SerializeObject(obj);

                using (NDC.Push(context))
                {
                    LoggingEvent loggingEvent = new LoggingEvent(new LoggingEventData
                    {
                        TimeStamp = DateTime.Now,
                        Level = ConvertLogLevel(level),
                        Message = message,
                        LoggerName = type.FullName
                    });
                    _log.Logger.Log(loggingEvent);
                }
            }
            catch (Exception)
            {
                // ignore, can't log
            }
        }

        public void SubmitException(Type type, Exception e, string context = null)
        {
            if (LogFactory.Instance.GetLogLevel() > LogLevel.Error)
                return;

            try
            {
                string message = e + ": " + e.Message + Environment.NewLine + e.StackTrace;
                Submit(type, LogLevel.Error, message, context);
            }
            catch (Exception)
            {
                // ignore, can't log                                                                           
            }
        }

        private static Level ConvertLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Critical;
                case LogLevel.Fatal:
                    return Level.Fatal;
                default:
                    return Level.Warn;
            }
        }
    }
}
