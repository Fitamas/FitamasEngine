using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public enum GUIGroupOrientation
    {
        Horizontal,
        Vertical,
    }

    public abstract class GUIGroup : GUIComponent
    {
        public static readonly DependencyProperty<Thickness> PaddingProperty = new DependencyProperty<Thickness>(Thickness.Zero);

        public static readonly DependencyProperty<GUIGroupOrientation> OrientationProperty = new DependencyProperty<GUIGroupOrientation>(GUIGroupOrientation.Horizontal);

        public static readonly DependencyProperty<bool> ControlSizeWidthProperty = new DependencyProperty<bool>(false);

        public static readonly DependencyProperty<bool> ControlSizeHeightProperty = new DependencyProperty<bool>(false);

        public Thickness Padding
        {
            get
            {
                return GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        public GUIGroupOrientation Orientation
        {
            get
            {
                return GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        public bool ControlSizeWidth
        {
            get
            {
                return GetValue(ControlSizeWidthProperty);
            }
            set
            {
                SetValue(ControlSizeWidthProperty, value);
            }
        }

        public bool ControlSizeHeight
        {
            get
            {
                return GetValue(ControlSizeHeightProperty);
            }
            set
            {
                SetValue(ControlSizeHeightProperty, value);
            }
        }

        protected override void OnAddChild(GUIComponent component)
        {
            CalculateComponents();
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            CalculateComponents();
        }

        protected override void OnChildSizeChanged(GUIComponent component)
        {
            CalculateComponents();
        }

        private void CalculateComponents()
        {
            GUIComponent[] components = ChildrensComponent.ToArray();
            foreach (var component in components)
            {
                component.HorizontalAlignment = GUIHorizontalAlignment.Left;
                component.VerticalAlignment = GUIVerticalAlignment.Top;
            }

            if (ControlSizeWidth || ControlSizeHeight)
            {
                Point newSize = CalculateSize(components, LocalSize);
                Point size;
                size.X = ControlSizeWidth ? newSize.X : LocalSize.X;
                size.Y = ControlSizeHeight ? newSize.Y : LocalSize.Y;
                LocalSize = size;
            }

            Rectangle rectangle = Rectangle;
            Thickness padding = Padding;
            rectangle.Location += new Point(padding.Left, padding.Top);
            rectangle.Size -= new Point(padding.Right, padding.Bottom) + new Point(padding.Left, padding.Top);
            CalculateComponents(components, rectangle);
        }

        protected abstract Point CalculateSize(GUIComponent[] components, Point size);

        protected abstract void CalculateComponents(GUIComponent[] components, Rectangle rectangle);

        public Point GetGroupSize()
        {
            Rectangle rectangle = new Rectangle();
            foreach (var component in ChildrensComponent)
            {
                if (rectangle.Size == Point.Zero)
                {
                    rectangle = component.Rectangle;
                }
                else
                {
                    rectangle = Rectangle.Union(rectangle, component.Rectangle);
                }                
            }

            return rectangle.Size;
        }
    }
}
