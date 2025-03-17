using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public enum GUITextHorisontalAlignment
    {
        Left,
        Middle,
        Right,
    }

    public enum GUITextVerticalAlignment
    {
        Top,
        Middle,
        Bottom,
    }

    public class GUITextBlock : GUIComponent
    {
        public static readonly DependencyProperty<string> TextProperty = new DependencyProperty<string>(TextChangedCallback);

        public static readonly DependencyProperty<bool> AutoScaleProperty = new DependencyProperty<bool>(AutoScaleChangedCallback, true, false);

        public static readonly DependencyProperty<float> ScaleProperty = new DependencyProperty<float>(ScaleChangedCallback, 1, false);

        public static readonly DependencyProperty<Color> ColorProperty = new DependencyProperty<Color>(Color.Black);

        public static readonly DependencyProperty<GUITextHorisontalAlignment> TextHorisontalAlignmentProperty = new DependencyProperty<GUITextHorisontalAlignment>(GUITextHorisontalAlignment.Left);

        public static readonly DependencyProperty<GUITextVerticalAlignment> TextVerticalAlignmentProperty = new DependencyProperty<GUITextVerticalAlignment>(GUITextVerticalAlignment.Top);

        public static readonly DependencyProperty<SpriteFont> FontProperty = new DependencyProperty<SpriteFont>(FontChangedCallback, FontManager.DefaultFont, false);

        public Point Offset { get; set; }

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

        public GUITextHorisontalAlignment TextHorisontalAlignment
        {
            get
            {
                return GetValue(TextHorisontalAlignmentProperty);
            }
            set
            {
                SetValue(TextHorisontalAlignmentProperty, value);
            }
        }

        public GUITextVerticalAlignment TextVerticalAlignment
        {
            get
            {
                return GetValue(TextVerticalAlignmentProperty);
            }
            set
            {
                SetValue(TextVerticalAlignmentProperty, value);
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

        public Point TextSize
        {
            get
            {
                SpriteFont font = Font;
                if (font == null)
                {
                    return Point.Zero;
                }

                string text = Text;
                if (string.IsNullOrEmpty(text))
                {
                    return FontManager.GetDefaultCharacterSize(font);
                }

                return font.MeasureString(text).ToPoint();
            }
        }

        public Point ScaledTextSize
        {
            get
            {
                Vector2 textSize = TextSize.ToVector2();
                float scale = Scale;
                return (textSize * scale).ToPoint();
            }
        }

        public Point TextPostion
        {
            get
            {
                Point position = Rectangle.Location + Offset;
                Point textScale = ScaledTextSize;

                switch (TextHorisontalAlignment)
                {
                    case GUITextHorisontalAlignment.Left:
                        break;
                    case GUITextHorisontalAlignment.Middle:
                        position.X += Rectangle.Width / 2 - textScale.X / 2;
                        break;
                    case GUITextHorisontalAlignment.Right:
                        position.X += Rectangle.Width - textScale.X;
                        break;
                }

                switch (TextVerticalAlignment)
                {
                    case GUITextVerticalAlignment.Top:
                        break;
                    case GUITextVerticalAlignment.Middle:
                        position.Y += Rectangle.Height / 2 - textScale.Y / 2;
                        break;
                    case GUITextVerticalAlignment.Bottom:
                        position.Y += Rectangle.Height - textScale.Y;
                        break;
                }

                return position;
            }
        }

        public GUITextBlock()
        {

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

            Render.Begin(context.Mask);
            Render.DrawString(font, text, TextPostion, Color, Scale);
            Render.End();

            base.OnDraw(gameTime, context);
        }

        private void UpdateSize()
        {
            if (AutoScale)
            {
                Point localScale = LocalSize;
                Point scale = ScaledTextSize;

                if (HorizontalAlignment != GUIHorizontalAlignment.Stretch)
                {
                    localScale.X = scale.X;
                    LocalSize = localScale;
                }

                if (VerticalAlignment != GUIVerticalAlignment.Stretch)
                {
                    localScale.Y = scale.Y;
                    LocalSize = localScale;
                }
            }
        }

        public int GetIndexFromScreenPos(Point position)
        {
            return GetIndexFromLocalPos(ToLocal(position));
        }

        public int GetIndexFromLocalPos(Point position)
        {
            position -= Offset;

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

            int height = FontManager.GetHeight(font) + (int)font.Spacing;
            //int lineIndex = position.Y / height;                      //TODO OPTIMIZATION
            //int startIndex = text.FirstIndexOfLine(lineIndex);
            //string line = text.SubstringLine(startIndex);

            Dictionary<char, SpriteFont.Glyph> glyphs = font.GetGlyphs();
            float xPos = 0;
            float yPos = height / 2;
            float closestXDist = float.PositiveInfinity;
            float closestYDist = float.PositiveInfinity;
            int closestIndex = -1;

            for (int i = 0; i < text.Length + 1; i++)
            {
                float xDist = Math.Abs(position.X - xPos);
                float yDist = Math.Abs(position.Y - yPos);

                if (yDist < closestYDist)
                {
                    closestYDist = yDist;
                    closestXDist = float.PositiveInfinity;
                }

                if (NearlyEqual(yDist, closestYDist) && xDist < closestXDist)
                {
                    closestIndex = i;
                    closestXDist = xDist;
                }

                if (i == text.Length)
                {
                    break;
                }

                if (text[i] == '\n')
                {
                    yPos += height;
                    xPos = 0;
                }
                else if (glyphs.TryGetValue(text[i], out SpriteFont.Glyph glyph))
                {
                    xPos += glyph.WidthIncludingBearings + font.Spacing;
                }
            }

            return closestIndex;
        }

        public static bool NearlyEqual(double? value1, double? value2, double unimportantDifference = 0.0001)
        {
            if (value1 != value2)
            {
                if (value1 == null || value2 == null)
                    return false;

                return Math.Abs(value1.Value - value2.Value) < unimportantDifference;
            }

            return true;
        }

        private static void TextChangedCallback(DependencyObject dependencyObject, DependencyProperty<string> property, string oldValue, string newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }

        private static void AutoScaleChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }

        private static void ScaleChangedCallback(DependencyObject dependencyObject, DependencyProperty<float> property, float oldValue, float newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }

        private static void FontChangedCallback(DependencyObject dependencyObject, DependencyProperty<SpriteFont> property, SpriteFont oldValue, SpriteFont newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }
    }
}
