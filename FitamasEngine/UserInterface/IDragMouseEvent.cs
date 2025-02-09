using MonoGame.Extended.Input.InputListeners;
using System;

namespace Fitamas.UserInterface
{
    public interface IDragMouseEvent
    {
        void OnStartDragMouse(MouseEventArgs mouse);
        void OnDragMouse(MouseEventArgs mouse);
        void OnEndDragMouse(MouseEventArgs mouse);
    }
}
