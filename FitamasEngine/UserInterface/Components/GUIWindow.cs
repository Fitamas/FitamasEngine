using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public enum SizeToContent
    {
        Manual,
        Width,
        Height,
        WidthAndHeight
    }

    public class GUIWindow : GUIComponent
    {
        public static readonly DependencyProperty<Thickness> PaddingProperty = new DependencyProperty<Thickness>(Thickness.Zero, false);

        public static readonly DependencyProperty<SizeToContent> SizeToContentProperty = new DependencyProperty<SizeToContent>(SizeToContent.Manual, false);

        public static readonly DependencyProperty<bool> IsActiveProperty = new DependencyProperty<bool>(true, false);

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

        public SizeToContent SizeToContent
        {
            get
            {
                return GetValue(SizeToContentProperty);
            }
            set
            {
                SetValue(SizeToContentProperty, value);
            }
        }

        public bool IsActive
        {
            get
            {
                return GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public GUIComponent Content { get; set; }

        public GUIWindow()
        {
            IsMask = true;
            RaycastTarget = true;
        }

        protected override void OnChildSizeChanged(GUIComponent component)
        {
            if (component == Content)
            {
                Thickness padding = Padding;

                switch (SizeToContent)
                {
                    case SizeToContent.Manual:
                        break;
                    case SizeToContent.Width:
                        LocalSize = new Point(component.LocalSize.X + padding.Left + padding.Right, LocalSize.Y);
                        break;
                    case SizeToContent.Height:
                        LocalSize = new Point(LocalSize.X, component.LocalSize.Y + padding.Top + padding.Bottom);
                        break;
                    case SizeToContent.WidthAndHeight:
                        LocalSize = component.LocalSize + new Point(padding.Left + padding.Right, padding.Top + padding.Bottom);
                        break;
                }                
            }
        }

        public virtual void Open()
        {
            IsActive = true;
        }

        public virtual void Close()
        {
            IsActive = false;
            Destroy();
        }
    }
}
