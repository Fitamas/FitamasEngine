using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public enum GUIPinType
    {
        Input,
        Output
    }

    public class GUIPin : GUIComponent, IMouseEvent, IDragMouseEvent
    {
        public static readonly DependencyProperty<GUIPinType> PinTypeProperty = new DependencyProperty<GUIPinType>(GUIPinType.Input, false);

        public static readonly DependencyProperty<bool> IsConnectedProperty = new DependencyProperty<bool>(false, false);

        public GUINode Node { get; set; }
        public GUINodeEditor NodeEditor => Node.NodeEditor;

        public GUIPinType PinType
        {
            get
            {
                return GetValue(PinTypeProperty);
            }
            set
            {
                SetValue(PinTypeProperty, value);
            }
        }

        public bool IsConnected
        {
            get
            {
                return GetValue(IsConnectedProperty);
            }
            set
            {
                SetValue(IsConnectedProperty, value);
            }
        }

        public GUIPin()
        {
            RaycastTarget = true;
        }

        protected override void OnMouseEntered()
        {
            Point mousePosition = InputSystem.mouse.MousePosition;
            Point delta = InputSystem.mouseDelta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                           MouseButton.None, GUINodeEditorEventType.Entered, this);
            NodeEditor.OnPinInteractMouseEvent.Invoke(args);
        }

        protected override void OnMouseExitted()
        {
            Point mousePosition = InputSystem.mouse.MousePosition;
            Point delta = InputSystem.mouseDelta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                                      MouseButton.None, GUINodeEditorEventType.Exitted, this);
            NodeEditor.OnPinInteractMouseEvent.Invoke(args);
        }

        public void OnMovedMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUINodeEditorEventType.Click);
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnStartDragMouse(GUIMouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUINodeEditorEventType.StartDrag);
        }

        public void OnDragMouse(GUIMouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUINodeEditorEventType.Drag);
        }

        public void OnEndDragMouse(GUIMouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUINodeEditorEventType.EndDrag);
        }

        private void InvokeMouseEvent(GUIMouseEventArgs mouse, GUINodeEditorEventType eventType)
        {
            if (!NodeEditor.IsFocused || !IsMouseOver)
            {
                return;
            }

            Point delta = mouse.DistanceMoved;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mouse.Position, delta, mouse.Button, eventType, this);
            NodeEditor.OnPinInteractMouseEvent.Invoke(args);
        }
    }
}
