﻿using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;

namespace Fitamas.UserInterface.Components
{
    public class DragEventArgs
    {
        public Point Position { get; set; }
        public Point Delta { get; set; }
    }

    public class GUIThumb : GUIComponent, IDragMouseEvent
    {
        public static readonly DependencyProperty<bool> IsDragProperty = new DependencyProperty<bool>(false, false);

        public static readonly RoutedEvent DragStartEvent = new RoutedEvent();

        public static readonly RoutedEvent DragDeltaEvent = new RoutedEvent();

        public static readonly RoutedEvent DragEndEvent = new RoutedEvent();

        public GUIEvent<GUIThumb, DragEventArgs> DragStart { get; }
        public GUIEvent<GUIThumb, DragEventArgs> DragDelta { get; }
        public GUIEvent<GUIThumb, DragEventArgs> DragEnd { get; }

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

            DragStart = eventHandlersStore.Create<GUIThumb, DragEventArgs>(DragStartEvent);
            DragDelta = eventHandlersStore.Create<GUIThumb, DragEventArgs>(DragDeltaEvent);
            DragEnd = eventHandlersStore.Create<GUIThumb, DragEventArgs>(DragEndEvent);
        }

        public void OnStartDragMouse(MouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                IsDrag = true;
                StartDragOffset = mouse.Position - Rectangle.Location;
                DragStart.Invoke(this, new DragEventArgs() { Position = mouse.Position });
            }
        }

        public void OnDragMouse(MouseEventArgs mouse)
        {
            if (IsDrag)
            {
                DragDelta.Invoke(this, new DragEventArgs() { Position = mouse.Position, Delta = mouse.DistanceMoved.ToPoint() });
            }
        }

        public void OnEndDragMouse(MouseEventArgs mouse)
        {
            if (IsDrag)
            {
                IsDrag = false;
                DragEnd.Invoke(this, new DragEventArgs() { Position = mouse.Position });
            }
        }
    }
}
