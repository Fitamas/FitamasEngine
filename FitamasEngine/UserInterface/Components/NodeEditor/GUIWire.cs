using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUIWire : GUILineRenderer
    {
        public GUIPin PinA { get; private set; }
        public GUIPin PinB { get; private set; }

        public GUIWire()
        {

        }

        public void CreateConnection(GUIPin pinA, GUIPin pinB)
        {
            if (pinA == null || pinB == null)
            {
                throw new Exception("Cannot connect pins: pin is null.");
            }

            LocalPosition = Parent.ToLocal(pinA.Rectangle.Center);

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
            //List<Point> points = [.. AnchorPoints];

            //if (PinA != null && PinB != null)
            //{
            //    //LocalPosition = Parent.ToLocalPosition(PinA.Rectangle.Center);
            //    points.Insert(0, Parent.ToLocal(PinA.Rectangle.Center) - LocalPosition);
            //    points.Add(Parent.ToLocal(PinB.Rectangle.Center) - LocalPosition);
            //}
            //else
            //{
            //    points.Add(drawToPoint - LocalPosition);
            //}
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
    }
}
