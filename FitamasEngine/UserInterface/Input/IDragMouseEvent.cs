using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.UserInterface.Input
{
    public interface IDragMouseEvent
    {
        void OnStartDragMouse(GUIMouseEventArgs mouse);
        void OnDragMouse(GUIMouseEventArgs mouse);
        void OnEndDragMouse(GUIMouseEventArgs mouse);
    }
}
