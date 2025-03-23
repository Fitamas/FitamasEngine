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
        public static readonly DependencyProperty<bool> IsPressedProperty = new DependencyProperty<bool>();

        public static readonly RoutedEvent OnClickedEvent = new RoutedEvent();

        public GUIEvent<GUIButton, ClickButtonEventArgs> OnClicked { get; }

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
            OnClicked = eventHandlersStore.Create<GUIButton, ClickButtonEventArgs>(OnClickedEvent);
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
                    OnClicked.Invoke(this, new ClickButtonEventArgs() { MousePosition = mouse.Position, Button = mouse.Button});
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

    public class ClickButtonEventArgs
    {
        public Point MousePosition { get; set; }
        public MouseButton Button { get; set; }
    }
}
