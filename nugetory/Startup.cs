using System;
using System.Linq;
using nugetory.Console;
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


            if (args.Length >= 1 && args.Any(a => a == "-d"))
            {
                IConsole console = ConsoleFactory.GetConsole();
                
                console.StartAndWait();
            }
            else if (args.Length >= 1 && args.Any(a => a == "-s"))
            {
                IConsole console = ConsoleFactory.GetConsole();
                
                console.Stop();
            }
            else
            {
                Manager m;

                try
                {
                    m = new Manager();

                    m.Start();
                }
                catch (Exception e)
                {
                    _log.Submit(LogLevel.Fatal, "An error occurred when starting server!");
                    _log.SubmitException(e);
                    return;
                }

                _log.Submit(LogLevel.Info, new string('-', 70));
                _log.Submit(LogLevel.Info, "Nugetory started! Press [ENTER] to stop...");
                _log.Submit(LogLevel.Info, new string('-', 70));
                System.Console.ReadLine();

                m.Stop();
            }
        }
    }
}
