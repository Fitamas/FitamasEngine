using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Input;
using System;
using System.Linq;
using Fitamas.UserInterface.Themes;
using R3;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components
{
    public class GUIButton : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<GUIStyle> ColorProperty = new DependencyProperty<GUIStyle>();

        public static readonly DependencyProperty<bool> IsPressedProperty = new DependencyProperty<bool>();

        public static readonly RoutedEvent OnClickedEvent = new RoutedEvent();

        public GUIEvent<GUIButton> OnClicked { get; }

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
            OnClicked = eventHandlersStore.Create<GUIButton>(OnClickedEvent);
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                if (Interecteble && mouse.Button == MouseButton.Left)
                {
                    IsPressed = true;
                }
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {
            if (IsPressed)
            {
                IsPressed = false;
                OnClicked?.Invoke(this);
                OnClickedButton(mouse);
            }
        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }

        protected virtual void OnClickedButton(MouseEventArgs mouse)
        {

        }
    }
}
