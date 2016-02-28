using System;
using System.Threading;
using Newtonsoft.Json;

namespace nugetory.Logging
{
    internal sealed class LoggerConsole : ILoggerService
    {
        public void Submit(Type type, LogLevel level, object obj, string context = null)
        {
            if (LogFactory.Instance.GetLogLevel() > level)
                return;

            try
            {
                string timeStamp = DateTime.Now.ToString("O");
                string threadName = Thread.CurrentThread.Name;
                string message = obj as string ?? JsonConvert.SerializeObject(obj);

                System.Console.WriteLine("{0} [{1}] {2} {3} {4}", timeStamp, threadName,
                    level.ToString().ToUpperInvariant(),
                    type.FullName, message);
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
    }
}
