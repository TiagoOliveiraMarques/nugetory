using System;

namespace nugetory.Logging
{
    public interface ILoggerService
    {
        void Submit(Type type, LogLevel level, object obj, string context = null);
        void SubmitException(Type type, Exception e, string context = null);
    }
}
