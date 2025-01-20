using System;
using System.IO;
using System.Text;

namespace Fitamas.DebugTools
{
    public class DebugLogger : TextWriter
    {
        public const string Root = "Logs";
        private StreamWriter memoryStream;
        private string path;

        public delegate void LogMessage(MessegeType type, DateTime time, string message);

        public static event LogMessage OnLogMessage;

        public static DebugLogger Create(string name)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Root);
            string filePath = Path.Combine(folderPath, $"{name}.txt");

            Directory.CreateDirectory(folderPath);

            return new DebugLogger()
            {
                path = filePath,
                memoryStream = new StreamWriter(filePath),
            };
        }

        protected override void Dispose(bool disposing)
        {
            memoryStream.Close();
            memoryStream.Dispose();
            base.Dispose(disposing);
        }

        public override void WriteLine(string value)
        {
            LogRaw(value);
            base.WriteLine(value);
        }

        public void LogRaw(MessegeType type, DateTime time, string message)
        {
            string typeStr = "[" + type.ToString() + "] ";
            string date = "[" + time.ToLongTimeString() + "] ";
            LogRaw(typeStr + date + message);

            OnLogMessage?.Invoke(type, time, message);
        }

        public void LogRaw(MessegeType type, string message)
        {
            LogRaw(type, DateTime.Now, message);
        }

        public void LogRaw(string message)
        {
            memoryStream.WriteLine(message);
            memoryStream.Flush();
            Console.WriteLine(message);
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

    public enum MessegeType
    {
        info,
        warning,
        error
    }
}
