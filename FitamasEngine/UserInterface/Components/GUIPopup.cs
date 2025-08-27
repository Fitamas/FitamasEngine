using Fitamas.Events;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;

namespace Fitamas.UserInterface.Components
{
    public enum GUIPlacementMode
    {
        Left,
        Top,
        Right,
        Bottom,
        MousePoint,
        Mouse,
    }

    public class GUIPopup : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<GUIPlacementMode> PlacementModeProperty = new DependencyProperty<GUIPlacementMode>(GUIPlacementMode.MousePoint, false);

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

        public void Place(Rectangle rectangle)
        {
            if (window == null || !IsOpen)
            {
                return;
            }

            switch (PlacementMode)
            {
                case GUIPlacementMode.Left:
                    window.LocalPosition = ToLocal(rectangle.Location); //TODO
                    break;
                case GUIPlacementMode.Top:
                    window.LocalPosition = ToLocal(rectangle.Location);
                    break;
                case GUIPlacementMode.Right:
                    window.LocalPosition = ToLocal(rectangle.Location);
                    break;
                case GUIPlacementMode.Bottom:
                    window.LocalPosition = ToLocal(rectangle.Location);
                    break;
                case GUIPlacementMode.MousePoint:
                    window.LocalPosition = ToLocal(rectangle.Location);
                    break;
                case GUIPlacementMode.Mouse:
                    UpdatePosition(rectangle.Location);
                    break;
            }
        }

        public void UpdatePosition(Point point)
        {
            if (window == null || !IsOpen)
            {
                return;
            }

            if (PlacementMode == GUIPlacementMode.Mouse)
            {
                window.LocalPosition = ToLocal(point);
            }
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {
            UpdatePosition(mouse.Position);
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
            if (!IsInHierarchy)
            {
                return;
            }

            Manager.Root.Popup = this;
            Manager.Mouse.MouseCapture = this;
            Enable = true;
            OnOpen.Invoke(this);
            RaiseEvent(new GUIEventArgs(OnOpenEvent, this));
            UpdatePosition(Manager.Mouse.Position);
        }

        private void ClosePopup()
        {
            if (!IsInHierarchy)
            {
                return;
            }

            Enable = false;
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
