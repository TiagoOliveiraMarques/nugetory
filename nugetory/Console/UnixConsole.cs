using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Mono.Unix;
using Mono.Unix.Native;
using nugetory.Logging;

namespace nugetory.Console
{
    internal class UnixConsole : IConsole, IDisposable
    {
        private Manager Manager { get; set; }
        private static ILogger _log;

        private ManualResetEvent StopResetEvent { get; set; }

        public UnixConsole()
        {
            _log = LogFactory.Instance.GetLogger(GetType());
            StopResetEvent = new ManualResetEvent(false);
        }

        public void StartAndWait()
        {
            try
            {
                Manager = new Manager();

                StartSignalsListener();

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
            _log.Submit(LogLevel.Info, "Received a signal to terminate. Sending event to terminate.");
            StopResetEvent.Set();
        }

        #region Unix Signals

        private void StartSignalsListener()
        {
            new Thread(() =>
                       {
                           // on mono, processes will usually run as daemons - this allows you to listen
                           // for termination signals (ctrl+c, shutdown, etc) and finalize correctly
                           List<UnixSignal> signals = new List<UnixSignal>
                           {
                               new UnixSignal(Signum.SIGTERM),
                               new UnixSignal(Signum.SIGQUIT)
                           };

                           _log.Submit(LogLevel.Info, "Waiting for any signal to terminate (SIGTERM; SIGQUIT)");

                           if (!signals.Any(s => s.IsSet))
                               UnixSignal.WaitAny(signals.ToArray());

                           _log.Submit(LogLevel.Info, "Received a signal to terminate. Sending event to terminate.");

                           StopResetEvent.Set();
                       })
            {
                IsBackground = true,
                Name = "nugetory.Console.UnixConsole.SignalsListener"
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
