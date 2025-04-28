using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public enum GUINodeItemAlignment
    {
        Left,
        Right,
    }

    public class GUINodeItem : GUIComponent
    {
        public static readonly DependencyProperty<GUINodeItemAlignment> AlignmentProperty = new DependencyProperty<GUINodeItemAlignment>(AlignmentChangedCallback, GUINodeItemAlignment.Left, false);

        private GUINode node;
        private GUIPin pin;
        private GUIComponent content;

        public GUINodeItemAlignment Alignment
        {
            get
            {
                return GetValue(AlignmentProperty);
            }
            set
            {
                SetValue(AlignmentProperty, value);
            }
        }

        public GUINode Node
        {
            get
            {
                return node;
            }
            set
            {
                node = value;
                if (pin != null)
                {
                    pin.Node = value;
                }
            }
        }

        public GUIPin Pin 
        { 
            get
            {
                return pin;
            }
            set
            {
                pin = value;
                pin.Node = Node;
                RecalculateContent();
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

        public GUINodeItem()
        {

        }

        protected override void OnAddChild(GUIComponent component)
        {
            CalculateSize();
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            CalculateSize();
        }

        protected override void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {
            base.OnChildPropertyChanged(component, property);

            if (property == MarginProperty)
            {
                CalculateSize();
            }
        }

        private void RecalculateContent()
        {
            if (Pin != null)
            {
                if (Alignment == GUINodeItemAlignment.Left)
                {
                    Pin.HorizontalAlignment = GUIHorizontalAlignment.Left;
                    Pin.VerticalAlignment = GUIVerticalAlignment.Center;
                    Pin.Pivot = new Vector2(0, 0.5f);
                }
                else
                {
                    Pin.HorizontalAlignment = GUIHorizontalAlignment.Right;
                    Pin.VerticalAlignment = GUIVerticalAlignment.Center;
                    Pin.Pivot = new Vector2(1, 0.5f);
                }
            }


            if (Content != null)
            {
                if (Alignment == GUINodeItemAlignment.Left)
                {
                    Content.HorizontalAlignment = GUIHorizontalAlignment.Right;
                    Content.VerticalAlignment = GUIVerticalAlignment.Center;
                    Content.Pivot = new Vector2(1, 0.5f);
                }
                else
                {
                    Content.HorizontalAlignment = GUIHorizontalAlignment.Left;
                    Content.VerticalAlignment = GUIVerticalAlignment.Center;
                    Content.Pivot = new Vector2(0, 0.5f);
                }
            }
        }

        private void CalculateSize()
        {
            Point size0 = Pin != null ? Pin.LocalSize : new Point();
            Point size1 = Content != null ? Content.LocalSize : new Point();
            LocalSize = new Point(size0.X + size1.X, Math.Max(size0.Y, size1.Y));
        }

        private static void AlignmentChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUINodeItemAlignment> property, GUINodeItemAlignment oldValue, GUINodeItemAlignment newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUINodeItem nodeItem)
                {
                    nodeItem?.RecalculateContent();
                }
            }
        }
    }
}
