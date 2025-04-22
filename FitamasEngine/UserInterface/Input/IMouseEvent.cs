using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.UserInterface.Input
{
    public interface IMouseEvent
    {
        void OnMovedMouse(GUIMouseEventArgs mouse);
        void OnClickedMouse(GUIMouseEventArgs mouse);
        void OnReleaseMouse(GUIMouseEventArgs mouse);
        void OnScrollMouse(GUIMouseEventArgs mouse);
    }
}
