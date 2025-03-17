using Fitamas.Graphics;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Middle;
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
            checkBox.Style = style;
            checkBox.LocalPosition = position;
            checkBox.LocalSize = size;
            checkBox.ControlTemplate = new ControlTemplate();

            GUIImage backGround = new GUIImage();
            backGround.SetAlignment(GUIAlignment.Stretch);
            checkBox.AddChild(backGround);

            GUIImage checkmark = new GUIImage();
            checkmark.Name = GUICheckBoxStyle.CheckMark;
            checkmark.SetAlignment(GUIAlignment.Stretch);
            checkBox.ControlTemplate.Add(checkmark);
            checkBox.AddChild(checkmark);

            return checkBox;
        }

        public static GUIComboBox CreateComboBox(Point position, IEnumerable<string> items = null, Point? size = null)
        {
            return CreateComboBox(ResourceDictionary.DefaultResources, position, items, size);
        }

        public static GUIComboBox CreateComboBox(ResourceDictionary dictionary, Point position, IEnumerable<string> items = null, Point? size = null)
        {
            return CreateComboBox(dictionary[CommonResourceKeys.ComboBoxStyle] as GUIStyle, position, items, size);
        }

        public static GUIComboBox CreateComboBox(GUIStyle style, Point position, IEnumerable<string> items = null, Point? size = null)
        {
            Point padding = style.Resources.FramePadding;

            GUIComboBox comboBox = new GUIComboBox();
            comboBox.Style = style;
            comboBox.LocalPosition = position;
            comboBox.SetItems(items);

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            comboBox.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.SetAlignment(GUIAlignment.Stretch);
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Left;
            textBlock.AutoScale = false;
            textBlock.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            comboBox.AddChild(textBlock);

            if (size.HasValue)
            {
                comboBox.LocalSize = size.Value;
            }
            else if (style != null)
            {
                Point localSize = FontManager.GetDefaultCharacterSize(textBlock.Font);
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
            context.Style = style;
            context.LocalPosition = position;
            context.Pivot = new Vector2(0, 0);
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
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Left;
            item.AddChild(textBlock);
            
            item.LocalSize = padding + padding + textBlock.Font.MeasureString(text).ToPoint();

            return item;
        }

        public static GUITextInput CreateTextInput(Point position, int lenght = 0)
        {
            return CreateTextInput(ResourceDictionary.DefaultResources, position, lenght);
        }

        public static GUITextInput CreateTextInput(ResourceDictionary dictionary, Point position, int lenght = 0)
        {
            return CreateTextInput(dictionary[CommonResourceKeys.TextInputStyle] as GUIStyle, position, lenght);
        }

        public static GUITextInput CreateTextInput(GUIStyle style, Point position, int lenght = 0)
        {
            Point padding = style.Resources.FramePadding;

            GUITextInput input = new GUITextInput();
            input.LocalPosition = position;
            input.LocalSize = new Point(lenght, FontManager.GetHeight()) + padding + padding;
            input.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            input.AddChild(image);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            textBlock.SetAlignment(GUIAlignment.Stretch);
            input.AddChild(textBlock);
            input.TextBlock = textBlock;

            textBlock = new GUITextBlock();
            textBlock.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            textBlock.SetAlignment(GUIAlignment.Stretch);
            //input.AddChild(textBlock);
            //input.TextBlock = textBlock;

            return input;
        }

        public static GUISlider CreateSlider(Point position, GUISliderDirection direction = GUISliderDirection.LeftToRight, int lenght = 0)
        {
            return CreateSlider(ResourceDictionary.DefaultResources, position, direction, lenght);
        }

        public static GUISlider CreateSlider(ResourceDictionary dictionary, Point position, GUISliderDirection direction = GUISliderDirection.LeftToRight, int lenght = 0)
        {
            return CreateSlider(dictionary[CommonResourceKeys.TrackBarStyle] as GUIStyle, dictionary[CommonResourceKeys.TrackBarThumbStyle] as GUIStyle, position, direction, lenght);
        }

        public static GUISlider CreateSlider(GUIStyle sliderStyle, GUIStyle thumbStyle, Point position, GUISliderDirection direction = GUISliderDirection.LeftToRight, int lenght = 0)
        {
            Point padding = sliderStyle.Resources.FramePadding;
            Point size;
            int size1 = FontManager.GetDefaultCharacterSize().Y + padding.Y * 2;

            if (direction == GUISliderDirection.LeftToRight || direction == GUISliderDirection.RightToLeft)
            {
                size = new Point(lenght + padding.X * 2, size1);
            }
            else
            {
                size = new Point(size1, lenght + padding.X * 2);
            }

            GUISlider slider = new GUISlider();
            slider.Style = sliderStyle;
            slider.LocalPosition = position;
            slider.LocalSize = size;
            slider.ControlTemplate = new ControlTemplate();

            GUIImage slidingArea = new GUIImage();
            slidingArea.SetAlignment(GUIAlignment.Stretch);
            slidingArea.Name = GUITrackBarStyle.SlidingArea;
            slider.ControlTemplate.Add(slidingArea);
            slider.AddChild(slidingArea);

            GUITrack track = new GUITrack();
            track.Margin = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            track.SetAlignment(GUIAlignment.Stretch);

            GUIThumb thumb = new GUIThumb();
            thumb.Margin = new Thickness(-padding.X, -padding.Y, -padding.X, -padding.Y);
            thumb.SetAlignment(GUIAlignment.Stretch);
            thumb.Style = thumbStyle;
            track.Direction = direction;
            track.AddChild(thumb);
            track.Thumb = thumb;
            slider.AddChild(track);
            slider.Track = track;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            thumb.AddChild(image);

            return slider;
        }

        public static GUIScrollRect CreateScrollRect(Point position, Point size)
        {
            return CreateScrollRect(ResourceDictionary.DefaultResources, position, size);
        }

        public static GUIScrollRect CreateScrollRect(ResourceDictionary dictionary, Point position, Point size)
        {
            return CreateScrollRect(dictionary[CommonResourceKeys.TrackBarStyle] as GUIStyle, dictionary[CommonResourceKeys.TrackBarThumbStyle] as GUIStyle, position, size);
        }

        public static GUIScrollRect CreateScrollRect(GUIStyle sliderStyle, GUIStyle thumbStyle, Point position, Point size)
        {
            Point padding = sliderStyle.Resources.FramePadding;
            int size1 = FontManager.GetDefaultCharacterSize().Y + padding.Y * 2;

            GUIScrollRect scrollRect = new GUIScrollRect();
            scrollRect.LocalPosition = position;
            scrollRect.LocalSize = size;

            GUISlider horizontalScrollBar = CreateSlider(sliderStyle, thumbStyle, new Point(), GUISliderDirection.LeftToRight);
            horizontalScrollBar.Margin = new Thickness(0, 0, size1, size1);
            horizontalScrollBar.Pivot = new Vector2(0, 1);
            horizontalScrollBar.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            horizontalScrollBar.VerticalAlignment = GUIVerticalAlignment.Bottom;
            scrollRect.AddChild(horizontalScrollBar);
            scrollRect.HorizontalSlider = horizontalScrollBar;

            GUISlider verticalScrollBar = CreateSlider(sliderStyle, thumbStyle, new Point(), GUISliderDirection.BottomToTop);
            verticalScrollBar.Margin = new Thickness(0, 0, size1, size1);
            verticalScrollBar.Pivot = new Vector2(1, 0);
            verticalScrollBar.HorizontalAlignment = GUIHorizontalAlignment.Right;
            verticalScrollBar.VerticalAlignment = GUIVerticalAlignment.Stretch;            
            scrollRect.AddChild(verticalScrollBar);
            scrollRect.VerticalSlider = verticalScrollBar;

            GUIFrame viewport = new GUIFrame();
            viewport.SetAlignment(GUIAlignment.Stretch);
            viewport.IsMask = true;
            viewport.Margin = new Thickness(0, 0, verticalScrollBar.Rectangle.Width, horizontalScrollBar.Rectangle.Height);
            scrollRect.AddChild(viewport);
            scrollRect.Viewport = viewport;
            viewport.SetAsLastSibling();

            return scrollRect;
        }

        public static GUITreeView CreateTreeView(Point position, Point size)
        {
            return CreateTreeView(ResourceDictionary.DefaultResources, position, size);
        }

        public static GUITreeView CreateTreeView(ResourceDictionary dictionary, Point position, Point size)
        {
            return CreateTreeView(dictionary[CommonResourceKeys.TreeViewStyle] as GUIStyle, position, size);
        }

        public static GUITreeView CreateTreeView(GUIStyle style, Point position, Point size)
        {
            GUITreeView tree = new GUITreeView();
            tree.LocalPosition = position;
            tree.LocalSize = size;
            tree.Style = style;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            tree.AddChild(image);

            GUIStack stack = new GUIStack();
            stack.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            stack.Orientation = GUIGroupOrientation.Vertical;
            stack.ControlChildSizeWidth = true;
            tree.AddChild(stack);
            tree.Container = stack;

            return tree;
        }

        public static GUITreeNode CreateTreeNode(string text)
        {
            return CreateTreeNode(ResourceDictionary.DefaultResources, text);
        }

        public static GUITreeNode CreateTreeNode(ResourceDictionary dictionary, string text)
        {
            return CreateTreeNode(dictionary[CommonResourceKeys.TreeNodeStyle] as GUIStyle, text);
        }

        public static GUITreeNode CreateTreeNode(GUIStyle style, string text)
        {
            Point padding = style.Resources.FramePadding;
            int height = FontManager.GetHeight() + padding.Y * 2;

            GUITreeNode node = new GUITreeNode();
            node.Margin = new Thickness(0, 0, 20, height);
            node.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            node.Pivot = new Vector2(0, 0);
            node.Style = style;
            node.ControlTemplate = new ControlTemplate();

            GUIStack stack = new GUIStack();
            stack.Name = GUITreeStyle.Container;
            stack.LocalPosition = new Point(0, height);
            stack.Pivot = new Vector2(0, 0);
            stack.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            stack.Orientation = GUIGroupOrientation.Vertical;
            stack.ControlChildSizeWidth = true;
            stack.ControlSizeHeight = true;
            node.ControlTemplate.Add(stack);
            node.AddChild(stack);
            node.Container = stack;

            GUIButton button = new GUIButton();
            button.Margin = new Thickness(0, 0, 0, height);
            button.Pivot = new Vector2(0, 0);
            button.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            button.OnClicked.AddListener((b, a) => 
            { 
                node.IsOpen = !node.IsOpen;
                node.TreeView.Select(node, a);
            });
            node.AddChild(button);
            node.Header = button;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            button.AddChild(image);

            Point size = new Point(FontManager.GetHeight());
            Point position = padding;

            GUIImage image0 = new GUIImage();
            image0.Name = GUITreeStyle.ArrowIcon;
            image0.LocalPosition = position;
            image0.LocalSize = size;
            image0.Pivot = new Vector2(0, 0);
            node.ControlTemplate.Add(image0);
            node.AddChild(image0);

            position.X += size.X + padding.X;

            GUIImage image1 = new GUIImage();
            image1.Name = GUITreeStyle.Icon;
            image1.LocalPosition = position;
            image1.LocalSize = size;
            image1.Pivot = new Vector2(0, 0);
            node.ControlTemplate.Add(image1);
            node.AddChild(image1);

            position.X += size.X + padding.X;

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Margin = new Thickness(position.X, padding.Y, padding.X, padding.Y);
            textBlock.SetAlignment(GUIAlignment.Stretch);
            textBlock.Text = text;
            button.AddChild(textBlock);

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