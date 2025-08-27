using System;
using System.IO;
using System.Reflection;

namespace Fitamas.Core
{
    public static class Application
    {
        public static Version Version = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0, 0);

        public static string Name = Assembly.GetEntryAssembly()?.GetName().Name ?? nameof(Fitamas);

        public static readonly string DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(Fitamas), Name);

        public static readonly float Dpi = 96f; //TODO

        static Application()
        {
            Directory.CreateDirectory(DataPath);
        }
    }
}
