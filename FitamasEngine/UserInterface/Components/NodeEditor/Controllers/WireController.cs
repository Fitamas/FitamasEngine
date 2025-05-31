using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    internal class WireController : EditorController
    {
        private GUIWire wire;
        private GUIPin startPin;
        private bool startPinIsConnected;
        private bool isCreateWire;

        public WireController(GUINodeEditor editor) : base(editor)
        {
            wire = GUINodeUtils.CreateWire();
            wire.RaycastTarget = false;
            editor.AddItem(wire);

            editor.OnMouseEvent.AddListener(OnMouseEvent);
            editor.OnPinInteractMouseEvent.AddListener(OnPinEvent);
            editor.OnWireInteractMouseEvent.AddListener(OnWireEvent);
        }

        public override bool IsBusy()
        {
            return isCreateWire;
        }

        private void OnMouseEvent(GUINodeEditorEventArgs args)
        {
            Point localPosition = editor.ToLocal(args.MousePosition);

            if (args.Button == MouseButton.Right && args.EventType == GUINodeEditorEventType.Click)
            {
                isCreateWire = false;
                if (startPin != null)
                {
                    startPin.IsConnected = startPinIsConnected;
                }
            }

            if (editor.IsMouseOver && args.Button == MouseButton.Left && args.EventType == GUINodeEditorEventType.Click)
            {
                if (isCreateWire)
                {
                    Point point = localPosition - wire.LocalPosition - editor.Content.LocalPosition;
                    wire.Anchors[wire.Anchors.Count - 1] = point;
                    wire.Anchors.Add(point);
                }
            }

            wire.Enable = isCreateWire;
            if (isCreateWire)
            {
                Point point = localPosition - wire.LocalPosition - editor.Content.LocalPosition;
                wire.DrawToPoint(point);
            }
        }

        private void OnPinEvent(GUINodeEditorEventArgs args)
        {
            if (editor.GetController<NodeController>().IsBusy())
            {
                return;
            }

            Point localPosition = editor.ToLocal(args.MousePosition);

            if (args.Button == MouseButton.Left && args.EventType == GUINodeEditorEventType.Click)
            {
                if (args.Target is GUIPin pin)
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
                        startPinIsConnected = pin.IsConnected;
                        startPin = pin;
                        startPin.IsConnected = true;
                        wire.Anchors.Clear();

                        Point point = localPosition - wire.LocalPosition - editor.Content.LocalPosition;
                        wire.Anchors.Add(point);
                        wire.Anchors.Add(point);
                    }
                }
            }

            wire.Enable = isCreateWire;
        }

        private void OnWireEvent(GUINodeEditorEventArgs args)
        {
            if (args.EventType == GUINodeEditorEventType.Entered)
            {
                if (args.Target is GUIWire wire)
                {
                    wire.ShadowEnable = true;
                }
            }

            if (args.EventType == GUINodeEditorEventType.Exitted)
            {
                if (args.Target is GUIWire wire)
                {
                    wire.ShadowEnable = false;
                }
            }
        }

        private bool TryCreateConnection(GUIPin pinA, GUIPin pinB)
        {
            if (IsValidConnection(pinA, pinB))
            {
                pinA.IsConnected = false;
                pinB.IsConnected = false;
                editor.OnCreateConnection.Invoke(new GUICreateConnectionEventArgs(pinA, pinB, wire.Anchors));

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
