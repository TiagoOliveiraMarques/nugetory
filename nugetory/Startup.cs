using System;
using nugetory.Data;
using nugetory.Endpoint;

namespace nugetory
{
    internal class Startup
    {
        private static void Main(string[] args)
        {
            DataManager dm = new DataManager();

            dm.Start();
            OwinHost.Start(Configuration.Port);

            Console.ReadLine();

            OwinHost.Stop();
            dm.Stop();
        }
    }
}
