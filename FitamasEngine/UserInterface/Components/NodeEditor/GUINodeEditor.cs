using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Fitamas.UserInterface.Components.NodeEditor.Controllers;
using Fitamas.UserInterface.Input;
using Fitamas.Input.InputListeners;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public enum GUINodeEditorEventType
    {
        None,
        Click,
        Moved,
        StartDrag,
        Drag,
        EndDrag,
        Entered,
        Exitted
    }

    public class GUINodeEditorEventArgs
    {
        public Point MousePosition { get; }
        public Point DragDelta { get; }
        public MouseButton Button { get; }
        public GUINodeEditorEventType EventType { get; }
        public GUIComponent Target { get; }

        public GUINodeEditorEventArgs(Point mousePosition, Point dragDelta, MouseButton button, GUINodeEditorEventType eventType, GUIComponent component)
        {
            MousePosition = mousePosition;
            DragDelta = dragDelta;
            Button = button;
            EventType = eventType;
            Target = component;
        }
    }

    public class GUICreateConnectionEventArgs
    {
        public GUIPin PinA { get; }
        public GUIPin PinB { get; }
        public List<Point> Points { get; }

        public GUICreateConnectionEventArgs(GUIPin pinA, GUIPin pinB, List<Point> points)
        {
            PinA = pinA;
            PinB = pinB;
            Points = points;
        }
    }

    public enum GUINodeEventType
    {
        Create,
        Moved,
        Destroy
    }

    public class GUINodeEventArgs
    {
        public GUINode Node { get; }
        public GUINodeEventType EventType { get; }

        public GUINodeEventArgs(GUINode node, GUINodeEventType eventType)
        {
            Node = node;
            EventType = eventType;
        }
    }

    public class GUINodeEditor : GUIItemsControl, IMouseEvent, IDragMouseEvent, IKeyboardEvent
    {
        private List<GUINode> nodes = new List<GUINode>();
        private List<GUIWire> wires = new List<GUIWire>();
        private EditorController[] controllers;

        public GUIComponent Content { get; }
        public GUIComponent SelectRegion { get; set; }

        public MonoEvent<GUINodeEventArgs> OnNodeEvent = new MonoEvent<GUINodeEventArgs>();

        public MonoEvent<GUIWire> OnCreateWire = new MonoEvent<GUIWire>();
        public MonoEvent<GUIWire> OnDestroyWire = new MonoEvent<GUIWire>();

        public MonoEvent<GUICreateConnectionEventArgs> OnCreateConnection = new MonoEvent<GUICreateConnectionEventArgs>();

        public MonoEvent<GUIKeyboardEventArgs> OnKeybordEvent = new MonoEvent<GUIKeyboardEventArgs>();
        public MonoEvent<GUINodeEditorEventArgs> OnMouseEvent = new MonoEvent<GUINodeEditorEventArgs>();
        public MonoEvent<GUINodeEditorEventArgs> OnPinInteractMouseEvent = new MonoEvent<GUINodeEditorEventArgs>();
        public MonoEvent<GUINodeEditorEventArgs> OnNodeInteractMouseEvent = new MonoEvent<GUINodeEditorEventArgs>();
        public MonoEvent<GUINodeEditorEventArgs> OnWireInteractMouseEvent = new MonoEvent<GUINodeEditorEventArgs>();

        public List<GUINode> Nodes => nodes;
        public List<GUIWire> Wires => wires;

        public GUINodeEditor()
        {
            IsFocusScope = true;
            RaycastTarget = true;
            IsMask = true;

            GUIFrame frame = new GUIFrame();
            AddChild(frame);
            Content = frame;

            controllers =
            [
                new NodeController(this),
                new WireController(this),
                new AreaController(this),
            ];
        }

        protected override void OnAddItem(GUIComponent component)
        {
            if (component is GUINode node)
            {
                Content.AddChild(node);
                nodes.Add(node);
                node.NodeEditor = this;
                OnNodeEvent.Invoke(new GUINodeEventArgs(node, GUINodeEventType.Create));
            }
            if (component is GUIWire wire)
            {
                Content.AddChild(wire);
                wires.Add(wire);
                wire.NodeEditor = this;
                OnCreateWire.Invoke(wire);
            }
        }

        protected override void OnRemoveItem(GUIComponent component)
        {
            if (component is GUINode node)
            {
                node.Destroy();
                nodes.Remove(node);

                foreach (var wire0 in wires.ToArray())
                {
                    foreach (var item in node.Items)
                    {
                        if (item is GUINodeItem nodeItem)
                        {
                            if (wire0.PinA == nodeItem.Pin || wire0.PinB == nodeItem.Pin)
                            {
                                RemoveItem(wire0);
                            }
                        }
                    }
                }

                OnNodeEvent.Invoke(new GUINodeEventArgs(node, GUINodeEventType.Destroy));
            }
            if (component is GUIWire wire)
            {
                wire.Destroy();
                wires.Remove(wire);
                OnDestroyWire.Invoke(wire);
            }
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            foreach (var component in wires)
            {
                component.Draw(gameTime, context);
            }

            foreach (var component in nodes)
            {
                component.Draw(gameTime, context);
            }

            SelectRegion.Draw(gameTime, context);
        }

        public GUINode CreateNode(string name)
        {
            GUINode node = GUINodeUtils.CreateNode(new Point(), name);
            AddItem(node);
            return node;
        }

        public GUIWire CreateWire(GUIPin pinA, GUIPin pinB)
        {
            GUIWire wire = GUINodeUtils.CreateWire();
            wire.CreateConnection(pinA, pinB);
            AddItem(wire);
            return wire;
        }

        public T GetController<T>() where T : EditorController
        {
            foreach (var controller in controllers)
            {
                if (controller.GetType() == typeof(T))
                {
                    return (T)controller;
                }
            }

            return null;
        }

        public bool IsAnyControllerBusy()
        {
            foreach (var controller in controllers)
            {
                if (controller.IsBusy())
                {
                    return true;
                }
            }
            return false;
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {
            if (!IsFocused)
            {
                return;
            }

            GUINodeEditorEventArgs args0 = new GUINodeEditorEventArgs(mouse.Position, mouse.Delta, MouseButton.None, GUINodeEditorEventType.Moved, this);
            OnMouseEvent.Invoke(args0);
        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            if (!IsFocused)
            {
                Focus();
            }

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

        public void OnKeyDown(GUIKeyboardEventArgs args)
        {
            OnKeybordEvent.Invoke(args);
        }

        public void OnKeyUP(GUIKeyboardEventArgs args)
        {

        }

        public void OnKey(GUIKeyboardEventArgs args)
        {

        }

        private void InvokeMouseEvent(GUIMouseEventArgs mouse, GUINodeEditorEventType eventType)
        {
            if (!IsFocused)
            {
                return;
            }

            Point delta = mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mouse.Position, delta, mouse.Button, eventType, this);
            OnMouseEvent.Invoke(args);
        }

        public void StartSelectRegion(Point mousePosition)
        {
            SelectRegion.LocalPosition = mousePosition;
        }

        public void DoSelectRegion(Point mousePosition)
        {
            Point scale = SelectRegion.LocalPosition - mousePosition;

            SelectRegion.LocalSize = new Point(global::System.Math.Abs(scale.X), global::System.Math.Abs(scale.Y));

            SelectRegion.Pivot = new Vector2(scale.X < 0 ? 0 : 1, scale.Y < 0 ? 0 : 1);
        }

        public Rectangle EndSelectRegion()
        {
            Rectangle region = SelectRegion.Rectangle;
            SelectRegion.LocalSize = new Point();

            return region;
        }
    }
}