using Fitamas.Serialization;
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
        public static readonly DependencyProperty<GUIPinType> PinTypeProperty = new DependencyProperty<GUIPinType>(GUIPinType.Input, false);

        public static readonly DependencyProperty<GUIPinAlignment> PinAlignmentProperty = new DependencyProperty<GUIPinAlignment>(GUIPinAlignment.Left, false);

        public static readonly DependencyProperty<bool> IsConnectedProperty = new DependencyProperty<bool>(false, false);

        private GUIComponent content;

        public GUIPinType PinType
        {
            get
            {
                return GetValue(PinTypeProperty);
            }
            set
            {
                SetValue(PinTypeProperty, value);
            }
        }

        public GUIPinAlignment PinAlignment
        {
            get
            {
                return GetValue(PinAlignmentProperty);
            }
            set
            {
                SetValue(PinAlignmentProperty, value);
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
                RecalculateContent();
            }
        }

        public bool IsConnected
        {
            get
            {
                return GetValue(IsConnectedProperty);
            }
            set
            {
                SetValue(IsConnectedProperty, value);
            }
        }

        public Point ContentScale
        {
            get
            {
                if (Content != null)
                {
                    return Content.LocalSize;
                }

                return new Point();
            }
        }

        public GUIPin()
        {

        }

        private void RecalculateContent()
        {
            if (content == null)
            {
                return;
            }

            if (PinAlignment == GUIPinAlignment.Left)
            {
                content.HorizontalAlignment = GUIHorizontalAlignment.Right;
                content.VerticalAlignment = GUIVerticalAlignment.Center;
                content.Pivot = new Vector2(0, 0.5f);
            }
            else
            {
                content.HorizontalAlignment = GUIHorizontalAlignment.Left;
                content.VerticalAlignment = GUIVerticalAlignment.Center;
                content.Pivot = new Vector2(1, 0.5f);
            }
        }
    }
}
