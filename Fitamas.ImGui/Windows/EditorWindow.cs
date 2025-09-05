using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet.Windows
{
    public abstract class EditorWindow
    {
        private int id;
        private bool enable = true;

        protected ImGuiManager manager;
        protected bool focusedWindow;
        protected bool mouseOverWindow;

        public string Name { get; set; }

        public bool Enable => enable;

        public EditorWindow()
        {

        }

        internal void Initialize(ImGuiManager manager, int id)
        {
            this.manager = manager;
            this.id = id;
            enable = true;
            OnOpen();
        }

        public void DrawGUI(GameTime gameTime)
        {
            if (!enable)
            {
                return;
            }

            bool flag = enable;
            string windowName = $"{Name}##{id}";
            if (ImGui.Begin(windowName, ref flag))
            {
                bool isWindowFocused = ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows);

                if (focusedWindow != isWindowFocused)
                {
                    focusedWindow = isWindowFocused;

                    if (focusedWindow)
                    {
                        OnFocus();
                    }
                    else
                    {
                        OnLostFocus();
                    }
                }

                mouseOverWindow = ImGui.IsWindowHovered(ImGuiHoveredFlags.RootAndChildWindows | ImGuiHoveredFlags.AllowWhenBlockedByActiveItem);

                if (flag)
                {
                    OnGUI(gameTime);
                }
            }

            if (!flag)
            {
                Close();
            }

            ImGui.End();
        }

        public void Close()
        {
            enable = false;
            OnClose();
        }

        public virtual void OnOpenEditor() { }

        public virtual void OnCloseEditor() { }

        protected virtual void OnOpen() { }

        protected virtual void OnGUI(GameTime gameTime) { }

        protected virtual void OnClose() { }

        protected virtual void OnFocus() { }

        protected virtual void OnLostFocus() { }
    }
}