using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Input
{
    public interface IKeyboardEvent
    {
        void OnKeyDown(GUIKeyboardEventArgs args);
        void OnKeyUP(GUIKeyboardEventArgs args);
        void OnKey(GUIKeyboardEventArgs args);
    }
}
