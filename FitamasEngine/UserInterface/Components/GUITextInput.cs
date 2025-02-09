using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using Fitamas.UserInterface.Extensions;

namespace Fitamas.UserInterface.Components
{
    public enum GUIContentType
    {
        Standart,
        Float,
        Integer,
    }

    public class GUITextInput : GUIComponent, IMouseEvent, IKeyboardEvent
    {
        private const int CaretWidth = 2;

        public GUITextBlock TextBlock;
        public GUIContentType ContentType;
        public bool OneLine;
        public Color CaretColor = Color.Black;

        private bool isSelect;
        private bool caretEnable;
        private int caretIndex;
        private Point caretPosition;
        private Point caretSize;

        public GUIEvent<GUITextInput, Keys> OnSelected = new GUIEvent<GUITextInput, Keys>();
        public GUIEvent<GUITextInput, Keys> OnDeselected = new GUIEvent<GUITextInput, Keys>();
        public GUIEvent<GUITextInput, Keys> OnKeyHit = new GUIEvent<GUITextInput, Keys>();

        public GUIEvent<GUITextInput, string> OnTextChanged = new GUIEvent<GUITextInput, string>();

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

        public bool IsSelect
        {
            get
            {
                if (IsInHierarchy)
                {
                    if (System.Focused == this)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public GUITextInput(Rectangle rectangle, GUIContentType contentType = GUIContentType.Standart) : base(rectangle)
        {
            IsMask = true;
            RaycastTarget = true;
            ContentType = contentType;
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender contextRender)
        {
            if (isSelect)
            {

            }

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

                Render.Begin(contextRender.Mask);
                Render.FillRectangle(caretPosition, caretSize, CaretColor, 1);
                Render.End();
            }
        }

        protected override void OnDestroy()
        {
            Unselect();
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (Contain(mouse.Position))
            {
                Select(mouse.Position);
            }
            else
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
            OnKeyHit?.Invoke(this, keyboard.Key);

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
                    if (isSelect)
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
                    if (isSelect)
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
                            ReceiveTextInput(keyboard.Character);
                        }
                        break;
                    }
                default:
                    ReceiveTextInput(keyboard.Character);
                    break;
            }
        }

        public void Select(Point mousePosition)
        {
            if (IsInHierarchy)
            {
                System.Focused = this;
                isSelect = false;
                caretEnable = true;
                caretIndex = TextBlock.GetCaretIndexFromScreenPos(mousePosition);
                OnSelected?.Invoke(this, Keys.None);
            }
        }

        public void Unselect()
        {
            if (IsSelect)
            {
                System.Focused = null;
                isSelect = false;
                caretEnable = false;


                //string numberOnly = Regex.Replace(TextBlock.Text, "[^0-9.-]", "");

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

                OnDeselected?.Invoke(this, Keys.None);
            }
        }

        private void CalculateCaret()
        {
            caretSize = new Point(CaretWidth, TextBlock.Font.MeasureString(TextBlock.Text).ToPoint().Y);
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
                Point stringSize = TextBlock.Font.MeasureString(textBefore).ToPoint();
                caretOffset = new Point(stringSize.X, 0);
            }

            caretPosition = textPosition + caretOffset;
        }

        private void ReceiveTextInput(char? c)
        {
            if (!c.HasValue)
            {
                return;
            }

            string text = c.ToString();

            if (isSelect)
            {
                RemoveSelectedText();
            }
            int num = string.IsNullOrEmpty(text) ? 0 : text.Length;
            string currentText = TextBlock.Text;
            if (string.IsNullOrEmpty(currentText))
            {
                SetText(text);
            }
            else
            {
                TextBlock.Text = currentText.Insert(caretIndex, text);
                OnTextChanged?.Invoke(this, TextBlock.Text);
            }
            caretIndex += num;
        }

        private void SetText(string text)
        {
            TextBlock.Text = text;
            OnTextChanged?.Invoke(this, TextBlock.Text);
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
                OnTextChanged?.Invoke(this, TextBlock.Text);
            }
        }

        private void RemoveSelectedText()
        {

        }
    }
}
