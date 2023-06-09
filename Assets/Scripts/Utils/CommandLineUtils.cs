using System;

namespace Utils
{
    public static class CommandLineUtils
    {
        //Parse command lines arguments. Example -port 8000
        public static string GetCommandLineValueFromKey(string key)
        {
            string[] args = System.Environment.GetCommandLineArgs ();
            for (int i = 0; i < args.Length; i++) {
                if (args[i].Equals($"-{key}", StringComparison.OrdinalIgnoreCase)) {
                    return args[i + 1];
                }
            }

            return string.Empty;
        }
    }
}