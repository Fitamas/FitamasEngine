using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUIHeaderedItemsControl : GUIItemsControl
    {
        public static readonly DependencyProperty<Thickness> ContainerPaddingProperty = new DependencyProperty<Thickness>(ContainerPaddingChangedCallback, Thickness.Zero, false);

        public GUIComponent Header { get; set; }
        public GUIComponent Container { get; set; }

        public Thickness ContainerPadding
        {
            get
            {
                return GetValue(ContainerPaddingProperty);
            }
            set
            {
                SetValue(ContainerPaddingProperty, value);
            }
        }

        protected override void OnAddChild(GUIComponent component)
        {
            UpdateSize();
        }

        protected override void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {
            base.OnChildPropertyChanged(component, property);

            if (property == MarginProperty || property == EnableProperty)
            {
                UpdateSize();
            }
        }

        protected override void OnAddItem(GUIComponent component)
        {
            Container.AddChild(component);
        }

        protected override Rectangle AvailableRectangle(GUIComponent component)
        {
            Rectangle rectangle = base.AvailableRectangle(component);
            if (component == Container)
            {
                Thickness padding = ContainerPadding;
                rectangle.Location += new Point(padding.Left, padding.Top);
                rectangle.Size -= new Point(padding.Left + padding.Right, padding.Top + padding.Bottom);
            }
            return rectangle;
        }

        protected void UpdateSize()
        {
            Point size0 = Point.Zero;
            if (Header != null && Header.Enable)
            {
                size0 = Header.LocalSize;
            }

            Point size1 = Point.Zero;
            if (Container != null && Container.Enable)
            {
                size1 = Container.LocalSize;
                Thickness thickness = ContainerPadding;
                size1 += new Point(thickness.Left + thickness.Right, thickness.Top + thickness.Bottom);
            }

            LocalSize = new Point(Math.Max(size0.X, size1.X), size0.Y + size1.Y);
        }

        private static void ContainerPaddingChangedCallback(DependencyObject dependencyObject, DependencyProperty<Thickness> property, Thickness oldValue, Thickness newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUIHeaderedItemsControl control)
                {
                    control.UpdateSize();
                }
            }
        }
    }
}
