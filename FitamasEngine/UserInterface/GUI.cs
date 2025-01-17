using Fitamas.Events;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.UserInterface.NodeEditor;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public static class GUI
    {
        public static GUIImage CreateImage(Sprite sprite, Rectangle rectangle)
        {
            GUIImage image = new GUIImage(rectangle);
            image.Sprite = sprite;

            return image;
        }

        public static GUIButton CreateButton(Rectangle rectangle, string text)
        {
            GUIButton button = new GUIButton(rectangle);

            GUITextBlock textBlock = CreateTextBlock(text);
            textBlock.Stretch = GUIStretch.HorizontalAndVertical;
            textBlock.TextAligment = GUITextAligment.Middle;
            textBlock.Text = text;
            button.AddChild(textBlock);
            button.TextBlock = textBlock;

            GUIImage image = new GUIImage(new Rectangle());
            image.Stretch = GUIStretch.HorizontalAndVertical;
            button.AddChild(image);
            button.Image = image;

            return button;
        }

        public static GUIButton CreateButton()
        {
            GUIButton button = new GUIButton();

            GUIImage image = new GUIImage(new Rectangle());
            image.Stretch = GUIStretch.HorizontalAndVertical;
            button.AddChild(image);
            button.Image = image;

            return button;
        }

        public static GUIGridGroup CreateGroup(Point maxSize)
        {
            GUIGridGroup group = new GUIGridGroup(maxSize);

            return group;
        }

        public static GUIHorizontalGroup CreateHorizontalGroup()
        {
            GUIHorizontalGroup group = new GUIHorizontalGroup();

            return group;
        }

        public static GUIVerticalGroup CreateVerticalGroup()
        {
            GUIVerticalGroup group = new GUIVerticalGroup();

            return group;
        }

        public static GUITextBlock CreateTextBlock(string text)
        {
            GUITextBlock textBlock = new GUITextBlock(GUIStyle.DefoultFont, text);
            textBlock.AutoScale = true;

            return textBlock;
        }

        public static GUITextInput CreateTextInput(Rectangle rectangle)
        {
            GUITextInput input = new GUITextInput(rectangle);
            input.CaretColor = GUIStyle.TextColor;

            GUITextBlock textBlock = CreateTextBlock("Text");
            textBlock.Anchor = new Vector2(0, 0.5f);
            textBlock.Pivot = new Vector2(0, 0.5f);
            textBlock.Color = GUIStyle.TextColor;
            input.AddChild(textBlock);
            input.TextBlock = textBlock;

            return input;
        }

        public static GUIFrame CreateFieldInput(Point scale, MonoAction<GUITextInput, string> onValueChanged = null, string name = "Text", string defoultValue = " ")
        {
            GUIFrame frame = new GUIFrame();
            frame.LocalScale = scale;
            
            GUITextBlock text = CreateTextBlock(name);
            text.AutoScale = true;
            text.Color = GUIStyle.TextColor;
            text.Pivot = new Vector2(0, 0.5f);
            text.Anchor = new Vector2(0, 0.5f);
            frame.AddChild(text);

            GUITextInput input = CreateTextInput(new Rectangle());
            input.OnTextChanged.AddListener(onValueChanged);
            input.Stretch = GUIStretch.HorizontalAndVertical;
            input.LocalPosition = new Point(text.LocalScale.X, 0);
            input.Text = defoultValue;
            frame.AddChild(input);

            GUIImage image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            image.LocalPosition = new Point(text.LocalScale.X, 0);
            image.Color = GUIStyle.BackgroundTitle;
            frame.AddChild(image);

            return frame;
        }

        public static GUITrackBar CreateTrackBar(Rectangle rectangle)
        {
            GUITrackBar trackBar = new GUITrackBar(rectangle);

            GUIImage slidingArea = new GUIImage(new Rectangle());
            slidingArea.Stretch = GUIStretch.HorizontalAndVertical;
            trackBar.AddChild(slidingArea);
            trackBar.SlidingArea = slidingArea;

            GUIImage handle = new GUIImage(new Rectangle(new Point(), rectangle.Size));
            handle.Alignment = GUIAlignment.Center;
            trackBar.AddChild(handle);
            trackBar.Handle = handle;

            trackBar.HandleSize = new Point(30, 50);

            return trackBar;    
        }

        public static GUIScrollBar CreateScrollBar(Rectangle rectangle)
        {
            GUIScrollBar scrollBar = new GUIScrollBar(rectangle);

            GUIImage slidingArea = new GUIImage(new Rectangle());
            slidingArea.Stretch = GUIStretch.HorizontalAndVertical;
            scrollBar.AddChild(slidingArea);
            scrollBar.SlidingArea = slidingArea;

            GUIImage handle = new GUIImage(new Rectangle(new Point(), rectangle.Size));
            handle.Alignment = GUIAlignment.Center;
            scrollBar.AddChild(handle);
            scrollBar.Handle = handle;

            scrollBar.HandleSize = new Point(30, 50);

            return scrollBar;
        }

        public static GUIScrollRect CreateScrollRect(GUIComponent content, Rectangle rectangle)
        {
            GUIScrollRect scrollRect = new GUIScrollRect(content, rectangle);

            GUIScrollBar horizontalScrollBar = CreateScrollBar(new Rectangle(new Point(), new Point(0, 20)));
            horizontalScrollBar.Stretch = GUIStretch.Horizontal;
            horizontalScrollBar.Alignment = GUIAlignment.LeftDown;
            horizontalScrollBar.Pivot = new Vector2(0, 1);
            horizontalScrollBar.Direction = GUISliderDirection.LeftToRight;
            scrollRect.AddChild(horizontalScrollBar);
            scrollRect.HorizontalScrollBar = horizontalScrollBar;

            GUIScrollBar verticalScrollBar = CreateScrollBar(new Rectangle(new Point(), new Point(0, 20)));
            verticalScrollBar.Stretch = GUIStretch.Vertical;
            verticalScrollBar.Alignment = GUIAlignment.RightUp;
            verticalScrollBar.Pivot = new Vector2(1, 0);
            verticalScrollBar.Direction = GUISliderDirection.TopToBottom;
            scrollRect.AddChild(verticalScrollBar);
            scrollRect.VerticalScrollBar = verticalScrollBar;

            return scrollRect;
        }

        public static GUICheckBox CreateCheckBox(Rectangle rectangle)
        {
            GUICheckBox checkBox = new GUICheckBox(rectangle);

            GUIImage checkmark = new GUIImage(new Rectangle());
            checkmark.Stretch = GUIStretch.HorizontalAndVertical;
            checkBox.AddChild(checkmark);
            checkBox.Checkmark = checkmark;

            GUIImage backGround = new GUIImage(new Rectangle());
            backGround.Stretch = GUIStretch.HorizontalAndVertical;
            checkBox.AddChild(backGround);
            checkBox.BackGround = backGround;

            return checkBox;
        }

        public static GUIComboBox CreateComboBox(Rectangle rectangle, IEnumerable<string> items)
        {
            GUIComboBox comboBox = new GUIComboBox(rectangle, items);

            GUIVerticalGroup group = new GUIVerticalGroup();
            group.Pivot = new Vector2();
            group.Alignment = GUIAlignment.LeftDown;
            comboBox.AddChild(group);
            comboBox.Group = group;

            return comboBox;
        }

        public static GUITreeNode CreateTreeNode(string text = "item")
        {
            GUITreeNode node = new GUITreeNode();

            GUITextBlock textBlock = CreateTextBlock(text);
            textBlock.AutoScale = true;
            textBlock.Text = text;
            node.TextBlock = textBlock;
            node.AddChild(textBlock);            

            GUIImage image = new GUIImage();
            node.FolderIconOpen = image;
            node.AddChild(image);            

            image = new GUIImage();
            node.FolderIconClose = image;
            node.AddChild(image);            

            image = new GUIImage();
            node.Icon = image;
            node.AddChild(image);

            image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            node.Image = image;
            node.AddChild(image);

            return node;
        }

        public static GUINode CreateNode(Rectangle rectangle, string text = "header")
        {
            GUINode node = new GUINode(rectangle);

            GUITextBlock textBlock = CreateTextBlock(text);
            textBlock.Alignment = GUIAlignment.Center;
            textBlock.AutoScale = true;
            node.HeaderTextBlock = textBlock;

            GUIImage image = new GUIImage();
            image.Stretch = GUIStretch.Horizontal;
            image.Pivot = new Vector2(0, 0);
            node.HeaderImage = image;
            node.AddChild(image);
            image.AddChild(textBlock);

            image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            node.Image = image;
            node.AddChild(image);

            image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            node.SelectedImage = image;
            node.AddChild(image);

            return node;
        }

        public static GUIPin CreatePin(string text, GUIPinType type, GUIPinAlignment pinAlignment)
        {
            GUIPin pin = CreatePin(type, pinAlignment);

            GUITextBlock textBlock = CreateTextBlock(text);
            textBlock.AutoScale = true;
            pin.Content = textBlock;
            pin.AddChild(textBlock);

            return pin;
        }

        public static GUIPin CreatePin(GUIPinType type, GUIPinAlignment pinAlignment)
        {
            GUIPin pin = new GUIPin(type);
            pin.PinAlignment = pinAlignment;

            GUIImage image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            pin.ImageOn = image;
            pin.AddChild(image);

            image = new GUIImage();
            image.Stretch = GUIStretch.HorizontalAndVertical;
            pin.ImageOff = image;
            pin.AddChild(image);

            return pin;
        }

        public static GUIWire CreateWire()
        {
            GUIWire wire = new GUIWire();
            wire.LocalScale = new Point(1, 1);

            return wire;
        }

        public static GUIContextMenu CreateContextMenu()
        {
            Point mousePosition = InputSystem.mouse.MousePosition;
            GUICanvas canvas = GUISystem.GetActiveLayout().Canvas;
            Point localPosition = canvas.ToLocalPosition(mousePosition);
            return CreateContextMenu(localPosition);
        }

        public static GUIContextMenu CreateContextMenu(Point mousePosition)
        {
            GUIContextMenu context = new GUIContextMenu();
            context.LocalPosition = mousePosition;
            context.Group.CellSize = GUIStyle.ContextItemSize;
            return context;
        }
    }
}
