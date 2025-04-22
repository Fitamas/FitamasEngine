using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

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

        protected override void OnChildSizeChanged(GUIComponent component)
        {
            UpdateSize();
        }

        protected override void OnAddItem(GUIComponent component)
        {
            Container.AddChild(component);
        }

        protected void UpdateSize()
        {
            Thickness thickness = ContainerPadding;

            Point size0 = Point.Zero;
            if (Header != null && Header.Enable)
            {
                Header.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
                Header.Pivot = new Vector2(0, 0);
                size0 = Header.LocalSize;
            }

            Point size1 = Point.Zero;
            if (Container != null && Container.Enable)
            {
                size1 = Container.LocalSize;
                Container.LocalPosition = new Point(thickness.Left, -thickness.Top);
                Container.Pivot = new Vector2(0, 1);
                Container.VerticalAlignment = GUIVerticalAlignment.Bottom;
            }

            size1 += new Point(thickness.Left + thickness.Right, thickness.Top + thickness.Bottom);

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
