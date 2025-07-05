using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUIRoot : GUIComponent
    {
        private GUIWindow screen;
        private List<GUIWindow> windows;
        private GUIWindow messageBox;
        private GUIPopup popup;

        public GUIPopup Popup 
        { 
            get
            {
                return popup;
            }
            set
            {
                if (popup != value)
                {
                    if (popup != null)
                    {
                        popup.IsOpen = false;
                    }
                    popup = value;
                    if (popup != null)
                    {
                        popup.IsOpen = true;
                    }
                }
            }
        }

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

        protected override void OnInit()
        {
            messageBox = GUIMessageBox.Instance.Init(this);
        }

        public override void RaycastAll(Point point, List<GUIComponent> result)
        {
            if (messageBox != null && messageBox.Enable)
            {
                messageBox.RaycastAll(point, result);
                return;
            }

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

            if (Popup != null)
            {
                Popup.RaycastAll(point, result);
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

            if (Popup != null)
            {
                Popup.Draw(gameTime, context);
            }

            if (messageBox != null)
            {
                messageBox.Draw(gameTime, context);
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

            if (Popup != null && Popup.Window == window)
            {
                Popup.Destroy();
            }
        }
    }
}
