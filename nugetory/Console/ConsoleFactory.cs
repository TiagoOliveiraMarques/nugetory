using nugetory.Tools;

namespace nugetory.Console
{
    internal static class ConsoleFactory
    {
        public static IConsole GetConsole()
        {
            if (OSEnvironment.IsUnix)
                return new UnixConsole();
            return new WindowsConsole();
        }
    }
}
