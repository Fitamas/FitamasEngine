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
        public static GUIImage CreateImage(Sprite sprite, Rectangle rectangle)
        {
            GUIImage image = new GUIImage();
            image.LocalRectangle = rectangle;
            image.Sprite = sprite;
            return image;
        }

        public static GUIButton CreateButton(Rectangle rectangle, string text)
        {
            return CreateButton(ResourceDictionary.DefaultResources, rectangle, text);
        }

        public static GUIButton CreateButton(ResourceDictionary dictionary, Rectangle rectangle, string text)
        {
            if (dictionary.TryGetValue(CommonResourceKeys.ButtonStyle, out object res) && res is GUIStyle style)
            {
                return CreateButton(style, rectangle, text);
            }

            return null;
        }

        public static GUIButton CreateButton(GUIStyle style, Rectangle rectangle, string text)
        {
            GUIButton button = new GUIButton();
            button.LocalRectangle = rectangle;
            button.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            button.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            textBlock.TextAligment = GUITextAligment.Middle;
            textBlock.SetAlignment(GUIAlignment.Center);
            button.AddChild(textBlock);

            return button;
        }

        public static GUITextBlock CreateTextBlock(string text)
        {
            return CreateTextBlock(ResourceDictionary.DefaultResources, text);
        }

        public static GUITextBlock CreateTextBlock(ResourceDictionary dictionary, string text)
        {
            if (dictionary.TryGetValue(CommonResourceKeys.TextBlockStyle, out object res) && res is GUIStyle style)
            {
                return CreateTextBlock(style, text);
            }

            return null;
        }

        public static GUITextBlock CreateTextBlock(GUIStyle style, string text)
        {
            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Style = style;
            textBlock.Text = text;

            return textBlock;
        }

        public static GUICheckBox CreateCheckBox(Rectangle rectangle)
        {
            return CreateCheckBox(ResourceDictionary.DefaultResources, rectangle);
        }

        public static GUICheckBox CreateCheckBox(ResourceDictionary dictionary, Rectangle rectangle)
        {
            if (dictionary.TryGetValue(CommonResourceKeys.CheckBoxStyle, out object res) && res is GUIStyle style)
            {
                return CreateCheckBox(style, rectangle);
            }

            return null;
        }

        public static GUICheckBox CreateCheckBox(GUIStyle style, Rectangle rectangle)
        {
            GUICheckBox checkBox = new GUICheckBox();
            checkBox.LocalRectangle = rectangle;

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

        public static GUIComboBox CreateComboBox(Rectangle rectangle, IEnumerable<string> items)
        {
            GUIComboBox comboBox = new GUIComboBox(rectangle, items);

            GUIVerticalGroup group = new GUIVerticalGroup();
            group.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            group.VerticalAlignment = GUIVerticalAlignment.Bottom;
            group.Pivot = new Vector2();
            group.CellSize = new Point(0, rectangle.Height);
            comboBox.AddChild(group);
            comboBox.Group = group;

            return comboBox;
        }

        public static GUIContextMenu CreateContextMenu(Rectangle rectangle)
        {
            GUIContextMenu context = new GUIContextMenu();
            context.LocalRectangle = rectangle;
            //context.Group.CellSize = GUIStyle.ContextItemSize;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            context.AddChild(image);

            return context;
        }

        public static GUIContextItem CreateContextItem(string text)
        {
            GUIContextItem item = new GUIContextItem();
            item.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            //button.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            item.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            textBlock.TextAligment = GUITextAligment.Middle;
            textBlock.SetAlignment(GUIAlignment.Center);
            item.AddChild(textBlock);
            return item;
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

        public static GUITextInput CreateTextInput(Rectangle rectangle)
        {
            GUITextInput input = new GUITextInput(rectangle);
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
            frame.LocalScale = scale;

            GUITextBlock text = new GUITextBlock();
            text.Text = name;
            text.AutoScale = true;
            text.Pivot = new Vector2(0, 0.5f);
            text.VerticalAlignment = GUIVerticalAlignment.Center;
            frame.AddChild(text);

            GUITextInput input = CreateTextInput(new Rectangle());
            input.OnTextChanged.AddListener(onValueChanged);
            input.SetAlignment(GUIAlignment.Stretch);
            input.LocalPosition = new Point(text.LocalScale.X, 0);
            input.Text = defoultValue;
            frame.AddChild(input);

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            image.LocalPosition = new Point(text.LocalScale.X, 0);
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
            handle.LocalScale = rectangle.Size;
            handle.SetAlignment(GUIAlignment.Center);
            trackBar.AddChild(handle);
            trackBar.Handle = handle;

            trackBar.HandleSize = new Point(30, 50);

            return trackBar;    
        }

        public static GUIScrollBar CreateScrollBar(Rectangle rectangle)
        {
            GUIScrollBar scrollBar = new GUIScrollBar(rectangle);

            GUIImage slidingArea = new GUIImage();
            slidingArea.SetAlignment(GUIAlignment.Stretch);
            scrollBar.AddChild(slidingArea);
            scrollBar.SlidingArea = slidingArea;

            GUIImage handle = new GUIImage();
            handle.LocalScale = rectangle.Size;
            handle.SetAlignment(GUIAlignment.Center);
            scrollBar.AddChild(handle);
            scrollBar.Handle = handle;

            scrollBar.HandleSize = new Point(30, 50);

            return scrollBar;
        }

        public static GUIScrollRect CreateScrollRect(GUIComponent content, Rectangle rectangle)
        {
            GUIScrollRect scrollRect = new GUIScrollRect(content, rectangle);

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
            GUINode node = new GUINode(rectangle);

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
            wire.LocalScale = new Point(1, 1);

            return wire;
        }
    }
}
