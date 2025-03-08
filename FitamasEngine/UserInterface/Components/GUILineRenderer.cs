using Fitamas.Math2D;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public class GUILineRenderer : GUIComponent
    {
        [SerializableField] protected List<Point> anchors = new List<Point>();
        [SerializableField] protected Texture2D texture;

        public Color Color = Color.White;
        public Color ShadowColor = Color.Black;
        public int Thickness = 10;
        public int ShadowSize = 10;
        public bool ShadowEnable = false;

        public GUILineRenderer()
        {
            texture = Texture2DHelper.DefaultTexture;
        }

        public void SetAnchorPoints(IEnumerable<Point> anchors)
        {
            this.anchors = anchors.ToList();

            if (this.anchors.Count >= 2)
            {
                Point min = this.anchors[0];
                Point max = this.anchors[0];

                foreach (Point anchor in anchors)
                {
                    if (anchor.X < min.X) min.X = anchor.X;
                    if (anchor.Y < min.Y) min.Y = anchor.Y;
                    if (anchor.X > max.X) max.X = anchor.X;
                    if (anchor.Y > max.Y) max.Y = anchor.Y;
                }

                //LocalScale = 
            }
            //LocalScale = new Point();
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            Rectangle rectangle = Rectangle;
            Point position = rectangle.Location;

            Render.Begin(context.Mask);

            if (ShadowEnable)
            {
                for (int i = 1; i < anchors.Count; i++)
                {
                    DrawLine(anchors[i - 1], anchors[i], position, ShadowColor, Thickness + ShadowSize);
                }
            }

            for (int i = 1; i < anchors.Count; i++)
            {
                DrawLine(anchors[i - 1], anchors[i], position, Color, Thickness);
            }

            Render.End();

            base.OnDraw(gameTime, context);
        }

        private void DrawLine(Point lastAnchor, Point currentAnchor, Point position, Color color, int thickness)
        {
            lastAnchor.Y = -lastAnchor.Y;
            currentAnchor.Y = -currentAnchor.Y;
            Vector2 v1 = lastAnchor.ToVector2();
            Vector2 v2 = currentAnchor.ToVector2();
            Vector2 direction = v2 - v1;
            float angle = MathV.Angle(direction) - MathF.PI / 2;
            float distance = Vector2.Distance(v1, v2);
            float halfThickness = thickness / 2;
            Point offset = (direction.PerpendicularCounterClockwise().NormalizeF() * halfThickness).ToPoint();
            Rectangle rectangle = new Rectangle(position + lastAnchor - offset, new Point((int)distance, thickness));

            Render.Draw(texture, rectangle, color, angle);
        }
    }
}
