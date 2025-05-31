using Fitamas.Events;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUITrack : GUIComponent
    {
        public static readonly DependencyProperty<float> MinValueProperty = new DependencyProperty<float>(0, false);

        public static readonly DependencyProperty<float> MaxValueProperty = new DependencyProperty<float>(1, false);

        public static readonly DependencyProperty<float> ValueProperty = new DependencyProperty<float>(ValueChangedCallback, 0, false);

        public static readonly DependencyProperty<GUISliderDirection> DirectionProperty = new DependencyProperty<GUISliderDirection>(DirectionChangedCallback, GUISliderDirection.LeftToRight, false);

        public static readonly RoutedEvent OnValueChangedEvent = new RoutedEvent();

        private GUIThumb thumb;
        private float thumbScale;

        public bool IsHorizontal => Direction == GUISliderDirection.LeftToRight || Direction == GUISliderDirection.RightToLeft;
        public bool IsVertical => Direction == GUISliderDirection.BottomToTop || Direction == GUISliderDirection.TopToBottom;

        public MonoEvent<GUITrack, float> OnValueChanged { get; set; }

        public float MinValue
        {
            get
            {
                return GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public float MaxValue
        {
            get
            {
                return GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public float Value
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

        public GUISliderDirection Direction
        {
            get
            {
                return GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }

        public GUIThumb Thumb
        {
            get
            {
                return thumb;
            }
            set
            {
                if (thumb != value)
                {
                    thumb = value;
                    UpdateThumb();
                }

            }
        }

        public int Lenght
        {
            get
            {
                return IsHorizontal ? Rectangle.Width : Rectangle.Height;
            }
        }

        public float ThumbScale
        {
            get
            {
                return thumbScale;
            }
            set
            {
                thumbScale = MathV.Clamp01(value);
                UpdateThumb();
            }
        }

        public GUITrack()
        {
            RaycastTarget = true;

            OnValueChanged = eventHandlersStore.Create<GUITrack, float>(OnValueChangedEvent);
        }

        protected override Rectangle AvailableRectangle(GUIComponent component)
        {
            Point position = CalculateSliderPosition() + Rectangle.Location;
            Point size;

            if (IsHorizontal)
            {
                size = new Point((int)(Lenght * thumbScale), Rectangle.Height);
            }
            else
            {
                size = new Point(Rectangle.Width, (int)(Lenght * thumbScale));
            }

            return new Rectangle(position, size);
        }

        private void UpdateThumb()
        {
            SetDirty();
        }

        protected virtual Point CalculateSliderPosition()
        {
            int remainingSize = Lenght - (int)(Lenght * thumbScale);
            Point position = Point.Zero;
            float value = Value;

            switch (Direction)
            {
                case GUISliderDirection.LeftToRight:
                    position.X = CalculateSliderProgress(remainingSize, value);
                    break;
                case GUISliderDirection.RightToLeft:
                    position.X = CalculateSliderProgress(remainingSize, MaxValue - value);
                    break;
                case GUISliderDirection.TopToBottom:
                    position.Y = CalculateSliderProgress(remainingSize, MaxValue - value);
                    break;
                case GUISliderDirection.BottomToTop:
                    position.Y = CalculateSliderProgress(remainingSize, value);
                    break;
            }

            return position;
        }

        protected int CalculateSliderProgress(int lenght, float value)
        {
            float distance = Math.Abs(MaxValue - MinValue);

            if (distance > 0)
            {
                float k = value / distance;
                return (int)(lenght * k);
            }

            return 0;
        }

        private static void ValueChangedCallback(DependencyObject dependencyObject, DependencyProperty<float> property, float oldValue, float newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUITrack slider)
                {
                    newValue = MathV.Clamp(newValue, slider.MinValue, slider.MaxValue);
                    dependencyObject.SetValue(property, newValue);
                    slider.OnValueChanged.Invoke(slider, newValue);
                    slider.UpdateThumb();
                }
            }
        }

        private static void DirectionChangedCallback(DependencyObject dependencyObject, DependencyProperty<GUISliderDirection> property, GUISliderDirection oldValue, GUISliderDirection newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUITrack slider)
                {
                    bool isHorizontal = oldValue == GUISliderDirection.LeftToRight || oldValue == GUISliderDirection.RightToLeft;
                    bool isValueHorizontal = newValue == GUISliderDirection.LeftToRight || newValue == GUISliderDirection.RightToLeft;

                    if (isHorizontal != isValueHorizontal)
                    {
                        slider.UpdateThumb();
                    }
                }
            }
        }
    }
}
