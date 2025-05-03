using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
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

        public static readonly DependencyProperty<GUIAlignment> ChildAlignmentProperty = new DependencyProperty<GUIAlignment>(GUIAlignment.LeftTop);

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

        public GUIAlignment ChildAlignment
        {
            get
            {
                return GetValue(ChildAlignmentProperty);
            }
            set
            {
                SetValue(ChildAlignmentProperty, value);
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
            CalculateComponentsAndSize();
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            CalculateComponentsAndSize();
        }

        protected override void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {
            base.OnChildPropertyChanged(component, property);

            if (property == EnableProperty || property == MarginProperty)
            {
                CalculateComponentsAndSize();
            }
        }

        public override void OnPropertyChanged(DependencyProperty property)
        {
            base.OnPropertyChanged(property);

            if (property == MarginProperty)
            {
                CalculateComponentsAndSize();
            }
        }

        protected override Rectangle AvailableRectangle(GUIComponent component)
        {
            Rectangle rectangle = base.AvailableRectangle(component);
            Thickness padding = Padding;
            rectangle.Location += new Point(padding.Left, padding.Top);
            rectangle.Size -= new Point(padding.Right, padding.Bottom) + new Point(padding.Left, padding.Top);
            return rectangle;
        }

        protected override void OnRecalculateRectangle(Rectangle rectangle)
        {
            CalculateComponents();
        }

        private GUIComponent[] GetComponents()
        {
            GUIComponent[] components = ChildrensComponent.Where(e => e.Enable).ToArray();
            foreach (var component in components)
            {
                component.SetAlignment(ChildAlignment);
            }

            return components;
        }

        private void CalculateComponentsAndSize()
        {
            GUIComponent[] components = GetComponents();

            if (ControlSizeWidth || ControlSizeHeight)
            {
                Point newSize = CalculateSize(components, LocalSize);
                Point size;
                size.X = ControlSizeWidth ? newSize.X : LocalSize.X;
                size.Y = ControlSizeHeight ? newSize.Y : LocalSize.Y;
                LocalSize = size;
            }

            CalculateComponents(components);
        }

        private void CalculateComponents()
        {
            CalculateComponents(GetComponents());
        }

        protected abstract Point CalculateSize(GUIComponent[] components, Point size);

        protected abstract void CalculateComponents(GUIComponent[] components);
    }
}