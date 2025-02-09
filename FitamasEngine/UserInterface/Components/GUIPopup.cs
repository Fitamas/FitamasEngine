using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Components
{
    public class GUIPopup : GUIWindow, IMouseEvent
    {
        public GUIComponent Target { get; set; }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (!Contain(mouse.Position)) //TODO fix
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
