using Microsoft.Xna.Framework;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components
{
    public class GUIButton : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<bool> IsPressedProperty = new DependencyProperty<bool>();

        public static readonly RoutedEvent OnClickedEvent = new RoutedEvent();

        public MonoEvent<GUIButton> OnClicked { get; }

        public bool IsPressed
        {
            get
            {
                return GetValue(IsPressedProperty);
            }
            set
            {
                SetValue(IsPressedProperty, value);
            }
        }

        public GUIButton()
        {
            RaycastTarget = true;
            OnClicked = new MonoEvent<GUIButton>();
        }

        protected override void OnMouseExitted()
        {
            IsPressed = false;
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                if (Interacteble && mouse.Button == MouseButton.Left)
                {
                    IsPressed = true;
                    Focus();
                }
            }
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {
            if (IsPressed)
            {
                IsPressed = false;
                if (IsMouseOver)
                {
                    OnClicked.Invoke(this);
                    RaiseEvent(new GUIEventArgs(OnClickedEvent, this));
                    OnClickedButton();
                }
            }
        }

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
        {

        }

        protected virtual void OnClickedButton()
        {

        }
    }
}
