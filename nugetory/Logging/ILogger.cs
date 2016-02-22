using System;

namespace nugetory.Logging
{
    public interface ILogger
    {
        void Submit(LogLevel level, object obj, string context = null);
        void SubmitException(Exception e, string context = null);
    }
}
