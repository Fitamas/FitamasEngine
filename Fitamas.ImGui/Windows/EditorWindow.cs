using Fitamas.DebugTools;
using Fitamas.Serialization;
using Fitamas.Serialization.Json;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Fitamas.ImGuiNet.Windows
{
    public abstract class EditorWindow
    {
        private class WindowSettings
        {
            public List<EditorWindow> Windows = new List<EditorWindow>();
        }

        internal static readonly string SettingsFile = Path.Combine(EditorPrefs.RootDirectory, "WindowSettings.json");
        internal static readonly string IniFile = Path.Combine(EditorPrefs.RootDirectory, "imgui.ini");

        private static List<EditorWindow> windows = new List<EditorWindow>();
        private static float saveTimer = 0f;

        public static Vector2 DefaultSize = new Vector2(500, 300);
        public static float SaveInterval = 5f;
        public static bool EnableAutoSave = true;

        internal static IReadOnlyList<EditorWindow> Windows => windows;

        [SerializeField] private int id;
        [SerializeField] private string name;

        private bool enable;
        private Vector2 position;
        private Vector2 size;

        protected bool focusedWindow;
        protected bool mouseOverWindow;

        public ImGuiWindowFlags Flags;

        public string Name => name;
        public bool Enable => enable;
        internal string FullName => $"{Name}##{id}";

        public EditorWindow(string name = "")
        {
            this.name = string.IsNullOrEmpty(name) ? GetType().Name : name;
        }

        public static void OpenWindow(EditorWindow window)
        {
            if (!windows.Contains(window))
            {
                int index = 0;
                IEnumerable<EditorWindow> enumerable = windows.Where(x => x.name == window.name);
                while(enumerable.FirstOrDefault(x => x.id == index) != null)
                {
                    index++;
                }

                windows.Add(window);
                window.id = index;
                window.enable = true;
                window.OnOpen();
            }
        }

        public static void CloseWindow(EditorWindow window)
        {
            window.enable = false;
        }

        internal static void LoadData()
        {
            ImGui.LoadIniSettingsFromDisk(IniFile);

            if (!File.Exists(SettingsFile))
            {
                return;
            }

            foreach (var window in windows)
            {
                window.enable = false;
                window.OnClose();
            }
            windows.Clear();

            WindowSettings settings = JsonUtility.Load<WindowSettings>(SettingsFile);
            if (settings != null)
            {
                foreach (var window in settings.Windows)
                {
                    windows.Add(window);
                    window.enable = true;
                    window.OnOpen();
                }
            }
        }

        internal static void SaveData()
        {
            WindowSettings settings = new WindowSettings();
            foreach (var window in windows)
            {
                if (!window.enable || window.Flags.HasFlag(ImGuiWindowFlags.NoSavedSettings))
                {
                    continue;
                }

                settings.Windows.Add(window);
            }

            ImGui.SaveIniSettingsToDisk(IniFile);
            JsonUtility.Save(SettingsFile, settings);
        }

        internal static void Draw(GameTime gameTime)
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
            {
                EditorWindow window = Windows[i];
                window.OnDrawGUI(gameTime);

                if (!window.enable)
                {
                    window.OnClose();
                    windows.RemoveAt(i);
                }
            }

            if (!EnableAutoSave)
            {
                return;
            }

            saveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (saveTimer >= SaveInterval)
            {
                SaveData();
                saveTimer = 0f;
            }
        }

        internal static void DrawScene(GameTime gameTime)
        {
            Gizmos.Begin();
            foreach (var window in windows)
            {
                window.OnScene(gameTime);
            }
            Gizmos.End();
        }

        private void OnDrawGUI(GameTime gameTime)
        {
            if (!enable)
            {
                return;
            }


            //TODO set size and pos
            //ImGui.SetNextWindowPos(position.ToNumerics(), ImGuiCond.FirstUseEver);
            //ImGui.SetNextWindowSize(size.ToNumerics(), ImGuiCond.FirstUseEver);
            //ImGui.SetNextWindowCollapsed(layout.IsCollapsed, ImGuiCond.FirstUseEver);


            if (ImGui.Begin(FullName, ref enable, Flags))
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

                if (enable)
                {
                    OnGUI(gameTime);
                }
            }

            ImGui.End();
        }

        public void Close()
        {
            CloseWindow(this);
        }

        public virtual void OnOpenEditor() { }

        public virtual void OnCloseEditor() { }

        protected virtual void OnOpen() { }

        protected virtual void OnGUI(GameTime gameTime) { }
        
        protected virtual void OnScene(GameTime gameTime) { }

        protected virtual void OnClose() { }

        protected virtual void OnFocus() { }

        protected virtual void OnLostFocus() { }
    }
}