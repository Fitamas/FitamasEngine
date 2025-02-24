using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.UserInterface
{
    public interface IMouseEvent
    {
        void OnClickedMouse(MouseEventArgs mouse);
        void OnReleaseMouse(MouseEventArgs mouse);
        void OnScrollMouse(MouseEventArgs mouse);
    }
}
