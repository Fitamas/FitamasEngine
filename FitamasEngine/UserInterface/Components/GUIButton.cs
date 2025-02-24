using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Fitamas.UserInterface.Themes;
using R3;
using Fitamas.Events;
using Fitamas.Input;
using Fitamas.Input.InputListeners;

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
                if (Interacteble && mouse.Button == MouseButton.Left)
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
                if (IsMouseOver)
                {
                    OnClicked.Invoke(this);
                    OnClickedButton(mouse);
                }
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
