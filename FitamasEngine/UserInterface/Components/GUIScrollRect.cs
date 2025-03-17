using Fitamas.Input.InputListeners;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;

namespace Fitamas.UserInterface.Components
{
    public class GUIScrollRect : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<Vector2> ValueProperty = new DependencyProperty<Vector2>(ValueChangedCallback, new Vector2(0, 1), false);

        public static readonly RoutedEvent OnValueChangedEvent = new RoutedEvent();

        private GUIComponent content;
        private GUIComponent viewport;

        public GUISlider HorizontalSlider { get; set; } 
        public GUISlider VerticalSlider { get; set; }
        public float Sensitivity { get; set; }

        public GUIEvent<GUIScrollRect, Vector2> OnValueChanged { get; }

        public Vector2 Value
        {
            get
            {
                return GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public bool Horizontal
        {
            get
            {
                return HorizontalSlider != null && HorizontalSlider.Enable;
            }
            set
            {
                if (HorizontalSlider != null)
                {
                    HorizontalSlider.Enable = value;
                }
            }
        }

        public bool Vertical
        {
            get
            {
                return VerticalSlider != null && VerticalSlider.Enable;
            }
            set
            {
                if (VerticalSlider != null)
                {
                    VerticalSlider.Enable = value;
                }
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
                if (content != value) 
                {
                    content = value;
                    UpdateContent();
                }
            }
        }

        public GUIComponent Viewport
        {
            get
            {
                return viewport;
            }
            set
            {
                if (viewport != value)
                {
                    viewport = value;
                    UpdateContent();
                }
            }
        }

        public GUIScrollRect()
        {
            Sensitivity = 0.1f;
            Horizontal = true;
            Vertical = true;

            OnValueChanged = eventHandlersStore.Create<GUIScrollRect, Vector2>(OnValueChangedEvent);
        }

        protected override void OnInit()
        {
            HorizontalSlider.Track.Value = Value.X;
            VerticalSlider.Track.Value = Value.Y;

            HorizontalSlider.Track.OnValueChanged.AddListener((s, v) =>
            {
                Value = new Vector2(v, VerticalSlider.Track.Value);
            });

            VerticalSlider.Track.OnValueChanged.AddListener((s, v) =>
            {
                Value = new Vector2(HorizontalSlider.Track.Value, v);
            });

            UpdateContent();
        }

        private void UpdateContent()
        {
            if (Content == null || Viewport == null)
            {
                return;
            }

            Vector2 contentSize = Content.Rectangle.Size.ToVector2();
            Vector2 viewportSize = Viewport.Rectangle.Size.ToVector2();

            float horizontal = contentSize.X > 0 ? viewportSize.X / contentSize.X : 0;
            if (horizontal > 0.0f && horizontal < 1.0f)
            {
                Horizontal = true;
                HorizontalSlider.Track.ThumbScale = horizontal;
            }
            else
            {
                Horizontal = false;
            }

            float vertical = contentSize.Y > 0 ? viewportSize.Y / contentSize.Y : 0;
            if (vertical > 0.0f && vertical < 1.0f)
            {
                Vertical = true;
                VerticalSlider.Track.ThumbScale = vertical;
            }
            else
            {
                Vertical = false;
            }

            Content.Pivot = (Vector2.One - viewportSize / contentSize) * Value;
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {

        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                float direction = MathV.Sign(mouse.ScrollWheelDelta);
                VerticalSlider.Track.Value += Sensitivity * direction;
            }
        }

        private static void ValueChangedCallback(DependencyObject dependencyObject, DependencyProperty<Vector2> property, Vector2 oldValue, Vector2 newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUIScrollRect slider)
                {
                    slider.UpdateContent();
                    slider.OnValueChanged.Invoke(slider, newValue);
                }
            }
        }
    }
}
