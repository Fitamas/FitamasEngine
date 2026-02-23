using Fitamas.Core;
using System;
using System.IO;
using System.Text;

namespace Fitamas.DebugTools
{
    public class DebugLogger
    {
        public static readonly string RootDirectory = Path.Combine(Application.DataPath, "Logs");

        public delegate void LogMessage(MessageType type, DateTime time, string message);

        public static event LogMessage OnLogMessage;

        private StreamWriter memoryStream;

        public DebugLogger(string name)
        {
            Directory.CreateDirectory(RootDirectory);

            string filePath = Path.Combine(RootDirectory, $"{name}.txt");

            memoryStream = new StreamWriter(filePath);
        }

        public void Log(object value)
        {
            Log(value?.ToString());
        }

        public void Log(string message)
        {
            LogRaw(MessageType.info, message);
        }

        public void LogError(object value)
        {
            LogError(value?.ToString());
        }

        public void LogError(string message)
        {
            LogRaw(MessageType.error, message);
        }

        public void LogWarning(object value)
        {
            LogWarning(value?.ToString());
        }

        public void LogWarning(string message)
        {
            LogRaw(MessageType.warning, message);
        }

        public void LogExeption(Exception exception)
        {
            string message = $"{exception.Message}\n{exception.StackTrace}";
            LogError(message);
        }

        public void LogRaw(MessageType type, DateTime time, string message)
        {
            LogRaw($"[{type}] [{time.ToLongTimeString()}] {message}");

            OnLogMessage?.Invoke(type, time, message);
        }

        public void LogRaw(MessageType type, string message)
        {
            LogRaw(type, DateTime.Now, message);
        }

        public void LogRaw(string message)
        {
            memoryStream.WriteLine(message);
            memoryStream.Flush();
            Console.WriteLine(message);
        }
    }

    public enum MessageType
    {
        info,
        warning,
        error
    }
}
