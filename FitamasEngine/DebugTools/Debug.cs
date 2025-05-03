using Fitamas.DebugTools;

namespace Fitamas
{
    public static class Debug
    {
        private static readonly DebugLogger main = new DebugLogger("logs");

        public static void Log(string message)
        {
            main.LogRaw(MessageType.info, message);
        }

        public static void Log(object value)
        {
            Log(value?.ToString());
        }

        public static void LogError(string message)
        {
            main.LogRaw(MessageType.error, message);
        }

        public static void LogError(object value)
        {
            LogError(value?.ToString());
        }

        public static void LogWarning(string message)
        {
            main.LogRaw(MessageType.warning, message);
        }

        public static void LogWarning(object value)
        {
            LogWarning(value?.ToString());
        }
    }
}
