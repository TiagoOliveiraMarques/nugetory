using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using nugetory.Logging;

namespace nugetory.Console
{
    internal class WindowsConsole : IConsole, IDisposable
    {
        public const string BliquaResetEventHandlerName = @"nugetory";

        private Manager Manager { get; set; }
        private static ILogger _log;

        private ManualResetEvent StopResetEvent { get; set; }

        public WindowsConsole()
        {
            _log = LogFactory.Instance.GetLogger(GetType());
            StopResetEvent = new ManualResetEvent(false);
        }

        public void StartAndWait()
        {
            try
            {
                StartEventListener();

                Manager = new Manager();

                _log.Submit(LogLevel.Info, "Starting Services");

                Manager.Start();

                _log.Submit(LogLevel.Info, "Services started. Waiting for stop signal");

                StopResetEvent.WaitOne();

                Manager.Stop();
            }
            catch (Exception e)
            {
                _log.SubmitException(e);
            }

            Environment.Exit(0);
        }

        public void Stop()
        {
            EventWaitHandle manualResetEvent = new EventWaitHandle(false, EventResetMode.ManualReset,
                BliquaResetEventHandlerName);
            manualResetEvent.Set();
        }

        #region Event Listener

        private void StartEventListener()
        {
            new Thread(() =>
                       {
                           EventWaitHandle manualResetEvent = new EventWaitHandle(false, EventResetMode.ManualReset,
                               BliquaResetEventHandlerName);

                           manualResetEvent.WaitOne();

                           StopResetEvent.Set();
                       })
            {
                IsBackground = true,
                Name = "nugetory.Console.UnixConsole.EventListener"
            }.Start();
        }

        #endregion

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Microsoft.Usage", "CA2213", Justification = "StopResetEvent is being disposed.")]
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                StopResetEvent.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
