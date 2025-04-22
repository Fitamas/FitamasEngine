using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUIRoot : GUIComponent
    {
        private GUIWindow screen;
        private List<GUIWindow> windows;

        public GUIWindow PopupWindow { get; set; }

        public GUIWindow Screen
        {
            get
            {
                if (screen == null)
                {
                    screen = new GUIWindow();
                    screen.SetAlignment(GUIAlignment.Stretch);
                    AddChild(screen);
                }

                return screen;
            }
            set
            {
                if (screen != null)
                {
                    screen.Close();
                }

                screen = value;

                if (screen != null)
                {
                    AddChild(screen);
                }
            }
        }

        public GUIRoot()
        {
            IsFocusScope = true;

            windows = new List<GUIWindow>();
        }

        public override void RaycastAll(Point point, List<GUIComponent> result)
        {
            bool flag = false;
            foreach (GUIWindow window in windows)
            {
                if (window.IsOnTop)
                {
                    window.RaycastAll(point, result);
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                if (screen != null)
                {
                    screen.RaycastAll(point, result);
                }

                foreach (GUIWindow window in windows)
                {
                    window.RaycastAll(point, result);
                }
            }

            if (PopupWindow != null)
            {
                PopupWindow.RaycastAll(point, result);
            }
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            if (screen != null)
            {
                screen.Draw(gameTime, context);
            }

            foreach (GUIWindow window in windows)
            {
                window.Draw(gameTime, context);
            }

            if (PopupWindow != null)
            {
                PopupWindow.Draw(gameTime, context);
            }
        }

        public void OpenWindow(GUIWindow window)
        {
            if (!windows.Contains(window))
            {
                windows.Add(window);
                AddChild(window);
            }
        }

        internal void CloseWindow(GUIWindow window)
        {
            if (screen == window)
            {
                screen = null;
            }

            windows.Remove(window);

            if (PopupWindow == window && window.Parent is GUIPopup popup)
            {
                popup.Destroy();
            }
        }
    }
}
