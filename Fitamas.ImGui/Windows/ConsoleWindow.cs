using Fitamas.DebugTools;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.ImGuiNet.Windows
{
    public class ConsoleWindow : EditorWindow
    {
        private List<string> messagePool;

        public ConsoleWindow() : base("Console")
        {

        }

        protected override void OnOpen()
        {
            DebugLogger.OnLogMessage += OnLog;
            messagePool = new List<string>();
        }

        protected override void OnGUI(GameTime gameTime)
        {
            foreach (var message in messagePool)
            {
                ImGui.Text(message);
            }
        }

        private void OnLog(MessageType type, DateTime time, string message)
        {
            string typeStr = "[" + type.ToString() + "] ";
            string date = "[" + time.ToLongTimeString() + "] ";
            string result = typeStr + date + message;
            messagePool.Add(result);

            if (messagePool.Count > 10)
            {
                messagePool.RemoveAt(0);
            }
        }
    }
}
