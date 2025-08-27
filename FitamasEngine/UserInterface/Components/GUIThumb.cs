using Fitamas.Events;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;

namespace Fitamas.UserInterface.Components
{
    public class GUIDragEventArgs : GUIEventArgs
    {
        public Point Position { get; set; }
        public Point Delta { get; set; }

        public GUIDragEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }
    }

    public class GUIThumb : GUIComponent, IDragMouseEvent
    {
        public static readonly DependencyProperty<bool> IsDragProperty = new DependencyProperty<bool>(false, false);

        public static readonly RoutedEvent DragStartEvent = new RoutedEvent();

        public static readonly RoutedEvent DragDeltaEvent = new RoutedEvent();

        public static readonly RoutedEvent DragEndEvent = new RoutedEvent();

        public MonoEvent<GUIThumb, GUIDragEventArgs> DragStart { get; }
        public MonoEvent<GUIThumb, GUIDragEventArgs> DragDelta { get; }
        public MonoEvent<GUIThumb, GUIDragEventArgs> DragEnd { get; }

        public Point StartDragOffset { get; private set; }

        public bool IsDrag
        {
            get
            {
                return GetValue(IsDragProperty);
            }
            protected set
            {
                SetValue(IsDragProperty, value);
            }
        }

        public GUIThumb()
        {
            RaycastTarget = true;

            DragStart = new MonoEvent<GUIThumb, GUIDragEventArgs>();
            DragDelta = new MonoEvent<GUIThumb, GUIDragEventArgs>();
            DragEnd = new MonoEvent<GUIThumb, GUIDragEventArgs>();
        }

        public void OnStartDragMouse(GUIMouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                Manager.Mouse.MouseCapture = this;
                IsDrag = true;
                StartDragOffset = mouse.Position - Rectangle.Location;
                GUIDragEventArgs args = new GUIDragEventArgs(DragStartEvent, this)
                {
                    Position = mouse.Position
                };
                DragStart.Invoke(this, args);
                RaiseEvent(args);
            }
        }

        public void OnDragMouse(GUIMouseEventArgs mouse)
        {
            if (IsDrag)
            {
                GUIDragEventArgs args = new GUIDragEventArgs(DragDeltaEvent, this)
                {
                    Position = mouse.Position,
                    Delta = mouse.Delta
                };
                DragDelta.Invoke(this, args);
                RaiseEvent(args);
            }
        }

        public void OnEndDragMouse(GUIMouseEventArgs mouse)
        {
            if (IsDrag)
            {
                Manager.Mouse.MouseCapture = null;
                IsDrag = false;
                GUIDragEventArgs args = new GUIDragEventArgs(DragEndEvent, this)
                {
                    Position = mouse.Position,
                };
                DragEnd.Invoke(this, args);
                RaiseEvent(args);
            }
        }
    }
}
