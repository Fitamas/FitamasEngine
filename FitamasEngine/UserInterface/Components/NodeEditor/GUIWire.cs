using Fitamas.Math2D;
using Fitamas.Serialization;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUIWire : GUILineRenderer
    {
        [SerializableField] private List<Point> anchorPoints = new List<Point>();
        [SerializableField] private Color enableColor = Color.White;
        [SerializableField] private Color disableColor = Color.White;

        private Point drawToPoint;
        private bool isPowered;

        public GUIPin PinA { get; private set; }
        public GUIPin PinB { get; private set; }

        public List<Point> AnchorPoints => anchorPoints;

        public Color EnableColor
        {
            get
            {
                return enableColor;
            }
            set
            {
                enableColor = value;
                Color = isPowered ? EnableColor : DisableColor;
            }
        }
        public Color DisableColor
        {
            get
            {
                return disableColor;
            }
            set
            {
                disableColor = value;
                Color = isPowered ? EnableColor : DisableColor;
            }
        }

        public bool IsPowered
        {
            get
            {
                return isPowered;
            }
            set
            {
                isPowered = value;
                Color = isPowered ? EnableColor : DisableColor;
            }
        }

        public GUIWire()
        {
            IsPowered = false;
        }

        protected void OnUpdate(GameTime gameTime)
        {
            List<Point> points = [.. AnchorPoints];

            if (PinA != null && PinB != null)
            {
                //LocalPosition = Parent.ToLocalPosition(PinA.Rectangle.Center);
                points.Insert(0, Parent.ToLocal(PinA.Rectangle.Center) - LocalPosition);
                points.Add(Parent.ToLocal(PinB.Rectangle.Center) - LocalPosition);
            }
            else
            {
                points.Add(drawToPoint - LocalPosition);
            }

            SetAnchorPoints(points);
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

        public void SetAnchors(IEnumerable<Point> points)
        {
            if (points != null)
            {
                anchorPoints = points.ToList();
            }
        }

        public void DrawToPoint(Point targetPoint)
        {
            drawToPoint = targetPoint;
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
