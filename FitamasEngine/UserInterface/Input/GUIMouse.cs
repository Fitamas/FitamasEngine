using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fitamas.UserInterface.Input
{
    public class GUIMouse
    {
        public static readonly RoutedEvent MouseMovedEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseDownEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseUpEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseWheelMovedEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseDragStartEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseDragEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseDragEndEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseEnterGUIEvent = new RoutedEvent();

        public static readonly RoutedEvent MouseExitGUIEvent = new RoutedEvent();

        private MouseListener listener;
        private GUIComponent mouseOver;

        public Point Position => listener.MousePosition;
        public bool MouseOnGUI => mouseOver != null;

        public GUIComponent MouseCapture { get; set; }

        public GUIComponent MouseOver 
        { 
            get
            {
                return mouseOver;
            }
            set
            {
                GUIComponent oldMouseOver = mouseOver;
                mouseOver = value;

                if (mouseOver != oldMouseOver)
                {
                    if (oldMouseOver != null)
                    {
                        oldMouseOver.IsMouseOver = false;
                        if (mouseOver == null)
                        {
                            oldMouseOver?.RaiseEvent(new GUIEventArgs(MouseExitGUIEvent, oldMouseOver));
                        }
                    }

                    if (mouseOver != null)
                    {
                        mouseOver.IsMouseOver = true;
                        if (oldMouseOver == null)
                        {
                            mouseOver?.RaiseEvent(new GUIEventArgs(MouseEnterGUIEvent, mouseOver));
                        }
                    }
                }
            }
        }

        public GUIMouse()
        {
            listener = new MouseListener();

            listener.MouseMoved += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseMovedEvent, MouseOver));
            };
            listener.MouseDown += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseDownEvent, MouseOver));
            };
            listener.MouseUp += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseUpEvent, MouseOver));
            };
            listener.MouseWheelMoved += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseWheelMovedEvent, MouseOver));
            };
            listener.MouseDragStart += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseDragStartEvent, MouseOver));
            };
            listener.MouseDrag += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseDragEvent, MouseOver));
            };
            listener.MouseDragEnd += (s, a) =>
            {
                InvokeEvent(new GUIMouseEventArgs(a, MouseDragEndEvent, MouseOver));
            };
        }

        public void Update(GameTime gameTime)
        {
            listener.Update(gameTime);
        }

        private void InvokeEvent(GUIEventArgs args)
        {
            if (MouseCapture != MouseOver)
            {
                MouseCapture?.RaiseEvent(args);
            }

            MouseOver?.RaiseEvent(args);
        }

        static GUIMouse()
        {
            GUIEventManager.Register(MouseMovedEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseMoved));
            GUIEventManager.Register(MouseDownEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseDown));
            GUIEventManager.Register(MouseUpEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseUp));
            GUIEventManager.Register(MouseWheelMovedEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseWheelMoved));
            GUIEventManager.Register(MouseDragStartEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseDragStart));
            GUIEventManager.Register(MouseDragEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseDrag));
            GUIEventManager.Register(MouseDragEndEvent, new GUIEvent<GUIComponent, GUIMouseEventArgs>(OnMouseDragEnd));
        }

        private static void OnMouseMoved(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IMouseEvent mouseEvent)
            {
                mouseEvent.OnMovedMouse(args);
            }
        }

        private static void OnMouseDown(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IMouseEvent mouseEvent)
            {
                mouseEvent.OnClickedMouse(args);
            }
        }

        private static void OnMouseUp(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IMouseEvent mouseEvent)
            {
                mouseEvent.OnReleaseMouse(args);
            }
        }

        private static void OnMouseWheelMoved(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IMouseEvent mouseEvent)
            {
                mouseEvent.OnScrollMouse(args);
            }
        }

        private static void OnMouseDragStart(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IDragMouseEvent mouseEvent)
            {
                mouseEvent.OnStartDragMouse(args);
            }
        }

        private static void OnMouseDrag(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IDragMouseEvent mouseEvent)
            {
                mouseEvent.OnDragMouse(args);
            }
        }

        private static void OnMouseDragEnd(GUIComponent sender, GUIMouseEventArgs args)
        {
            if (sender is IDragMouseEvent mouseEvent)
            {
                mouseEvent.OnEndDragMouse(args);
            }
        }
    }
}
