using Fitamas.Events;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Fitamas.UserInterface.Components.NodeEditor;

namespace Fitamas.UserInterface
{
    public static class GUI
    {
        public static GUIImage CreateImage(Sprite sprite, Point position, Point size)
        {
            GUIImage image = new GUIImage();
            image.LocalPosition = position;
            image.LocalSize = size;
            image.Sprite = sprite;
            return image;
        }

        public static GUIButton CreateButton(Point position, string text, Point? size = null)
        {
            return CreateButton(ResourceDictionary.DefaultResources, position, text, size);
        }

        public static GUIButton CreateButton(ResourceDictionary dictionary, Point position, string text, Point? size = null)
        {
            return CreateButton(dictionary[CommonResourceKeys.ButtonStyle] as GUIStyle, position, text, size);
        }

        public static GUIButton CreateButton(GUIStyle style, Point position, string text, Point? size = null)
        {
            GUIButton button = new GUIButton();
            button.Style = style;
            button.LocalPosition = position;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            button.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            textBlock.TextAligment = GUITextAligment.Middle;
            textBlock.SetAlignment(GUIAlignment.Center);
            button.AddChild(textBlock);

            if (size.HasValue)
            {
                button.LocalSize = size.Value;
            }
            else if (style != null)
            {
                Point padding = style.Resources.FramePadding;
                Point localSize = padding + padding + textBlock.Font.MeasureString(text).ToPoint();

                button.LocalSize = localSize;
            }            

            return button;
        }

        public static GUITextBlock CreateTextBlock(Point position, string text)
        {
            return CreateTextBlock(ResourceDictionary.DefaultResources, position, text);
        }

        public static GUITextBlock CreateTextBlock(ResourceDictionary dictionary, Point position, string text)
        {
            return CreateTextBlock((GUIStyle)dictionary[CommonResourceKeys.TextBlockStyle], position, text);
        }

        public static GUITextBlock CreateTextBlock(GUIStyle style, Point position, string text)
        {
            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Style = style;
            textBlock.LocalPosition = position;
            textBlock.Text = text;

            return textBlock;
        }

        public static GUICheckBox CreateCheckBox(Point position)
        {
            return CreateCheckBox(ResourceDictionary.DefaultResources, position);
        }

        public static GUICheckBox CreateCheckBox(ResourceDictionary dictionary, Point position)
        {
            return CreateCheckBox((GUIStyle)dictionary[CommonResourceKeys.CheckBoxStyle], position);
        }

        public static GUICheckBox CreateCheckBox(GUIStyle style, Point position)
        {
            Point padding = style.Resources.FramePadding;
            Point size = new Point(FontManager.GetHeight()) + padding + padding;

            GUICheckBox checkBox = new GUICheckBox();
            checkBox.LocalPosition = position;
            checkBox.LocalSize = size;

            GUIImage backGround = new GUIImage();
            backGround.SetAlignment(GUIAlignment.Stretch);
            checkBox.AddChild(backGround);

            GUIImage checkmark = new GUIImage();
            checkmark.Name = GUICheckBoxStyle.CheckMark;
            checkmark.SetAlignment(GUIAlignment.Stretch);
            checkBox.AddChild(checkmark);

            checkBox.Style = style;

            return checkBox;
        }

        public static GUIComboBox CreateComboBox(Point position, IEnumerable<string> items, Point? size = null)
        {
            return CreateComboBox(ResourceDictionary.DefaultResources, position, items, size);
        }

        public static GUIComboBox CreateComboBox(ResourceDictionary dictionary, Point position, IEnumerable<string> items, Point? size = null)
        {
            return CreateComboBox(dictionary[CommonResourceKeys.ComboBoxStyle] as GUIStyle, position, items, size);
        }

        public static GUIComboBox CreateComboBox(GUIStyle style, Point position, IEnumerable<string> items, Point? size = null)
        {
            Point padding = style.Resources.FramePadding;

            GUIComboBox comboBox = new GUIComboBox(items);
            comboBox.LocalPosition = position;
            comboBox.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            comboBox.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            textBlock.VerticalAlignment = GUIVerticalAlignment.Stretch;
            textBlock.TextAligment = GUITextAligment.Left;
            textBlock.AutoScale = false;
            textBlock.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            comboBox.AddChild(textBlock);

            if (size.HasValue)
            {
                comboBox.LocalSize = size.Value;
            }
            else if (style != null)
            {
                Point localSize = Point.Zero; //TODO fix
                foreach (var item in items)
                {
                    localSize = textBlock.Font.MeasureString(item).ToPoint();
                }

                comboBox.LocalSize = padding + padding + localSize;
            }

            return comboBox;
        }

        public static GUIContextMenu CreateContextMenu(Point position)
        {
            return CreateContextMenu(ResourceDictionary.DefaultResources, position);
        }

        public static GUIContextMenu CreateContextMenu(ResourceDictionary dictionary, Point position)
        {
            return CreateContextMenu(dictionary[CommonResourceKeys.ContextMenuStyle] as GUIStyle, position);
        }

        public static GUIContextMenu CreateContextMenu(GUIStyle style, Point position)
        {
            Point padding = style.Resources.WindowPadding;

            GUIContextMenu context = new GUIContextMenu();
            context.LocalPosition = position;
            context.Pivot = new Vector2(0, 1);
            context.Style = style;
            context.Padding = new Thickness(padding.X, padding.Y, padding.X, padding.Y);

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            context.AddChild(image);

            GUIStack group = new GUIStack();
            group.LocalPosition = new Point(padding.X, padding.Y);
            group.Pivot = new Vector2(0, 0);
            group.Orientation = GUIGroupOrientation.Vertical;
            group.ControlChildSizeWidth = true;
            group.ControlSizeWidth = true;
            group.ControlSizeHeight = true;
            context.AddChild(group);
            context.Content = group;
            context.Group = group;

            return context;
        }

        public static GUIContextItem CreateContextItem(string text)
        {
            return CreateContextItem(ResourceDictionary.DefaultResources, text);
        }

        public static GUIContextItem CreateContextItem(ResourceDictionary dictionary, string text)
        {
            return CreateContextItem(dictionary[CommonResourceKeys.ContextItemStyle] as GUIStyle, text);
        }

        public static GUIContextItem CreateContextItem(GUIStyle style, string text)
        {
            Point padding = style.Resources.FramePadding;

            GUIContextItem item = new GUIContextItem();
            item.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            item.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            textBlock.SetAlignment(GUIAlignment.Stretch);
            textBlock.AutoScale = false;
            textBlock.Text = text;
            textBlock.TextAligment = GUITextAligment.Left;
            item.AddChild(textBlock);
            
            item.LocalSize = padding + padding + textBlock.Font.MeasureString(text).ToPoint();

            return item;
        }

        public static GUITextInput CreateTextInput(Rectangle rectangle)
        {
            GUITextInput input = new GUITextInput();

            //input.CaretColor = GUIStyle.TextColor;

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = "Text";
            textBlock.VerticalAlignment = GUIVerticalAlignment.Center;
            textBlock.Pivot = new Vector2(0, 0.5f);
            input.AddChild(textBlock);
            input.TextBlock = textBlock;

            return input;
        }

        public static GUIFrame CreateFieldInput(Point scale, MonoAction<GUITextInput, string> onValueChanged = null, string name = "Text", string defoultValue = " ")
        {
            GUIFrame frame = new GUIFrame();
            frame.LocalSize = scale;

            GUITextBlock text = new GUITextBlock();
            text.Text = name;
            text.AutoScale = true;
            text.Pivot = new Vector2(0, 0.5f);
            text.VerticalAlignment = GUIVerticalAlignment.Center;
            frame.AddChild(text);

            GUITextInput input = CreateTextInput(new Rectangle());
            input.OnTextChanged.AddListener(onValueChanged);
            input.SetAlignment(GUIAlignment.Stretch);
            input.LocalPosition = new Point(text.LocalSize.X, 0);
            input.Text = defoultValue;
            frame.AddChild(input);

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            image.LocalPosition = new Point(text.LocalSize.X, 0);
            frame.AddChild(image);

            return frame;
        }

        public static GUITrackBar CreateTrackBar(Rectangle rectangle)
        {
            GUITrackBar trackBar = new GUITrackBar(rectangle);

            GUIImage slidingArea = new GUIImage();
            slidingArea.SetAlignment(GUIAlignment.Stretch);
            trackBar.AddChild(slidingArea);
            trackBar.SlidingArea = slidingArea;

            GUIImage handle = new GUIImage();
            handle.LocalSize = rectangle.Size;
            handle.SetAlignment(GUIAlignment.Center);
            trackBar.AddChild(handle);
            trackBar.Handle = handle;

            trackBar.HandleSize = new Point(30, 50);

            return trackBar;    
        }

        public static GUIScrollBar CreateScrollBar(Rectangle rectangle)
        {
            GUIScrollBar scrollBar = new GUIScrollBar();

            GUIImage slidingArea = new GUIImage();
            slidingArea.SetAlignment(GUIAlignment.Stretch);
            scrollBar.AddChild(slidingArea);
            scrollBar.SlidingArea = slidingArea;

            GUIImage handle = new GUIImage();
            handle.LocalSize = rectangle.Size;
            handle.SetAlignment(GUIAlignment.Center);
            scrollBar.AddChild(handle);
            scrollBar.Handle = handle;

            scrollBar.HandleSize = new Point(30, 50);

            return scrollBar;
        }

        public static GUIScrollRect CreateScrollRect(GUIComponent content, Rectangle rectangle)
        {
            GUIScrollRect scrollRect = new GUIScrollRect(content);

            GUIScrollBar horizontalScrollBar = CreateScrollBar(new Rectangle(new Point(), new Point(0, 20)));
            horizontalScrollBar.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            horizontalScrollBar.VerticalAlignment = GUIVerticalAlignment.Top;
            horizontalScrollBar.Pivot = new Vector2(0, 1);
            horizontalScrollBar.Direction = GUISliderDirection.LeftToRight;
            scrollRect.AddChild(horizontalScrollBar);
            scrollRect.HorizontalScrollBar = horizontalScrollBar;

            GUIScrollBar verticalScrollBar = CreateScrollBar(new Rectangle(new Point(), new Point(0, 20)));
            verticalScrollBar.HorizontalAlignment = GUIHorizontalAlignment.Right;
            verticalScrollBar.VerticalAlignment = GUIVerticalAlignment.Stretch;            
            verticalScrollBar.Pivot = new Vector2(1, 0);
            verticalScrollBar.Direction = GUISliderDirection.TopToBottom;
            scrollRect.AddChild(verticalScrollBar);
            scrollRect.VerticalScrollBar = verticalScrollBar;

            return scrollRect;
        }

        public static GUITreeNode CreateTreeNode(string text = "item")
        {
            GUITreeNode node = new GUITreeNode();

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
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
            image.SetAlignment(GUIAlignment.Stretch);
            node.AddChild(image);

            return node;
        }

        public static GUINode CreateNode(Rectangle rectangle, string text = "header")
        {
            GUINode node = new GUINode();

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            textBlock.SetAlignment(GUIAlignment.Center);
            node.HeaderTextBlock = textBlock;

            GUIImage image = new GUIImage();
            image.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            image.Pivot = new Vector2(0, 0);
            node.HeaderImage = image;
            node.AddChild(image);
            image.AddChild(textBlock);

            image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            node.Image = image;
            node.AddChild(image);

            image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            node.SelectedImage = image;
            node.AddChild(image);

            return node;
        }

        public static GUIPin CreatePin(string text, GUIPinType type, GUIPinAlignment pinAlignment)
        {
            GUIPin pin = CreatePin(type, pinAlignment);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            pin.Content = textBlock;
            pin.AddChild(textBlock);

            return pin;
        }

        public static GUIPin CreatePin(GUIPinType type, GUIPinAlignment pinAlignment)
        {
            GUIPin pin = new GUIPin(type);
            pin.PinAlignment = pinAlignment;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            pin.ImageOn = image;
            pin.AddChild(image);

            image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            pin.ImageOff = image;
            pin.AddChild(image);

            return pin;
        }

        public static GUIWire CreateWire()
        {
            GUIWire wire = new GUIWire();
            wire.LocalSize = new Point(1, 1);

            return wire;
        }
    }
}