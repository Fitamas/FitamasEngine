using Assimp;
using Fitamas.Input;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public static class GUINodeUtils
    {
        public static GUINodeEditor CreateNodeEditor()
        {
            return CreateNodeEditor(ResourceDictionary.DefaultResources);
        }

        public static GUINodeEditor CreateNodeEditor(ResourceDictionary dictionary)
        {
            return CreateNodeEditor(dictionary[CommonResourceKeys.NodeEditorStyle] as GUIStyle);
        }

        public static GUINodeEditor CreateNodeEditor(GUIStyle style)
        {
            GUINodeEditor editor = new GUINodeEditor();
            editor.ControlTemplate = new GUIControlTemplate();

            GUIImage image = new GUIImage();
            editor.AddChild(image);
            editor.SelectRegion = image;
            editor.ControlTemplate.Add(image, GUINodeEditorStyle.SelectRegion);

            editor.Style = style;
            return editor;
        }

        public static GUINode CreateNode(Point position, string name)
        {
            return CreateNode(ResourceDictionary.DefaultResources, position, name);
        }

        public static GUINode CreateNode(ResourceDictionary dictionary, Point position, string name)
        {
            return CreateNode(dictionary[CommonResourceKeys.NodeEditorNodeStyle] as GUIStyle, position, name);
        }

        public static GUINode CreateNode(GUIStyle style, Point position, string name)
        {
            Point padding = style.Resources.FramePadding;
            int height = FontManager.GetHeight() + padding.Y * 2;

            GUINode node = new GUINode();
            node.LocalPosition = position;
            node.ContainerPadding = new Thickness(padding.X, padding.Y, padding.X, padding.Y);
            node.ControlTemplate = new GUIControlTemplate();

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            image.Margin = new Thickness(-padding.X, -padding.Y, -padding.X, -padding.Y);
            node.AddChild(image);
            node.ControlTemplate.Add(image, GUINodeEditorStyle.NodeSelect);

            GUIImage image0 = new GUIImage();
            image0.SetAlignment(GUIAlignment.Stretch);
            node.AddChild(image0);
            node.ControlTemplate.Add(image0, GUINodeEditorStyle.NodeBody);

            GUIStack stack = new GUIStack();
            stack.Orientation = GUIGroupOrientation.Horizontal;
            stack.ControlSizeWidth = true;
            stack.ControlSizeHeight = true;
            stack.Spacing = padding.X;
            stack.LocalPosition = new Point(0, height);
            stack.Pivot = new Vector2(0, 0);
            node.Container = stack;
            node.AddChild(stack);

            GUIStack leftStack = new GUIStack();
            leftStack.Orientation = GUIGroupOrientation.Vertical;
            leftStack.ControlSizeWidth = true;
            leftStack.ControlSizeHeight = true;
            stack.AddChild(leftStack);
            node.LeftStack = leftStack;

            GUIStack rightStack = new GUIStack();
            rightStack.Orientation = GUIGroupOrientation.Vertical;
            rightStack.ControlSizeWidth = true;
            rightStack.ControlSizeHeight = true;
            stack.AddChild(rightStack);
            node.RightStack = rightStack;

            GUIFrame header = new GUIFrame();
            header.Margin = new Thickness(0, 0, 0, height);
            header.Pivot = new Vector2(0, 0);
            header.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            node.AddChild(header);
            node.Header = header;
            node.ControlTemplate.Add(header, GUINodeEditorStyle.NodeHead);

            GUIImage image1 = new GUIImage();
            image1.SetAlignment(GUIAlignment.Stretch);
            header.AddChild(image1);

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = name;
            textBlock.TextHorisontalAlignment = GUITextHorisontalAlignment.Middle;
            textBlock.SetAlignment(GUIAlignment.Center);
            header.AddChild(textBlock);

            node.Style = style;
            return node;
        }

        public static GUINodeItem CreateNodeItem(string text, GUINodeItemAlignment alignment, GUIPinType type)
        {
            return CreateNodeItem(ResourceDictionary.DefaultResources, text, alignment, type);
        }

        public static GUINodeItem CreateNodeItem(ResourceDictionary dictionary, string text, GUINodeItemAlignment alignment, GUIPinType type)
        {
            return CreateNodeItem(dictionary[CommonResourceKeys.NodeEditorPinStyle] as GUIStyle, text, alignment, type);
        }

        public static GUINodeItem CreateNodeItem(GUIStyle pinStyle, string text, GUINodeItemAlignment alignment, GUIPinType type)
        {
            GUINodeItem nodeItem = new GUINodeItem();
            nodeItem.Alignment = alignment;

            GUIPin pin = CreatePin(pinStyle, type);
            nodeItem.AddChild(pin);
            nodeItem.Pin = pin;

            GUITextBlock textBlock = new GUITextBlock();
            textBlock.Text = text;
            nodeItem.Content = textBlock;
            nodeItem.AddChild(textBlock);

            return nodeItem;
        }

        public static GUIPin CreatePin(GUIPinType type)
        {
            return CreatePin(ResourceDictionary.DefaultResources, type);
        }

        public static GUIPin CreatePin(ResourceDictionary dictionary, GUIPinType type)
        {
            return CreatePin(dictionary[CommonResourceKeys.NodeEditorPinStyle] as GUIStyle, type);
        }

        public static GUIPin CreatePin(GUIStyle style, GUIPinType type)
        {
            Point size = new Point(FontManager.GetHeight());

            GUIPin pin = new GUIPin();
            pin.LocalSize = size;
            pin.PinType = type;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            pin.AddChild(image);

            image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            pin.AddChild(image);

            pin.Style = style;
            return pin;
        }

        public static GUIWire CreateWire()
        {
            return CreateWire(ResourceDictionary.DefaultResources);
        }

        public static GUIWire CreateWire(ResourceDictionary dictionary)
        {
            return CreateWire(dictionary[CommonResourceKeys.NodeEditorWireStyle] as GUIStyle);
        }

        public static GUIWire CreateWire(GUIStyle style)
        {
            GUIWire wire = new GUIWire();

            wire.Style = style;
            return wire;
        }
    }
}
