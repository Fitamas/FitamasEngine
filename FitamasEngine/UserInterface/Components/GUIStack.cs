using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public class GUIStack : GUIGroup
    {
        public static readonly DependencyProperty<int> SpacingProperty = new DependencyProperty<int>(0);

        public static readonly DependencyProperty<bool> InvertedProperty = new DependencyProperty<bool>(false);

        public static readonly DependencyProperty<bool> ControlChildSizeWidthProperty = new DependencyProperty<bool>(false);

        public static readonly DependencyProperty<bool> ControlChildSizeHeightProperty = new DependencyProperty<bool>(false);

        public int Spacing
        {
            get
            {
                return GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }

        public bool Inverted
        {
            get
            {
                return GetValue(InvertedProperty);
            }
            set
            {
                SetValue(InvertedProperty, value);
            }
        }

        public bool ControlChildSizeWidth
        {
            get
            {
                return GetValue(ControlChildSizeWidthProperty);
            }
            set
            {
                SetValue(ControlChildSizeWidthProperty, value);
            }
        }

        public bool ControlChildSizeHeight
        {
            get
            {
                return GetValue(ControlChildSizeHeightProperty);
            }
            set
            {
                SetValue(ControlChildSizeHeightProperty, value);
            }
        }

        protected override Point CalculateSize(GUIComponent[] components, Point size)
        {
            int spacing = Spacing;
            if (Orientation == GUIGroupOrientation.Horizontal)
            {
                size.X = (components.Length - 1) * spacing;
                size.Y = 0;
                foreach (var component in components)
                {
                    Point localSize = component.LocalSize;
                    size.X += localSize.X;
                    size.Y = Math.Max(localSize.Y, size.Y);
                }
            }
            else
            {
                size.X = 0;
                size.Y = (components.Length - 1) * spacing;
                foreach (var component in components)
                {
                    Point localSize = component.LocalSize;
                    size.X = Math.Max(localSize.X, size.X);
                    size.Y += localSize.Y;
                }
            }

            return size;
        }

        protected override void CalculateComponents(GUIComponent[] components, Rectangle rectangle)
        {
            if (Inverted)
            {
                components = components.Reverse().ToArray();
            }

            GUIGroupOrientation orientation = Orientation;
            bool flagWidth = ControlChildSizeWidth;
            bool flagHeight = ControlChildSizeHeight;
            int spacing = Spacing;
            Point newSize = new Point();
            if (flagWidth)
            {
                if (orientation == GUIGroupOrientation.Horizontal)
                {
                    newSize.X = (rectangle.Width - (components.Length - 1) * spacing) / components.Length;
                }
                else
                {
                    newSize.X = rectangle.Width;
                }
                
            }
            if (flagHeight)
            {
                if (orientation == GUIGroupOrientation.Vertical)
                {
                    newSize.Y = (rectangle.Height - (components.Length - 1) * spacing) / components.Length;
                }
                else
                {
                    newSize.Y = rectangle.Height;
                }
            }
            
            Point position = Point.Zero;
            foreach (GUIComponent component in components)
            {
                Point size = component.LocalSize;

                if (flagWidth)
                {
                    size.X = newSize.X;
                }
                if (flagHeight)
                {
                    size.Y = newSize.Y;
                }

                Vector2 pivot = component.Pivot;
                Point localPosition = new Point((int)(size.X * pivot.X), (int)(size.Y * pivot.Y)) + position;
                component.Margin = new Thickness(localPosition.X, localPosition.Y, size.X, size.Y);

                if (orientation == GUIGroupOrientation.Horizontal)
                {
                    position += new Point(size.X + spacing, 0);
                }
                else
                {
                    position += new Point(0, size.Y + spacing);
                }
            }
        }
    }
}
