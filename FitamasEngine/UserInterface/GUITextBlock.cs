using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;

namespace Fitamas.UserInterface
{
    public enum GUITextAligment
    {
        Left,
        Middle,
        Right,
    }

    public class GUITextBlock : GUIComponent
    {
        [SerializableField] private string text;
        [SerializableField] private bool autoScale;

        public BitmapFont Font;
        public GUITextAligment TextAligment;
        public Color Color;
        public float Scale;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    CalculateScale();
                }
            }
        }

        public bool AutoScale
        {
            get
            {
                return autoScale;
            }
            set
            {
                if (autoScale != value)
                {
                    autoScale = value;
                    CalculateScale();
                }
            }
        }

        public GUITextBlock(BitmapFont font, string text) : base()
        {
            Font = font;
            Color = Color.Black;
            TextAligment = GUITextAligment.Left;
            AutoScale = false;
            Scale = 1f;

            if (!string.IsNullOrEmpty(text))
            {
                Text = text;
            }
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            //        else
            //        {
            //            if (rectangle.Left > ClippingRectangle.Left)
            //            {
            //                int offset = rectangle.Left - ClippingRectangle.Left;
            //                LocalPosition -= new Point(offset, 0);
            //            }
            //            else if (rectangle.Right < ClippingRectangle.Right)
            //            {
            //                int offset = ClippingRectangle.Right - rectangle.Right;
            //                LocalPosition += new Point(offset, 0);
            //            }
            //        }


            Vector2 position = Rectangle.Location.ToVector2();
            Vector2 textScale = Font.MeasureString(Text);

            switch (TextAligment)
            {
                case GUITextAligment.Left:
                    break;
                case GUITextAligment.Middle:
                    position.X += Rectangle.Width / 2 - textScale.X / 2;
                    break;
                case GUITextAligment.Right:
                    position.X += Rectangle.Width - textScale.X;
                    break;
            }

            GUIRender.DrawString(Font, Text, position, Color, Vector2.Zero, Scale, Layer);
        }

        private void CalculateScale()
        {
            if (AutoScale)
            {
                Point scale = Font.MeasureString(text).ToPoint();

                if (Stretch == GUIStretch.None)
                {
                    LocalScale = scale;
                }
                else if (Stretch == GUIStretch.Horizontal)
                {
                    LocalScale = new Point(0, scale.Y);
                }
                else if (Stretch == GUIStretch.Vertical)
                {
                    LocalScale = new Point(scale.Y, 0);
                }
            }
        }

        public int GetCaretIndexFromScreenPos(Point position) // TDOD fix caret position
        {
            Rectangle rectangle = Rectangle;

            if (rectangle.Contains(position))
            {
                return GetCaretIndexFromLocalPos(position - rectangle.Location);
            }

            return Text.Length;
        }

        public int GetCaretIndexFromLocalPos(Point position) 
        {
            Vector2[] positions = GetAllCaretPositions();
            if (positions == null || positions.Length == 0) { return 0; }

            float closestXDist = float.PositiveInfinity;
            float closestYDist = float.PositiveInfinity;
            int closestIndex = -1;
            for (int i = 0; i < positions.Length; i++)
            {
                float xDist = Math.Abs(position.X - positions[i].X);
                float yDist = Math.Abs(position.Y - (positions[i].Y + Font.LineHeight * 0.5f));
                if (yDist < closestYDist || (NearlyEqual(yDist, closestYDist) && xDist < closestXDist))
                {
                    closestIndex = i;
                    closestXDist = xDist;
                    closestYDist = yDist;
                }
            }

            return closestIndex >= 0 ? closestIndex : Text.Length;
        }

        public Vector2[] GetAllCaretPositions()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return null;
            }

            Vector2[] vectors = new Vector2[Text.Length];

            float xPos = 0;

            for (int i = 0; i < vectors.Length; i++)
            {
                if (i > 0)
                {
                    char c = Text[i - 1];
                    var region = Font.GetCharacterRegion(c);
                    xPos += region != null ? region.Width : 0;
                }

                vectors[i] = new Vector2(xPos, Font.LineHeight);
            }

            return vectors;
        }

        public bool NearlyEqual(double? value1, double? value2, double unimportantDifference = 0.0001)
        {
            if (value1 != value2)
            {
                if (value1 == null || value2 == null)
                    return false;

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }
    }
}
