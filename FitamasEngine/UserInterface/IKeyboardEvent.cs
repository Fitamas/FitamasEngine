using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface
{
    public interface IKeyboardEvent
    {
        void OnKeyDown(KeyboardEventArgs args);
        void OnKeyUP(KeyboardEventArgs args);
        void OnKey(KeyboardEventArgs args);
    }
}
