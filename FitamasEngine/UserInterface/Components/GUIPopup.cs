using Fitamas.Events;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using ObservableCollections;
using System;

namespace Fitamas.UserInterface.Components
{
    public enum GUIPlacementMode
    {
        Left,
        Top,
        Right,
        Bottom,
        Center,
        MousePoint,
    }

    public class GUIPopup : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<GUIPlacementMode> PlacementModeProperty = new DependencyProperty<GUIPlacementMode>(GUIPlacementMode.Center, false);

        public static readonly DependencyProperty<bool> IsOpenProperty = new DependencyProperty<bool>(IsOpenChangedCallback, false, false);

        public static readonly RoutedEvent OnOpenEvent = new RoutedEvent();

        public static readonly RoutedEvent OnCloseEvent = new RoutedEvent();

        private GUIWindow window;

        public MonoEvent<GUIPopup> OnOpen { get; }
        public MonoEvent<GUIPopup> OnClose { get; }

        public GUIPlacementMode PlacementMode
        {
            get
            {
                return GetValue(PlacementModeProperty);
            }
            set
            {
                SetValue(PlacementModeProperty, value);
            }
        }

        public bool IsOpen
        {
            get
            {
                return GetValue(IsOpenProperty);
            }
            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        public GUIWindow Window 
        {
            get
            {
                return window;
            }
            set
            {
                if (window != value)
                {
                    window = value;
                    window.Enable = IsOpen;
                    UpdatePosition();
                }
            }
        }

        public GUIPopup()
        {
            OnOpen = new MonoEvent<GUIPopup>();
            OnClose = new MonoEvent<GUIPopup>();
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            if (component == window)
            {
                window = null;
            }
        }

        private void UpdatePosition()
        {
            //TODO
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {
            UpdatePosition();
        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            if (Window != null && !Window.Contains(mouse.Position))
            {
                IsOpen = false;
            }
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {
            if (Window != null && !Window.Contains(mouse.Position))
            {
                IsOpen = false;
            }
        }

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
        {

        }

        private void OpenPopup()
        {
            System.Root.PopupWindow = Window;
            System.Mouse.MouseCapture = this;
            OnOpen.Invoke(this);
            RaiseEvent(new GUIEventArgs(OnOpenEvent, this));
        }

        private void ClosePopup()
        {
            System.Root.PopupWindow = null;
            System.Mouse.MouseCapture = null;
            OnClose.Invoke(this);
            RaiseEvent(new GUIEventArgs(OnCloseEvent, this));
        }

        private static void IsOpenChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (dependencyObject is GUIPopup popup)
            {
                if (popup.Window != null)
                {
                    popup.Window.Enable = newValue;
                }

                if (newValue)
                {
                    popup.OpenPopup();
                }
                else
                {
                    popup.ClosePopup();
                }
            }
        }
    }
}
