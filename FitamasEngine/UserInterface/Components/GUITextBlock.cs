using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public enum GUITextAligment
    {
        Left,
        Middle,
        Right,
    }

    public class GUITextBlock : GUIComponent
    {
        public static readonly DependencyProperty<string> TextProperty = new DependencyProperty<string>();

        public static readonly DependencyProperty<bool> AutoScaleProperty = new DependencyProperty<bool>(true, false);

        public static readonly DependencyProperty<float> ScaleProperty = new DependencyProperty<float>(1, false);

        public static readonly DependencyProperty<Color> ColorProperty = new DependencyProperty<Color>(Color.Black);

        public static readonly DependencyProperty<GUITextAligment> TextAligmentProperty = new DependencyProperty<GUITextAligment>(GUITextAligment.Left);

        public static readonly DependencyProperty<SpriteFont> FontProperty = new DependencyProperty<SpriteFont>(FontChangedCallback, FontExpressionChangedCallback);

        public string Text
        {
            get
            {
                return GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public bool AutoScale
        {
            get
            {
                return GetValue(AutoScaleProperty);
            }
            set
            {
                SetValue(AutoScaleProperty, value);
            }
        }

        public float Scale
        {
            get
            {
                return GetValue(ScaleProperty);
            }
            set
            {
                SetValue(ScaleProperty, value);
            }
        }

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

        public GUITextAligment TextAligment
        {
            get
            {
                return GetValue(TextAligmentProperty);
            }
            set
            {
                SetValue(TextAligmentProperty, value);
            }
        }

        public SpriteFont Font
        {
            get
            {
                return GetValue(FontProperty);
            }
            set
            {
                SetValue(FontProperty, value);
            }
        }

        public GUITextBlock()
        {
            Scale = 1f;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (AutoScale)
            {
                SpriteFont font = Font;
                string text = Text;
                if (font == null || string.IsNullOrEmpty(text))
                {
                    LocalSize = Point.Zero;
                    return;
                }

                Point localScale = LocalSize;
                Point scale = (font.MeasureString(text) * Scale).ToPoint();

                if (HorizontalAlignment != GUIHorizontalAlignment.Stretch)
                {
                    localScale.X = scale.X;
                }

                if (VerticalAlignment != GUIVerticalAlignment.Stretch)
                {
                    localScale.Y = scale.Y;
                }

                LocalSize = localScale;
            }
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            SpriteFont font = Font;
            if (font == null)
            {
                return;
            }

            string text = Text;
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Point position = Rectangle.Location;
            Point textScale = font.MeasureString(text).ToPoint();

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

            Render.Begin(context.Mask);
            Render.DrawString(font, text, position, Color, Scale);
            Render.End();

            base.OnDraw(gameTime, context);
        }

        public int GetCaretIndexFromScreenPos(Point position)
        {
            if (Contains(position))
            {
                return GetCaretIndexFromLocalPos(ToLocal(position));
            }

            return Text.Length;
        }

        public int GetCaretIndexFromLocalPos(Point position) 
        {
            SpriteFont font = Font;
            if (font == null)
            {
                return 0;
            }

            string text = Text;
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            Dictionary<char, SpriteFont.Glyph> glyphs = font.GetGlyphs();
            float xPos = 0;
            float yPos = font.MeasureString(text).Y;
            float closestXDist = float.PositiveInfinity;
            float closestYDist = float.PositiveInfinity;
            int closestIndex = -1;

            for (int i = 0; i < text.Length; i++)
            {
                float xDist = Math.Abs(position.X - xPos);
                float yDist = Math.Abs(position.Y - yPos);
                if (yDist < closestYDist || (NearlyEqual(yDist, closestYDist) && xDist < closestXDist))
                {
                    closestIndex = i;
                    closestXDist = xDist;
                    closestYDist = yDist;
                }

                if (glyphs.TryGetValue(text[i], out SpriteFont.Glyph glyph))
                {
                    xPos += glyph.WidthIncludingBearings + font.Spacing;
                }
            }

            if (Math.Abs(position.X - xPos) < closestXDist)
            {
                closestIndex = text.Length;
            }

            return closestIndex;
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

        private static void FontChangedCallback(DependencyObject dependencyObject, DependencyProperty<SpriteFont> property, SpriteFont oldValue, SpriteFont newValue)
        {

        }

        private static void FontExpressionChangedCallback(DependencyObject dependencyObject, DependencyProperty property, Expression oldExpression, Expression newExpression)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {

            }
        }
    }
}
