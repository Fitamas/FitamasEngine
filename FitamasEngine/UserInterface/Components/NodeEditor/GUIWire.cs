using Fitamas.Input.InputListeners;
using Fitamas.Math2D;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using SharpFont;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUIWire : GUILineRenderer, IMouseEvent, IDragMouseEvent
    {
        public GUINodeEditor NodeEditor { get; set; }
        public GUIPin PinA { get; private set; }
        public GUIPin PinB { get; private set; }

        public GUIWire()
        {
            RaycastTarget = true;
        }

        protected override void OnMouseEntered()
        {
            Point mousePosition = System.Mouse.Position;
            Point delta = System.Mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                           MouseButton.None, GUINodeEditorEventType.Entered, this);
            NodeEditor.OnWireInteractMouseEvent.Invoke(args);
        }

        protected override void OnMouseExitted()
        {
            Point mousePosition = System.Mouse.Position;
            Point delta = System.Mouse.Delta;
            GUINodeEditorEventArgs args = new GUINodeEditorEventArgs(mousePosition, delta,
                                      MouseButton.None, GUINodeEditorEventType.Exitted, this);
            NodeEditor.OnWireInteractMouseEvent.Invoke(args);
        }

        public override bool Contains(Point point)
        {
            for (int i = 1; i < Anchors.Count; i++)
            {
                Point a = FromLocal(Anchors[i - 1]);
                Point b = FromLocal(Anchors[i]);
                float distance = MathV.DistancePointToSegment(a.ToVector2(), b.ToVector2(), point.ToVector2());

                if (distance <= (Thickness + ShadowSize) / 2f)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            if (Anchors.Count >= 2 && PinA != null && PinB != null)
            {
                Anchors[0] = PinA.Rectangle.Center - Rectangle.Location;
                Anchors[Anchors.Count - 1] = PinB.Rectangle.Center - Rectangle.Location;
            }

            base.OnDraw(gameTime, context);
        }

        protected override void OnDestroy()
        {
            if (PinA != null)
            {
                PinA.IsConnected = false;
            }
            if (PinB != null)
            {
                PinB.IsConnected = false;
            }
        }

        public void CreateConnection(GUIPin pinA, GUIPin pinB)
        {
            if (pinA == null || pinB == null)
            {
                throw new Exception("Cannot connect pins: pin is null.");
            }

            pinA.IsConnected = true;
            pinB.IsConnected = true;

            PinA = pinA;
            PinB = pinB;
        }

        public bool HasPin(GUIPin pinA, GUIPin pinB)
        {
            return HasPin(pinA) && HasPin(pinB);
        }

        public bool HasPin(GUIPin pin)
        {
            return PinA == pin || PinB == pin;
        }

        public void DrawToPoint(Point targetPoint)
        {
            if (Anchors.Count > 0)
            {
                Anchors[Anchors.Count - 1] = targetPoint;
            }
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
            NodeEditor.OnWireInteractMouseEvent.Invoke(args);
        }
    }
}
