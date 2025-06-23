using Fitamas.Events;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public enum GUISizeToContent
    {
        Manual,
        Width,
        Height,
        WidthAndHeight
    }

    public class GUIWindow : GUIComponent, IMouseEvent
    {
        internal struct WindowMinMax
        {
            internal int minWidth;

            internal int maxWidth;

            internal int minHeight;

            internal int maxHeight;

            internal WindowMinMax(int minSize, int maxSize)
            {
                minWidth = minSize;
                maxWidth = maxSize;
                minHeight = minSize;
                maxHeight = maxSize;
            }
        }

        public static readonly DependencyProperty<Thickness> PaddingProperty = new DependencyProperty<Thickness>(PaddingChangedCallback, Thickness.Zero, false);

        public static readonly DependencyProperty<GUISizeToContent> SizeToContentProperty = new DependencyProperty<GUISizeToContent>(SizeToContentChangedCallback, GUISizeToContent.Manual, false);

        public static readonly DependencyProperty<bool> IsOnTopProperty = new DependencyProperty<bool>(false, false);

        public static readonly DependencyProperty<bool> DestroyOnCloseProperty = new DependencyProperty<bool>(true, false);


        public static readonly RoutedEvent OnCloseEvent = new RoutedEvent();

        private GUIComponent content;

        public MonoEvent<GUIWindow> OnClose { get; }

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

        public GUISizeToContent SizeToContent
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

        public bool IsOnTop
        {
            get
            {
                return (bool)GetValue(IsOnTopProperty);
            }
            set
            {
                SetValue(IsOnTopProperty, value);
            }
        }

        public bool DestroyOnClose
        {
            get
            {
                return GetValue(DestroyOnCloseProperty);
            }
            set
            {
                SetValue(DestroyOnCloseProperty, value);
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
                Recalculate();
            }
        }

        public GUIWindow()
        {
            RaycastTarget = true;
            IsFocusScope = true;

            OnClose = new MonoEvent<GUIWindow>();
        }

        protected override void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {
            base.OnChildPropertyChanged(component, property);

            if (property == MarginProperty)
            {
                Recalculate();
            }
        }

        protected override void OnFocus()
        {
            SetAsFirstSibling();
        }

        protected override Rectangle AvailableRectangle(GUIComponent component)
        {
            Rectangle rectangle = base.AvailableRectangle(component);
            if (component == Content)
            {
                Thickness padding = Padding;
                rectangle.Location += new Point(padding.Left, padding.Top);
                rectangle.Size -= new Point(padding.Right, padding.Bottom) + new Point(padding.Left, padding.Top);
            }
            return rectangle;
        }

        public void Close()
        {
            if (IsInHierarchy)
            {
                System.Root.CloseWindow(this);
                if (DestroyOnClose)
                {
                    Destroy();
                }
                OnCloseWindow();
                OnClose.Invoke(this);
                RaiseEvent(new GUIEventArgs(OnCloseEvent, this));
            }
        }

        protected virtual void OnCloseWindow()
        {

        }

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {
            if (!IsFocused)
            {
                Focus();
            }
        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
        {

        }

        private void Recalculate()
        {
            if (Content == null)
            {
                return;
            }

            Thickness padding = Padding;
            Content.Pivot = Vector2.Zero;

            switch (SizeToContent)
            {
                case GUISizeToContent.Manual:
                    break;
                case GUISizeToContent.Width:
                    LocalSize = new Point(Content.LocalSize.X + padding.Left + padding.Right, LocalSize.Y);
                    Content.HorizontalAlignment = GUIHorizontalAlignment.Left;
                    break;
                case GUISizeToContent.Height:
                    LocalSize = new Point(LocalSize.X, Content.LocalSize.Y + padding.Top + padding.Bottom);
                    Content.VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
                case GUISizeToContent.WidthAndHeight:
                    LocalSize = Content.LocalSize + new Point(padding.Left + padding.Right, padding.Top + padding.Bottom);
                    Content.HorizontalAlignment = GUIHorizontalAlignment.Left;
                    Content.VerticalAlignment = GUIVerticalAlignment.Top;
                    break;
            }
        }

        private static void PaddingChangedCallback(DependencyObject dependencyObject, DependencyProperty<Thickness> property, Thickness oldValue, Thickness newValue)
        {
            if (dependencyObject is GUIWindow window)
            {
                window.Recalculate();
            }
        }

        private static void SizeToContentChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUISizeToContent> property, GUISizeToContent oldValue, GUISizeToContent newValue)
        {
            if (dependencyObject is GUIWindow window)
            {
                window.Recalculate();
            }
        }
    }
}
