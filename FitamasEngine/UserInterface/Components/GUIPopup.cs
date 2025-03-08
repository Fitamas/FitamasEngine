using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUIPopup : GUIWindow, IMouseEvent
    {
        public GUIComponent Target { get; set; }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (!Contains(mouse.Position))
            {
                Close();
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {
            
        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {
            
        }
    }
}
