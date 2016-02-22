using System;

namespace nugetory.Logging
{
    public class Logger : ILogger
    {
        public Logger(Type type)
        {
            Type = type;
        }

        private Type Type { get; set; }

        public void Submit(LogLevel level, object obj, string context = null)
        {
            LogFactory.Instance.GetLoggerServices().ForEach(l => l.Submit(Type, level, obj, context));
        }

        public void SubmitException(Exception e, string context = null)
        {
            LogFactory.Instance.GetLoggerServices().ForEach(l => l.SubmitException(Type, e, context));
        }
    }
}
