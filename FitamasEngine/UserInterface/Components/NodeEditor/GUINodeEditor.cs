using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Math2D;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;
using Fitamas.UserInterface.Components.NodeEditor.Controllers;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public enum GUIEventType
    {
        None,
        Click,
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
        public GUIEventType EventType { get; }
        public GUIComponent Component { get; set; }

        public GUINodeEditorEventArgs(Point mousePosition, Point dragDelta, MouseButton button, GUIEventType eventType, GUIComponent component = null)
        {
            MousePosition = mousePosition;
            DragDelta = dragDelta;
            Button = button;
            EventType = eventType;
            Component = component;
        }
    }

    public class GUINodeEditor : GUIComponent, IMouseEvent, IDragMouseEvent, IKeyboardEvent
    {
        private List<GUINode> nodes = new List<GUINode>();
        private List<GUIWire> wires = new List<GUIWire>();
        private List<GUIComponent> selectComponents = new List<GUIComponent>();
        private GUIImage selectRegion;
        private GUIImage backGround;
        private EditorController[] controllers;
        private GUIFrame frame;

        public GUIEvent<GUINode> OnCreateNode = new GUIEvent<GUINode>();
        public GUIEvent<GUINode> OnDeleteNode = new GUIEvent<GUINode>();
        public GUIEvent<GUIPin> OnCreatePin = new GUIEvent<GUIPin>();
        public GUIEvent<GUIPin> OnDeletePin = new GUIEvent<GUIPin>();
        public GUIEvent<GUIWire> OnCreateWire = new GUIEvent<GUIWire>();
        public GUIEvent<GUIWire> OnDeleteWire = new GUIEvent<GUIWire>();

        public GUIEvent<GUIWire> OnCreateConnection = new GUIEvent<GUIWire>();

        public GUIEvent<KeyboardEventArgs> OnKeybordEvent = new GUIEvent<KeyboardEventArgs>();
        public GUIEvent<GUINodeEditorEventArgs> OnMouseEvent = new GUIEvent<GUINodeEditorEventArgs>();
        public GUIEvent<GUINodeEditorEventArgs> OnPinInteractMouseEvent = new GUIEvent<GUINodeEditorEventArgs>();
        public GUIEvent<GUINodeEditorEventArgs> OnNodeInteractMouseEvent = new GUIEvent<GUINodeEditorEventArgs>();
        public GUIEvent<GUINodeEditorEventArgs> OnWireInteractMouseEvent = new GUIEvent<GUINodeEditorEventArgs>();

        public GUIEvent<GUINodeEditorEventArgs, GUIContextMenu> OnCreateContextMenu = new GUIEvent<GUINodeEditorEventArgs, GUIContextMenu>();

        public NodeEditorSettings Settings { get; set; }

        public List<GUINode> Nodes => nodes;
        public List<GUIWire> Wires => wires;
        public GUIFrame Frame => frame;
        public bool OnEptyField => selectComponents.Count == 0;

        public GUINodeEditor() : base()
        {
            RaycastTarget = true;

            Settings = new NodeEditorSettings();

            frame = new GUIFrame();
            //frame.Anchor = new Vector2(0.5f, 0.5f);
            AddChild(frame);

            controllers =
            [
                new NodeController(this),
                new WireController(this),
                new AreaController(this),
            ];
        }

        protected override void OnInit()
        {
            selectRegion = new GUIImage();
            selectRegion.Color = Settings.SelectRegionColor;
            selectRegion.LocalScale = new Point(0, 0);

            AddChild(selectRegion);

            backGround = new GUIImage();
            backGround.Color = Settings.BackGroundColor;
            backGround.SetAlignment(GUIAlignment.Stretch);

            AddChild(backGround);

            foreach (var controller in controllers)
            {
                controller.Init();
            }
        }
        public void Add(GUINode node)
        {
            frame.AddChild(node);

            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                node.Init(this);
                OnCreateNode.Invoke(node);
                foreach (var pin in node.Pins)
                {
                    OnCreatePin.Invoke(pin);
                }
            }
        }

        public void Remove(GUINode node)
        {
            node.Destroy();

            nodes.Remove(node);
            OnDeleteNode.Invoke(node);

            foreach (var pin in node.Pins)
            {
                OnDeletePin.Invoke(pin);

                foreach (var wire in wires)
                {
                    if (wire.PinA == pin || wire.PinB == pin)
                    {
                        //frame.RemoveChild(wire);
                        Remove(wire);
                        break;
                    }
                }
            }
        }

        public void Add(GUIWire wire)
        {
            frame.AddChild(wire);

            if (!wires.Contains(wire))
            {
                wires.Add(wire);
                OnCreateWire.Invoke(wire);
            }
        }

        public void Remove(GUIWire wire)
        {
            wire.Destroy();

            wires.Remove(wire);
            OnDeleteWire.Invoke(wire);
        }

        //protected override void OnAddChild(GUIComponent component)
        //{
        //    if (component is GUINode node)
        //    {
        //        Add(node);
        //    }
        //    if (component is GUIWire wire)
        //    {
        //        Add(wire);
        //    }
        //}

        //protected override void OnRemoveChild(GUIComponent component)
        //{
        //    if (component is GUINode node)
        //    {
        //        Remove(node);
        //    }
        //    if (component is GUIWire wire)
        //    {
        //        Remove(wire);
        //    }
        //}

        public GUINode CreateDefoultNode(string name = "Node")
        {
            GUINode node = GUI.CreateNode(new Rectangle(new Point(), Settings.NodeSize), name);

            node.HeaderTextBlock.Color = Settings.HeaderTextBlockColor;
            node.HeaderImage.Color = Settings.HeaderImageColor;
            node.Image.Color = Settings.ImageColor;
            node.SelectedImage.Color = Settings.SelectedImageColor;
            node.SelectedImage.LocalPosition -= Settings.SelectBorderScale;
            node.SelectedImage.LocalScale -= Settings.SelectBorderScale;

            Add(node);
            return node;
        }

        public GUIWire CreateDefoultWire()
        {
            GUIWire wire = GUI.CreateWire();
            wire.EnableColor = Settings.EnableColor;
            wire.DisableColor = Settings.DisableColor;
            wire.ShadowColor = Settings.SelectWireColor;
            wire.Thickness = Settings.WireThickness;
            wire.ShadowSize = Settings.SelectWireSize;

            Add(wire);
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

        protected override void OnUpdate(GameTime gameTime)
        {
            foreach (var controller in controllers)
            {
                controller.Update();
            }

            Point mousePosition = InputSystem.mouse.MousePosition;
            Point delta = InputSystem.mouseDelta;
            Point localPosition = ScreenToLocal(mousePosition);
            List<GUIComponent> selects = [.. selectComponents];
            selectComponents.Clear();

            foreach (var node in nodes)
            {
                bool containNode = selects.Contains(node);

                if (node.Contain(mousePosition))
                {
                    selectComponents.Add(node);
                    if (!containNode)
                    {
                        GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                  MouseButton.None, GUIEventType.Entered, node);
                        OnNodeInteractMouseEvent.Invoke(args);
                    }

                    foreach (var pin in node.Pins)
                    {
                        bool containPin = selects.Contains(pin);

                        if (pin.Contain(mousePosition))
                        {
                            selectComponents.Add(pin);
                            if (!containPin)
                            {
                                GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                           MouseButton.None, GUIEventType.Entered, pin);
                                OnNodeInteractMouseEvent.Invoke(args);
                            }
                        }
                        else
                        {
                            if (containPin)
                            {
                                GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                           MouseButton.None, GUIEventType.Exitted, pin);
                                OnNodeInteractMouseEvent.Invoke(args);
                            }
                        }
                    }
                }
                else
                {
                    if (containNode)
                    {
                        GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                  MouseButton.None, GUIEventType.Exitted, node);
                        OnNodeInteractMouseEvent.Invoke(args);
                    }
                }
            }

            foreach (var wire in wires)
            {
                bool containWire = selects.Contains(wire);

                if (wire.Contain(mousePosition))
                {
                    selectComponents.Add(wire);
                    if (!containWire)
                    {
                        GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                   MouseButton.None, GUIEventType.Entered, wire);
                        OnWireInteractMouseEvent.Invoke(args);
                    }
                }
                else
                {
                    if (containWire)
                    {
                        GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta,
                                                  MouseButton.None, GUIEventType.Exitted, wire);
                        OnWireInteractMouseEvent.Invoke(args);
                    }
                }
            }
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUIEventType.Click);
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }

        public void OnStartDragMouse(MouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUIEventType.StartDrag);
        }

        public void OnDragMouse(MouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUIEventType.Drag);
        }

        public void OnEndDragMouse(MouseEventArgs mouse)
        {
            InvokeMouseEvent(mouse, GUIEventType.EndDrag);
        }

        public void OnKeyDown(KeyboardEventArgs args)
        {
            OnKeybordEvent.Invoke(args);
        }

        public void OnKeyUP(KeyboardEventArgs args)
        {

        }

        public void OnKey(KeyboardEventArgs args)
        {

        }

        private void InvokeMouseEvent(MouseEventArgs mouse, GUIEventType eventType)
        {
            if (!IsMouseOver && (eventType == GUIEventType.Click || eventType == GUIEventType.StartDrag))
            {
                return;
            }

            System.Focused = this;

            Point localPosition = ScreenToLocal(mouse.Position);
            Point delta = mouse.DistanceMoved.ToPoint();
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(localPosition, delta, mouse.Button, eventType);
            OnMouseEvent.Invoke(args);

            foreach (var component in selectComponents)
            {

                if (component.Contain(mouse.Position))
                {
                    args = new GUINodeEditorEventArgs(localPosition, delta, mouse.Button, eventType);
                    args.Component = component;

                    if (component is GUINode)
                    {
                        OnNodeInteractMouseEvent.Invoke(args);
                    }
                    else if (component is GUIPin)
                    {
                        OnPinInteractMouseEvent.Invoke(args);
                    }
                    else if (component is GUIWire)
                    {
                        OnWireInteractMouseEvent.Invoke(args);
                    }
                }
            }
        }

        public void StartSelectRegion(Point mousePosition)
        {
            selectRegion.LocalPosition = mousePosition;
        }

        public void SelectRegion(Point mousePosition)
        {
            Point scale = selectRegion.LocalPosition - mousePosition;

            selectRegion.LocalScale = new Point(Math.Abs(scale.X), Math.Abs(scale.Y));

            selectRegion.Pivot = new Vector2(scale.X < 0 ? 0 : 1, scale.Y < 0 ? 1 : 0);
        }

        public Rectangle EndSelectRegion()
        {
            Rectangle region = selectRegion.Rectangle;
            selectRegion.LocalScale = new Point();

            return region;
        }
    }

    public class NodeEditorSettings
    {
        public Color BackGroundColor = new Color(0.5f, 0.5f, 0.5f);
        public Color SelectRegionColor = new Color(0.5f, 0.8f, 1, 0.5f);

        public Point NodeSize = new Point(100, 100);
        public float HeaderSize = 1.1f;
        public Color HeaderTextBlockColor = Color.White;
        public Color HeaderImageColor = new Color(0, 0.36f, 0.48f);
        public Color ImageColor = new Color(0, 0.6f, 0.8f);
        public Color SelectedImageColor = new Color(0.8f, 0.8f, 0.8f, 0.7f);
        public Point SelectBorderScale = new Point(10, 10);

        public Point PinSize = new Point(25, 25);
        public Color PinTextColor = Color.White;
        public int PinSpacing = 5;
        public int SiteSpacing = 20;
        public Sprite PinOn;
        public Sprite PinOff;

        //public Color WireColor = Color.White;
        public Color EnableColor = new Color(1f, 0, 0);
        public Color DisableColor = new Color(0.4f, 0, 0);

        public Color EnableColor1 = new Color(0, 1f, 0);
        public Color DisableColor1 = new Color(0, 0.4f, 0);

        public Color EnableColor2 = new Color(0, 0, 1f);
        public Color DisableColor2 = new Color(0, 0, 0.4f);

        public Color SelectWireColor = new Color(0.8f, 0.8f, 0.8f, 0.7f);
        public int WireThickness = 8;
        public int SelectWireSize = 10;
    }
}