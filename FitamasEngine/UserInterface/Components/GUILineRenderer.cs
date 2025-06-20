using Fitamas.Math;
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
        public static readonly DependencyProperty<Color> ColorProperty = new DependencyProperty<Color>(Color.White);

        public static readonly DependencyProperty<Color> ShadowColorProperty = new DependencyProperty<Color>(Color.Black);

        public static readonly DependencyProperty<int> ThicknessProperty = new DependencyProperty<int>(1);

        public static readonly DependencyProperty<int> ShadowSizeProperty = new DependencyProperty<int>(1);

        public static readonly DependencyProperty<bool> ShadowEnableProperty = new DependencyProperty<bool>(false);

        protected Texture2D texture;

        public List<Point> Anchors { get; }

        public Color Color
        {
            get
            {
                return GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public Color ShadowColor
        {
            get
            {
                return GetValue(ShadowColorProperty);
            }
            set
            {
                SetValue(ShadowColorProperty, value);
            }
        }

        public int Thickness
        {
            get
            {
                return GetValue(ThicknessProperty);
            }
            set
            {
                SetValue(ThicknessProperty, value);
            }
        }

        public int ShadowSize
        {
            get
            {
                return GetValue(ShadowSizeProperty);
            }
            set
            {
                SetValue(ShadowSizeProperty, value);
            }
        }

        public bool ShadowEnable
        {
            get
            {
                return GetValue(ShadowEnableProperty);
            }
            set
            {
                SetValue(ShadowEnableProperty, value);
            }
        }

        public GUILineRenderer()
        {
            texture = Texture2DHelper.DefaultTexture;
            Anchors = new List<Point>();
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            if (Anchors == null || Anchors.Count == 0)
            {
                return;
            }

            Rectangle rectangle = Rectangle;
            Point position = rectangle.Location;

            Render.Begin(context.Mask);

            if (ShadowEnable)
            {
                for (int i = 1; i < Anchors.Count; i++)
                {
                    DrawLine(Anchors[i - 1], Anchors[i], position, ShadowColor, Thickness + ShadowSize);
                }
            }

            for (int i = 1; i < Anchors.Count; i++)
            {
                DrawLine(Anchors[i - 1], Anchors[i], position, Color, Thickness);
            }

            Render.End();

            base.OnDraw(gameTime, context);
        }

        private void DrawLine(Point lastAnchor, Point currentAnchor, Point position, Color color, int thickness)
        {
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
