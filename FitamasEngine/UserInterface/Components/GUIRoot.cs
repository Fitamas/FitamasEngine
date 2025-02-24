using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUIRoot : GUIComponent
    {
        private GUICanvas canvas;
        private GUIFrame mainFrame;
        private GUIFrame popupFrame;
        private List<GUIWindow> windows = new List<GUIWindow>();
        private GUIWindow mainWindow;
        private GUIPopup popup;

        public GUIWindow MainWindow
        {
            get
            {
                if (mainWindow == null)
                {
                    mainWindow = new GUIWindow();
                    mainWindow.SetAlignment(GUIAlignment.Stretch);

                    mainFrame.AddChild(mainWindow);
                }

                return mainWindow;
            }
            set
            {
                if (mainWindow != null)
                {
                    mainWindow.Close();
                }

                mainWindow = value;
            }
        }

        public GUIRoot()
        {
            canvas = new GUICanvas();
            canvas.Pivot = new Vector2(0, 0);
            AddChild(canvas);

            mainFrame = new GUIFrame();
            mainFrame.SetAlignment(GUIAlignment.Stretch);
            canvas.AddChild(mainFrame);

            popupFrame = new GUIFrame();
            popupFrame.SetAlignment(GUIAlignment.Stretch);
            canvas.AddChild(popupFrame);
        }

        public void AddComponent(GUIComponent component)
        {
            mainFrame.AddChild(component);
        }

        public void OpenWindow(GUIWindow window)
        {
            if (!windows.Contains(window))
            {
                if (mainWindow == null)
                {
                    mainWindow = window;
                }

                windows.Add(window);
                mainFrame.AddChild(window);
            }
        }

        public void CloseWindow(GUIWindow window)
        {
            if (mainWindow == window)
            {
                mainWindow = null;
            }

            windows.Remove(window);
            window.Destroy();
        }

        public void CloseAllWindows()
        {
            foreach (var window in windows.ToArray())
            {
                window.Close();
            }
        }

        public void OpenPopup(GUIPopup newPopup)
        {
            if (popup != newPopup)
            {
                ClosePopup();

                popup = newPopup;

                if (newPopup != null)
                {
                    newPopup.Open();
                    popupFrame.AddChild(newPopup);
                }
            }
        }

        public void ClosePopup()
        {
            if (popup != null)
            {
                popup.Close();
            }
        }
    }
}
