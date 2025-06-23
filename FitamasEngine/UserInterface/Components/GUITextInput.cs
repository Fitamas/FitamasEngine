using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using Fitamas.UserInterface.Input;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components
{
    public enum GUIContentType
    {
        Default,
        Float,
        Integer,
    }

    public class GUITextInput : GUIComponent, IMouseEvent, IDragMouseEvent, IKeyboardEvent
    {
        public static readonly DependencyProperty<bool> OneLineProperty = new DependencyProperty<bool>(true);

        public static readonly DependencyProperty<GUIContentType> ContentTypeProperty = new DependencyProperty<GUIContentType>(GUIContentType.Default);

        public static readonly DependencyProperty<int> CaretWidthProperty = new DependencyProperty<int>(2);

        public static readonly DependencyProperty<Color> CaretColorProperty = new DependencyProperty<Color>(Color.Black);

        public static readonly DependencyProperty<Color> SelectColorProperty = new DependencyProperty<Color>(new Color(0, 0, 0.5f, 0.2f));

        public static readonly RoutedEvent OnSelectEvent = new RoutedEvent();

        public static readonly RoutedEvent OnDeselectEvent = new RoutedEvent();

        public static readonly RoutedEvent OnValueChangedEvent = new RoutedEvent();

        public static readonly RoutedEvent OnEndEditEvent = new RoutedEvent();

        private bool caretEnable;
        private int caretIndex;
        private bool isSelect;
        private int startSelectIndex;

        public GUITextBlock TextBlock { get; set; }

        public MonoEvent<GUITextInput> OnSelect { get; }
        public MonoEvent<GUITextInput> OnDeselect { get; }
        public MonoEvent<GUITextInput, string> OnValueChanged { get; }
        public MonoEvent<GUITextInput, string> OnEndEdit { get; }

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

        public Color SelectColor
        { 
            get
            {
                return GetValue(SelectColorProperty);
            }
            set
            {
                SetValue(SelectColorProperty, value);
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
                OnValueChanged.Invoke(this, value);
                RaiseEvent(new GUIEventArgs(OnValueChangedEvent, this));
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

        public int CaretIndex
        {
            get
            {
                return caretIndex;
            }
            set
            {
                caretIndex = global::System.Math.Clamp(value, 0, TextLenght);
            }
        }

        public int CaretIndexOnLine
        {
            get
            {
                string text = Text;
                int start = global::System.Math.Clamp(caretIndex - 1, 0, text.Length > 0 ? text.Length - 1 : 0);
                int index = text.LastIndexOf('\n', start);
                return index == -1 ? caretIndex : caretIndex - index - 1;
            }
        }

        public GUITextInput()
        {
            IsMask = true;
            RaycastTarget = true;

            OnSelect = new MonoEvent<GUITextInput>();
            OnDeselect = new MonoEvent<GUITextInput>();
            OnValueChanged = new MonoEvent<GUITextInput, string>();
            OnEndEdit = new MonoEvent<GUITextInput, string>();
        }

        protected override void OnFocus()
        {
            caretEnable = true;
            System.Keyboard.Focused = this;
            Unselect();
        }

        protected override void OnUnfocus()
        {
            caretEnable = false;
            System.Keyboard.Focused = null;
            Unselect();

            switch (ContentType)
            {
                case GUIContentType.Integer:
                    int valueInt = Text.ConvertToInt();
                    Text = valueInt.ToString();
                    break;
                case GUIContentType.Float:
                    float valueFloat = Text.ConvertToInt();
                    Text = valueFloat.ToString();
                    break;
            }
            OnEndEdit.Invoke(this, Text);
            RaiseEvent(new GUIEventArgs(OnEndEditEvent, this));
        }

        protected override void OnDraw(GameTime gameTime, GUIContextRender context)
        {
            base.OnDraw(gameTime, context);

            if (!IsVisible)
            {
                return;
            }

            SpriteFont font = TextBlock.Font;
            if (font == null)
            {
                return;
            }

            string text = Text;
            int lineHeight = FontManager.GetHeight(font);

            if (isSelect && !string.IsNullOrEmpty(text))
            {
                int min = global::System.Math.Min(startSelectIndex, CaretIndex);
                int max = global::System.Math.Max(startSelectIndex, CaretIndex);
                int lineCount = 0;
                int lineStart = 0;

                for (int i = 0; i < text.Length + 1; i++)
                {
                    if (i == text.Length || text[i] == '\n')
                    {
                        bool before = false;
                        int start = -1;
                        int end = -1;

                        if (i > min && lineStart < max)
                        {
                            if (min > lineStart)
                            {
                                start = min;
                                before = true;
                            }
                            else
                            {
                                start = lineStart;
                            }
                        }

                        if (lineStart < max)
                        {
                            if (i == text.Length && i <= max)
                            {
                                end = text.Length;
                            }
                            else if (i > max)
                            {
                                end = max;
                            }
                            else
                            {
                                end = i;
                            }
                        }

                        if (start != -1 && end != -1)
                        {
                            int indent = before ? (int)font.MeasureString(text.Substring(lineStart, start - lineStart)).X : 0;
                            string line = text.Substring(start, end - start);
                            if (string.IsNullOrEmpty(line))
                            {
                                line = font.DefaultCharacter.Value.ToString();
                            }
                            Point lineSize = font.MeasureString(line).ToPoint();
                            Point linePosition = TextBlock.TextPostion + new Point(indent, lineCount * lineHeight);

                            Render.Begin(context.Mask);
                            Render.FillRectangle(linePosition, lineSize, SelectColor);
                            Render.End();
                        }

                        lineStart = i + 1;
                        lineCount++;
                    }
                }
            }

            if (caretEnable)
            {
                int start = CaretIndex == 0 ? 0 : text.LastIndexOf('\n', CaretIndex - 1);
                if (start == -1)
                {
                    start = 0;
                }

                int stringX = 0;
                int stringY = 0;

                if (!string.IsNullOrEmpty(text))
                {
                    string textBefore = text.Substring(start, CaretIndex - start);
                    stringX = (int)font.MeasureString(textBefore).X;
                    stringY = lineHeight * text.CountOf('\n', 0, CaretIndex);
                }

                CalculateTextOffset(new Point(stringX, stringY), lineHeight);

                Point caretSize = new Point(CaretWidth, lineHeight);
                Point caretPosition = TextBlock.TextPostion + new Point(stringX + (int)font.Spacing / 2 - caretSize.X / 2, stringY);

                Render.Begin(context.Mask);
                Render.FillRectangle(caretPosition, caretSize, CaretColor);
                Render.End();
            }
        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            if (IsMouseOver && !IsFocused)
            {
                Focus();
            }
            else
            {
                Unselect();
            }

            CaretIndex = TextBlock.GetIndexFromScreenPos(mouse.Position);
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
        {

        }

        public void OnStartDragMouse(GUIMouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                startSelectIndex = TextBlock.GetIndexFromScreenPos(mouse.Position);
                Select();
            }
        }

        public void OnDragMouse(GUIMouseEventArgs mouse)
        {
            if (isSelect)
            {
                CaretIndex = TextBlock.GetIndexFromScreenPos(mouse.Position);
                Select();
            }
        }

        public void OnEndDragMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnKeyDown(GUIKeyboardEventArgs keyboard)
        {

        }

        public void OnKeyUP(GUIKeyboardEventArgs keyboard)
        {

        }

        public void OnKey(GUIKeyboardEventArgs keyboard)
        {
            if (!IsFocused)
            {
                return;
            }

            switch (keyboard.Key)
            {
                case Keys.Left:
                    CaretLeft();
                    break;
                case Keys.Right:
                    CaretRight();
                    break;
                case Keys.Up:
                    CaretUp();
                    break;
                case Keys.Down:
                    CaretDown();
                    break;
                case Keys.Back:
                    if (isSelect)
                    {
                        RemoveSelectedText();
                    }
                    else
                    {
                        RemoveChar(CaretIndex - 1);
                        CaretIndex--;
                    }
                    break;
                case Keys.Delete:
                    if (isSelect)
                    {
                        RemoveSelectedText();
                    }
                    else
                    {
                        RemoveChar(CaretIndex);
                    }
                    break;
                case Keys.Escape:
                    {
                        Unfocus();
                        break;
                    }
                case Keys.Tab:
                    {
                        if (OneLine)
                        {
                            Unfocus();
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
                            Unfocus();
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

            Unselect();
        }

        public void Select()
        {
            isSelect = true;
            OnSelect.Invoke(this);
            RaiseEvent(new GUIEventArgs(OnSelectEvent, this));
        }

        public void Unselect()
        {
            isSelect = false;
            OnDeselect.Invoke(this);
            RaiseEvent(new GUIEventArgs(OnDeselectEvent, this));
        }

        public void RemoveSelectedText()
        {
            int min = global::System.Math.Min(startSelectIndex, CaretIndex);
            int max = global::System.Math.Max(startSelectIndex, CaretIndex);
            Unselect();
            CaretIndex = min;
            Text = Text.Remove(min, max - min);
        }

        private void CalculateTextOffset(Point caretLocalPosition, int lineHeight)
        {
            Rectangle rectangle = TextBlock.Rectangle;
            Point caretPosition = TextBlock.TextPostion + caretLocalPosition;
            Point textSize = TextBlock.ScaledTextSize;
            Point offset = TextBlock.Offset;

            if (textSize.X < rectangle.Width)
            {
                offset.X = 0;
            }
            else if (caretPosition.X < rectangle.Left)
            {
                offset.X += rectangle.Left - caretPosition.X;
            }
            else if (caretPosition.X > rectangle.Right)
            {
                offset.X += rectangle.Right - caretPosition.X;
            }

            if (textSize.Y < rectangle.Height)
            {
                offset.Y = 0;
            }
            else if (caretPosition.Y > rectangle.Bottom - lineHeight)
            {
                offset.Y += rectangle.Bottom - caretPosition.Y - lineHeight;
            }
            else if (caretPosition.Y < rectangle.Top)
            {
                offset.Y += rectangle.Top - caretPosition.Y;
            }

            TextBlock.Offset = offset;
        }

        private void ReceiveTextInput(char? c)
        {
            if (!c.HasValue)
            {
                return;
            }

            if (isSelect)
            {
                RemoveSelectedText();
            }

            string text = c.Value.ToString();

            int num = string.IsNullOrEmpty(text) ? 0 : text.Length;
            string currentText = Text;
            if (string.IsNullOrEmpty(currentText))
            {
                Text = text;
            }
            else
            {
                Text = currentText.Insert(CaretIndex, text);
            }

            CaretIndex += num;
        }

        private void RemoveChar(int position)
        {
            if (position < 0 || position >= TextLenght)
            {
                return;
            }

            string currentText = Text;
            if (!string.IsNullOrEmpty(currentText))
            {
                Text = currentText.Remove(position, 1);
            }
        }

        public void CaretLeft()
        {
            CaretIndex--;
        }

        public void CaretRight()
        {
            CaretIndex++;
        }

        public void CaretUp()
        {
            if (CaretIndex == 0)
            {
                return;
            }

            string text = Text;
            int end = text.LastIndexOf('\n', CaretIndex - 1);

            if (end != -1)
            {
                int start = text.LastIndexOf('\n', end - 1) + 1;
                CaretIndex = global::System.Math.Clamp(CaretIndexOnLine + start, start, end);
            }
        }

        public void CaretDown()
        {
            string text = Text;
            int start = text.IndexOf('\n', CaretIndex);

            if (start != -1)
            {
                start++;
                int end = text.IndexOf('\n', start);
                if (end == -1)
                {
                    end = text.Length;
                }

                CaretIndex = global::System.Math.Clamp(CaretIndexOnLine + start, start, end);
            }
        }
    }
}
