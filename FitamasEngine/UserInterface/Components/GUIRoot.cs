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
        private GUIWindow popup;

        public GUICanvas Canvas => canvas;
        public GUIFrame MainFrame => mainFrame;
        public GUIFrame PopupFrame => popupFrame;

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

                if (mainWindow != null)
                {
                    mainFrame.AddChild(mainWindow);
                    mainWindow.SetAsFirstSibling();
                }
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

        public void OpenWindow(GUIWindow window)
        {
            if (!windows.Contains(window))
            {
                windows.Add(window);
                mainFrame.AddChild(window);
            }
        }

        public void CloseWindow(GUIWindow window)
        {
            if (windows.Remove(window))
            {
                window.Close();
            }
        }

        public void CloseAllWindows()
        {
            foreach (var window in windows.ToArray())
            {
                CloseWindow(window);
            }
        }

        public void OpenPopup(GUIWindow newPopup)
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
