using System;
using nugetory.Logging;

namespace nugetory
{
    internal class Startup
    {
        private static ILogger _log;

        private static void Main(string[] args)
        {
            LogFactory.ForceConsoleMode = true;

            _log = LogFactory.Instance.GetLogger(typeof(Startup));

            Manager m = new Manager();

            try
            {
                m.Start();
            }
            catch (Exception e)
            {
                _log.Submit(LogLevel.Fatal, "An error occurred when starting server!");
                _log.SubmitException(e);
                Environment.Exit(1);
            }
            
            _log.Submit(LogLevel.Info, new string('-', 70));
            _log.Submit(LogLevel.Info, "Nugetory started! Press [ENTER] to stop...");
            _log.Submit(LogLevel.Info, new string('-', 70));
            Console.ReadLine();

            m.Stop();
        }
    }
}
