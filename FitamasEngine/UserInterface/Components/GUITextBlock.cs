using Fitamas.Fonts;
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

        public static readonly DependencyProperty<float> SizeProperty = new DependencyProperty<float>(ScaleChangedCallback, 10, false);

        public static readonly DependencyProperty<Color> ColorProperty = new DependencyProperty<Color>(Color.Black);

        public static readonly DependencyProperty<GUITextHorisontalAlignment> TextHorisontalAlignmentProperty = new DependencyProperty<GUITextHorisontalAlignment>(GUITextHorisontalAlignment.Left);

        public static readonly DependencyProperty<GUITextVerticalAlignment> TextVerticalAlignmentProperty = new DependencyProperty<GUITextVerticalAlignment>(GUITextVerticalAlignment.Top);

        public static readonly DependencyProperty<FontAtlas> FontProperty = new DependencyProperty<FontAtlas>(FontChangedCallback, FontManager.DefaultFont, false);

        public static readonly DependencyProperty<Thickness> PaddingProperty = new DependencyProperty<Thickness>(PaddingChangedCallback, Thickness.Zero, false);

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

        public float Size
        {
            get
            {
                return GetValue(SizeProperty);
            }
            set
            {
                SetValue(SizeProperty, value);
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

        public FontAtlas Font
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

        public Thickness Padding
        {
            get
            {
                return GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        public Point TextSize
        {
            get
            {
                FontAtlas font = Font;
                if (font == null)
                {
                    return Point.Zero;
                }

                string text = Text;
                if (string.IsNullOrEmpty(text))
                {
                    return new Point(0, font.Size);
                }

                return font.MeasureString(text).ToPoint();
            }
        }

        public Point ScaledTextSize
        {
            get
            {
                Thickness padding = Padding;
                return TextSize + new Point(padding.Left + padding.Right, padding.Top + padding.Bottom);
            }
        }

        public Point TextPostion
        {
            get
            {
                Thickness padding = Padding;
                Point position = Rectangle.Location + Offset + new Point(padding.Left, padding.Top);
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
            if (!IsVisible)
            {
                base.OnDraw(gameTime, context);
                return;
            }

            FontAtlas font = Font;
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
            Render.Draw(font, text, Size, TextPostion, Color, context.Alpha);
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

            FontAtlas font = Font;
            if (font == null)
            {
                return 0;
            }

            string text = Text;
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int height = font.Size + (int)font.Spacing;
            //int lineIndex = position.Y / height;                      //TODO OPTIMIZATION
            //int startIndex = text.FirstIndexOfLine(lineIndex);
            //string line = text.SubstringLine(startIndex);

            float xPos = 0;
            float yPos = height / 2;
            float closestXDist = float.PositiveInfinity;
            float closestYDist = float.PositiveInfinity;
            int closestIndex = -1;

            for (int i = 0; i < text.Length + 1; i++)
            {
                float xDist = global::System.Math.Abs(position.X - xPos);
                float yDist = global::System.Math.Abs(position.Y - yPos);

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
                else if (font.TryGetGlyph(text[i], out var glyph))
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

                return global::System.Math.Abs(value1.Value - value2.Value) < unimportantDifference;
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

        private static void FontChangedCallback(DependencyObject dependencyObject, DependencyProperty<FontAtlas> property, FontAtlas oldValue, FontAtlas newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }

        private static void PaddingChangedCallback(DependencyObject dependencyObject, DependencyProperty<Thickness> property, Thickness oldValue, Thickness newValue)
        {
            if (dependencyObject is GUITextBlock textBlock)
            {
                textBlock.UpdateSize();
            }
        }
    }
}
