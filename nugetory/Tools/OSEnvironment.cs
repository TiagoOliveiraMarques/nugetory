namespace nugetory.Tools
{
    public static class OSEnvironment
    {
        public static bool IsUnix
        {
            get
            {
                int p = (int) System.Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}
