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
