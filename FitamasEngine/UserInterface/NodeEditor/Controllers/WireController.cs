using Fitamas.Events;
using Fitamas.Input;
using Fitamas.UserInterface.Scripting;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.NodeEditor.Controllers
{
    public class WireController : EditorController
    {
        private GUIWire wire;
        private GUIPin startPin;
        private bool isCreateWire;

        public WireController(GUINodeEditor editor) : base(editor)
        {
            wire = editor.CreateDefoultWire();
        }

        public override void Init()
        {
            editor.OnMouseEvent.AddListener(OnMouseEvent);
            editor.OnPinInteractMouseEvent.AddListener(OnPinEvent);
            editor.OnWireInteractMouseEvent.AddListener(OnWireEvent);
            editor.OnDeleteWire.AddListener(OnDeleteWire);
        }

        public override void Update()
        {
            wire.Enable = isCreateWire;
            if (isCreateWire)
            {
                Point mousePosition = InputSystem.mouse.MousePosition;
                Point localPosition = editor.Frame.ToLocalPosition(mousePosition);
                wire.DrawToPoint(localPosition);
            }
        }

        public override bool IsBusy()
        {
            return isCreateWire;
        }

        private void OnMouseEvent(GUINodeEditorEventArgs args)
        {
            if (args.Button == MouseButton.Right && args.EventType == GUIEventType.Click)
            {
                isCreateWire = false;
                if (startPin != null)
                {
                    startPin.IsConnected = false;
                }
            }

            if (args.Button == MouseButton.Left && args.EventType == GUIEventType.Click)
            {
                if (isCreateWire)
                {
                    wire.AnchorPoints.Add(args.MousePosition - wire.LocalPosition - editor.Frame.LocalPosition);
                }
            }
        }

        private void OnPinEvent(GUINodeEditorEventArgs args)
        {
            if (editor.GetController<NodeController>().IsBusy())
            {
                return;
            }

            if (args.Button == MouseButton.Left && args.EventType == GUIEventType.Click)
            {
                if (args.Component is GUIPin pin)
                {
                    if (isCreateWire)
                    {
                        if (TryCreateConnection(startPin, pin))
                        {
                            startPin = null;
                            isCreateWire = false;
                        }
                    }
                    else
                    {
                        isCreateWire = true;
                        startPin = pin;
                        startPin.IsConnected = true;
                        wire.AnchorPoints.Clear();
                        wire.AnchorPoints.Add(new Point());
                        wire.LocalPosition = editor.Frame.ToLocalPosition(startPin.Rectangle.Center);
                    }
                }
            }
        }

        private void OnWireEvent(GUINodeEditorEventArgs args)
        {
            if (args.EventType == GUIEventType.Entered)
            {
                if (args.Component is GUIWire wire)
                {
                    wire.ShadowEnable = true;
                }
            }
            if (args.EventType == GUIEventType.Exitted)
            {
                if (args.Component is GUIWire wire)
                {
                    wire.ShadowEnable = false;
                }
            }
            if (args.EventType == GUIEventType.Click && args.Button == MouseButton.Right)
            {
                if (args.Component != this.wire && args.Component is GUIWire wire)
                {
                    GUIContextMenu contextMenu = GUI.CreateContextMenu(args.MousePosition);
                    contextMenu.AddItem("Delete wire", () => editor.Remove(wire));
                    contextMenu.AddItem("Color Red", () => 
                    { 
                        wire.EnableColor = editor.Settings.EnableColor; 
                        wire.DisableColor = editor.Settings.DisableColor; 
                    });
                    contextMenu.AddItem("Color Green", () => 
                    { 
                        wire.EnableColor = editor.Settings.EnableColor1; 
                        wire.DisableColor = editor.Settings.DisableColor1; 
                    });
                    contextMenu.AddItem("Color Blue", () => 
                    { 
                        wire.EnableColor = editor.Settings.EnableColor2; 
                        wire.DisableColor = editor.Settings.DisableColor2; 
                    });

                    editor.OnCreateContextMenu.Invoke(args, contextMenu);
                    editor.AddChild(contextMenu);
                }
            }
        }

        private void OnDeleteWire(GUIWire wire)
        {
            if (wire.PinA == null || wire.PinB == null)
            {
                return;
            }

            wire.PinA.IsConnected = false;
            wire.PinB.IsConnected = false;
        }

        private bool TryCreateConnection(GUIPin pinA, GUIPin pinB)
        {
            if (IsValidConnection(pinA, pinB))
            {
                List<Point> points = this.wire.AnchorPoints;
                points = points[1..(points.Count - 1)];
                GUIWire wire = editor.CreateDefoultWire();

                if (pinA.PinType == GUIPinType.Output && pinB.PinType == GUIPinType.Input)
                {
                    wire.CreateConnection(pinA, pinB);
                }
                else
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        points[i] = Point.Zero -  points[i];
                    }
                    wire.CreateConnection(pinB, pinA);
                }

                wire.AnchorPoints.AddRange(points);

                editor.OnCreateConnection.Invoke(wire);

                pinA.IsConnected = true;
                pinB.IsConnected = true;

                return true;
            }

            return false;
        }

        private bool IsValidConnection(GUIPin pinA, GUIPin pinB)
        {
            if (pinA == null || pinB == null)
            {
                return false;
            }

            if (pinA.PinType == GUIPinType.Input && pinB.PinType == GUIPinType.Output)
            {
                return !IsConnected(pinA);
            }

            if (pinA.PinType == GUIPinType.Output && pinB.PinType == GUIPinType.Input)
            {
                return !IsConnected(pinB);
            }

            return false;
        }

        private bool IsConnected(GUIPin pin)
        {
            foreach (var wire in editor.Wires)
            {
                if (wire.PinA == pin || wire.PinB == pin)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
