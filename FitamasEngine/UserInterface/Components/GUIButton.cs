﻿using Microsoft.Xna.Framework;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;

namespace Fitamas.UserInterface.Components
{
    public class GUIButton : GUIComponent, IMouseEvent
    {
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

        protected override void OnMouseExitted()
        {
            IsPressed = false;
        }

        public void OnMovedMouse(GUIMouseEventArgs mouse)
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
                    OnClickedButton();
                }
            }
        }

        public void OnScrollMouse(GUIMouseEventArgs mouse)
        {

        }

        protected virtual void OnClickedButton()
        {

        }
    }
}
