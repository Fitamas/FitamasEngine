using Fitamas.DebugTools;

namespace Fitamas
{
    public static class Debug
    {
        private static DebugLogger main = DebugLogger.Create("logs");

        public static void Log(string message)
        {
            main.LogRaw(MessegeType.info, message);
        }

        public static void Log(object value)
        {
            Log(value?.ToString());
        }

        public static void LogError(string message)
        {
            main.LogRaw(MessegeType.error, message);
        }

        public static void LogError(object value)
        {
            LogError(value?.ToString());
        }

        public static void LogWarning(string message)
        {
            main.LogRaw(MessegeType.warning, message);
        }

        public static void LogWarning(object value)
        {
            LogWarning(value?.ToString());
        }
    }
}
