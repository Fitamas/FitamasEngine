using Fitamas.Serializeble;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public enum GUIPinAlignment
    {
        Left,
        Right,
    }

    public enum GUIPinType
    {
        Input,
        Output
    }

    public class GUIPin : GUIComponent
    {
        [SerializableField] private GUIPinAlignment pinAlignment;
        [SerializableField] private GUIPinType type;
        [SerializableField] private GUIComponent content;

        private bool isConnected;

        public GUIImage ImageOn;
        public GUIImage ImageOff;

        public GUIPinType PinType
        {
            get
            {
                return type;
            }
        }

        public GUIPinAlignment PinAlignment
        {
            get
            {
                return pinAlignment;
            }
            set
            {
                pinAlignment = value;
                RecalculateContentAlignment();
            }
        }

        public GUIComponent Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                RecalculateContentAlignment();
            }
        }

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;

                if (ImageOn != null)
                {
                    ImageOn.Enable = value;
                }
                if (ImageOff != null)
                {
                    ImageOff.Enable = !value;
                }
            }
        }

        public Point ContentScale
        {
            get
            {
                if (Content != null)
                {
                    return Content.LocalScale;
                }

                return new Point();
            }
        }

        public GUIPin(GUIPinType type = GUIPinType.Input)
        {
            this.type = type;
        }

        private void RecalculateContentAlignment()
        {
            if (content != null)
            {
                if (pinAlignment == GUIPinAlignment.Left)
                {
                    content.HorizontalAlignment = GUIHorizontalAlignment.Right;
                    content.VerticalAlignment = GUIVerticalAlignment.Center;
                    content.Pivot = new Vector2(0, 0.5f);
                }
                else if (pinAlignment == GUIPinAlignment.Right)
                {
                    content.HorizontalAlignment = GUIHorizontalAlignment.Left;
                    content.VerticalAlignment = GUIVerticalAlignment.Center;
                    content.Pivot = new Vector2(1, 0.5f);
                }
            }
        }
    }
}
