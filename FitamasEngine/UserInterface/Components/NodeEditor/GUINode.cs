using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUINode : GUIHeaderedItemsControl, IMouseEvent, IDragMouseEvent
    {
        public static readonly DependencyProperty<bool> IsSelectProperty = new DependencyProperty<bool>(false, false);

        public GUIStack LeftStack { get; set; }
        public GUIStack RightStack { get; set; }

        public GUINodeEditor NodeEditor { get; set; }

        public bool IsSelect
        {
            get
            {
                return GetValue(IsSelectProperty);
            }
            set
            {
                SetValue(IsSelectProperty, value);
            }
        }

        public GUINode()
        {
            RaycastTarget = true;
        }

        protected override bool IsItemItsOwnContainerOverride(GUIComponent component)
        {
            return component is GUINodeItem;
        }

        protected override void OnAddItem(GUIComponent component)
        {
            if (component is GUINodeItem item)
            {
                item.Node = this;

                if (item.Alignment == GUINodeItemAlignment.Left)
                {
                    LeftStack?.AddChild(item);
                }
                else
                {
                    RightStack?.AddChild(item);
                }
            }
        }

        protected override void OnRemoveItem(GUIComponent component)
        {
            if (component is GUINodeItem item)
            {
                item.Node = null;
            }
        }

        protected override void OnMouseEntered()
        {
            Point mousePosition = System.Mouse.Position;
            Point delta = System.Mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                           MouseButton.None, GUINodeEditorEventType.Entered, this);
            NodeEditor.OnNodeInteractMouseEvent.Invoke(args);
        }

        protected override void OnMouseExitted()
        {
            Point mousePosition = System.Mouse.Position;
            Point delta = System.Mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                                      MouseButton.None, GUINodeEditorEventType.Exitted, this);
            NodeEditor.OnNodeInteractMouseEvent.Invoke(args);
        }

        public GUINodeItem CreateItem(string name, GUINodeItemAlignment pinAlignment = GUINodeItemAlignment.Left, GUIPinType type = GUIPinType.Input)
        {
            GUINodeItem nodeItem = GUINodeUtils.CreateNodeItem(name, pinAlignment, type);
            AddItem(nodeItem);
            return nodeItem;
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUINodeEditorEventType.Click);
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
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

            Point delta = mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mouse.Position, delta, mouse.Button, eventType, this);
            NodeEditor.OnNodeInteractMouseEvent.Invoke(args);
        }
    }
}
