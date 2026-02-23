using Fitamas.DebugTools;
using System;

namespace Fitamas
{
    public static class Debug
    {
        private static readonly DebugLogger main = new DebugLogger("logs");

        public static void Log(string message)
        {
            main.Log(message);
        }

        public static void Log(object value)
        {
            main.Log(value);
        }

        public static void LogError(string message)
        {
            main.LogError(message);
        }

        public static void LogError(object value)
        {
            main.LogError(value);
        }

        public static void LogWarning(string message)
        {
            main.LogWarning(message);
        }

        public static void LogWarning(object value)
        {
            main.LogWarning(value);
        }

        public static void LogExeption(Exception exception)
        {
            main.LogExeption(exception);
        }
    }
}
