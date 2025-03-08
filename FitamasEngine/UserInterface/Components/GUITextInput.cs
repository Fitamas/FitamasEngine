using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public enum GUIContentType
    {
        Default,
        Float,
        Integer,
    }

    public class GUITextInput : GUIComponent, IMouseEvent, IKeyboardEvent
    {
        public static readonly DependencyProperty<bool> OneLineProperty = new DependencyProperty<bool>(true);

        public static readonly DependencyProperty<GUIContentType> ContentTypeProperty = new DependencyProperty<GUIContentType>(GUIContentType.Default);

        public static readonly DependencyProperty<int> CaretWidthProperty = new DependencyProperty<int>(2);

        public static readonly DependencyProperty<Color> CaretColorProperty = new DependencyProperty<Color>(Color.Black);

        private bool caretEnable;
        private int caretIndex;
        private Point caretPosition;
        private Point caretSize;
        private bool selectText;

        public GUITextBlock TextBlock { get; set; }

        public GUIEvent<GUITextInput, string> OnSelect { get; }
        public GUIEvent<GUITextInput, string> OnDeselect { get; }
        public GUIEvent<GUITextInput, string> OnValueChanged { get; }
        public GUIEvent<GUITextInput, string> OnEndEdit { get; }

        public bool OneLine
        {
            get
            {
                return GetValue(OneLineProperty);
            }
            set
            {
                SetValue(OneLineProperty, value);
            }
        }

        public GUIContentType ContentType
        {
            get
            {
                return GetValue(ContentTypeProperty);
            }
            set
            {
                SetValue(ContentTypeProperty, value);
            }
        }

        public int CaretWidth
        {
            get
            {
                return GetValue(CaretWidthProperty);
            }
            set
            {
                SetValue(CaretWidthProperty, value);
            }
        }

        public Color CaretColor
        {
            get
            {
                return GetValue(CaretColorProperty);
            }
            set
            {
                SetValue(CaretColorProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return TextBlock.Text;
            }
            set
            {
                TextBlock.Text = value;
            }
        }

        public int TextLenght
        {
            get
            {
                if (!string.IsNullOrEmpty(TextBlock.Text))
                {
                    return TextBlock.Text.Length;
                }
                return 0;
            }
        }

        public GUITextInput()
        {
            IsMask = true;
            RaycastTarget = true;
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            base.OnDraw(gameTime, context);

            if (caretEnable)
            {
                CalculateCaret();

                Rectangle rectangle = Rectangle;
                if (caretPosition.X < rectangle.Left)
                {
                    TextBlock.LocalPosition += new Point((rectangle.Left - caretPosition.X), 0);
                    CalculateCaret();
                }
                else if (caretPosition.X > rectangle.Right)
                {
                    TextBlock.LocalPosition -= new Point((caretPosition.X - rectangle.Right), 0);
                    CalculateCaret();
                }

                Render.Begin(context.Mask);
                Render.FillRectangle(caretPosition, caretSize, CaretColor);
                Render.End();
            }
        }

        protected override void OnDestroy()
        {
            Unselect();
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                Focus();
                caretEnable = true;
                caretIndex = TextBlock.GetCaretIndexFromScreenPos(mouse.Position);
            }
            else if (IsFocused)
            {
                Unselect();
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }

        //private int FindNearestGlyphIndex(IGuiContext context, Point position)
        //{
        //    var font = Font ?? context.DefaultFont;
        //    var textInfo = GetTextInfo(context, Text, ContentRectangle, HorizontalTextAlignment, VerticalTextAlignment);
        //    var i = 0;

        //    foreach (var glyph in font.GetGlyphs(textInfo.Text, textInfo.Position))
        //    {
        //        var fontRegionWidth = glyph.FontRegion?.Width ?? 0;
        //        var glyphMiddle = (int)(glyph.Position.X + fontRegionWidth * 0.5f);

        //        if (position.X >= glyphMiddle)
        //        {
        //            i++;
        //            continue;
        //        }

        //        return i;
        //    }

        //    return i;
        //}

        public void OnKeyDown(KeyboardEventArgs keyboard)
        {

        }

        public void OnKeyUP(KeyboardEventArgs keyboard)
        {

        }

        public void OnKey(KeyboardEventArgs keyboard)
        {
            if (!IsFocused)
            {
                return;
            }

            switch (keyboard.Key)
            {
                case Keys.Left:
                    caretIndex--;
                    break;
                case Keys.Right:
                    caretIndex++;
                    break;
                case Keys.Up:
                    break;
                case Keys.Down:
                    break;
                case Keys.Back:
                    if (selectText)
                    {
                        RemoveSelectedText();
                    }
                    else
                    {
                        RemoveChar(caretIndex - 1);
                        caretIndex--;
                    }
                    break;
                case Keys.Delete:
                    if (selectText)
                    {
                        RemoveSelectedText();
                    }
                    else
                    {
                        RemoveChar(caretIndex);
                    }
                    break;
                case Keys.Escape:
                    {
                        Unselect();
                        break;
                    }
                case Keys.Tab:
                    {
                        if (OneLine)
                        {
                            Unselect();
                        }
                        else
                        {
                            ReceiveTextInput(keyboard.Character);
                        }
                        break;
                    }
                case Keys.Enter:
                    {
                        if (OneLine)
                        {
                            Unselect();
                        }
                        else
                        {
                            ReceiveTextInput('\n');
                        }
                        break;
                    }
                default:
                    ReceiveTextInput(keyboard.Character);
                    break;
            }
        }

        public void Unselect()
        {
            if (IsFocused)
            {
                Unfocus();
                caretEnable = false;

                switch (ContentType)
                {
                    case GUIContentType.Integer:
                        int valueInt = TextBlock.Text.ConvertToInt();
                        SetText(valueInt.ToString());
                        break;
                    case GUIContentType.Float:
                        float valueFloat = TextBlock.Text.ConvertToInt();
                        SetText(valueFloat.ToString());
                        break;
                }
            }
        }

        private void CalculateCaret()
        {
            SpriteFont font = TextBlock.Font;
            caretSize = new Point(CaretWidth, font.MeasureString(TextBlock.Text).ToPoint().Y);
            Point textPosition = TextBlock.Rectangle.Location;
            Point caretOffset = Point.Zero;

            if (caretIndex < 0)
            {
                caretIndex = 0;
            }
            else if (caretIndex > TextLenght) 
            { 
                caretIndex = TextLenght;
            }

            if (TextLenght > 0)
            {
                string textBefore = TextBlock.Text.Substring(0, caretIndex);
                Point stringSize = font.MeasureString(textBefore).ToPoint();
                caretOffset = new Point(stringSize.X + (int)font.Spacing / 2 - caretSize.X / 2, 0);
            }

            caretPosition = textPosition + caretOffset;
        }

        private void ReceiveTextInput(char? c)
        {
            if (!c.HasValue)
            {
                return;
            }

            string text = c.Value.ToString();

            RemoveSelectedText();

            int num = string.IsNullOrEmpty(text) ? 0 : text.Length;
            string currentText = TextBlock.Text;
            if (string.IsNullOrEmpty(currentText))
            {
                SetText(text);
            }
            else
            {
                TextBlock.Text = currentText.Insert(caretIndex, text);
                OnValueChanged?.Invoke(this, TextBlock.Text);
            }

            caretIndex += num;
        }

        private void SetText(string text)
        {
            TextBlock.Text = text;
            OnValueChanged?.Invoke(this, TextBlock.Text);
        }

        private void RemoveChar(int position)
        {
            if (position < 0 || position >= TextLenght)
            {
                return;
            }

            string currentText = TextBlock.Text;
            if (!string.IsNullOrEmpty(currentText))
            {
                TextBlock.Text = currentText.Remove(position, 1);
                OnValueChanged?.Invoke(this, TextBlock.Text);
            }
        }

        private void RemoveSelectedText()
        {

        }
    }
}
