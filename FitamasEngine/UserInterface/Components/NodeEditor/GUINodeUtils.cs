using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public static class GUINodeUtils
    {
        public static GUINode CreateNode(Point position, string name)
        {
            GUINode node = new GUINode();
            node.LocalPosition = position;

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
            GUIPin pin = new GUIPin();
            pin.PinType = type;
            pin.PinAlignment = pinAlignment;

            GUIImage image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            //pin.ImageOn = image;
            pin.AddChild(image);

            image = new GUIImage();
            image.SetAlignment(GUIAlignment.Stretch);
            //pin.ImageOff = image;
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
