using Fitamas.Math2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Fitamas.Serialization;
using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public abstract class GUISlider : GUIComponent, IDragMouseEvent
    {
        private float value;
        private float minValue;
        private float maxValue;
        private GUISliderDirection direction;

        public GUIImage SlidingArea;
        public GUIImage Handle;
        public Color DefoultColor = Color.White;
        public Color SelectColor = Color.DeepSkyBlue;

        protected bool isDrag;
        protected Point startDragOffset;

        public GUIEvent<GUISlider, float> OnValueChanged = new GUIEvent<GUISlider, float>();

        public bool IsHorizontal => direction == GUISliderDirection.LeftToRight || direction == GUISliderDirection.RightToLeft;
        public bool IsVertical => direction == GUISliderDirection.BottomToTop || direction == GUISliderDirection.TopToBottom;

        public float MinValue => minValue;
        public float MaxValue => maxValue;

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = MathV.Clamp(value, minValue, maxValue);

                OnValueChanged?.Invoke(this, this.value);
            }
        }

        public int Lenght
        {
            get
            {
                return IsHorizontal ? Rectangle.Width : Rectangle.Height;
            }
            set
            {
                LocalSize = IsHorizontal ? new Point(value, LocalSize.Y) : new Point(LocalSize.X, value);
            }
        }

        public Point HandleSize
        {
            get
            {
                Point result = Handle.LocalSize;

                return IsHorizontal ? result : new Point(result.Y, result.X);
            }
            set
            {
                if (Handle != null)
                {
                    Handle.LocalSize = IsHorizontal ? value : new Point(value.Y, value.X);
                }
            }
        }

        public GUISliderDirection Direction
        {
            get
            {
                return direction;
            }
            set
            {
                bool isHorizontal = direction == GUISliderDirection.LeftToRight || direction == GUISliderDirection.RightToLeft;
                bool isValueHorizontal = value == GUISliderDirection.LeftToRight || value == GUISliderDirection.RightToLeft;

                if (isHorizontal != isValueHorizontal)
                {
                    ForceRotateSlider();
                }

                direction = value;
            }
        }

        public GUISlider(int minValue = 0, int maxValue = 10)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            value = minValue;
            RaycastTarget = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Handle.LocalPosition = CalculateSliderPosition();

            if (Handle != null)
            {
                Handle.Color = isDrag || Handle.IsMouseOver ? SelectColor : DefoultColor;
            }
        }

        private void ForceRotateSlider()
        {
            Point scale = new Point(LocalSize.Y, LocalSize.X);
            LocalSize = scale;

            Point sliderScale = Handle.LocalSize;
            Handle.LocalSize = new Point(sliderScale.Y, sliderScale.X);
        }

        protected virtual Point CalculateSliderPosition()
        {
            Rectangle rectangle = Rectangle;
            Point position = IsHorizontal ? new Point(-LocalSize.X / 2, 0) : new Point(0, -LocalSize.Y / 2);

            switch (Direction)
            {
                case GUISliderDirection.LeftToRight:
                    position.X += (int)CalculateSliderProgress(rectangle.Width, value);
                    break;
                case GUISliderDirection.RightToLeft:
                    position.X += (int)CalculateSliderProgress(rectangle.Width, maxValue - value);
                    break;
                case GUISliderDirection.TopToBottom:
                    position.Y += (int)CalculateSliderProgress(rectangle.Height, maxValue - value);
                    break;
                case GUISliderDirection.BottomToTop:
                    position.Y += (int)CalculateSliderProgress(rectangle.Height, value);
                    break;
            }

            return position;
        }

        protected float CalculateSliderProgress(float lenght, float value)
        {
            float distance = Math.Abs(maxValue - minValue);

            if (distance > 0)
            {
                float k = value / distance;
                return lenght * k;
            }

            return 0;
        }

        protected virtual float FindNearestValue(Point position)
        {
            Point localPosition = position - Rectangle.Location;
            float result = 0;

            switch (Direction)
            {
                case GUISliderDirection.LeftToRight:
                    result = FindNearestValue(Lenght, localPosition.X);
                    break;
                case GUISliderDirection.RightToLeft:
                    result = FindNearestValue(Lenght, Lenght - localPosition.X);
                    break;
                case GUISliderDirection.TopToBottom:
                    result = FindNearestValue(Lenght, localPosition.Y);
                    break;
                case GUISliderDirection.BottomToTop:
                    result = FindNearestValue(Lenght, Lenght - localPosition.Y);
                    break;
            }

            return result;
        }

        protected float FindNearestValue(float lenght, float progress)
        {
            if (progress < 0)
            {
                return minValue;
            }
            else if (progress > lenght)
            {
                return maxValue;
            }
            else
            {
                float distance = Math.Abs(maxValue - minValue);
                float k = 0;

                if (distance > 0)
                {
                    k = lenght / distance;
                }

                return progress / k;
            }
        }

        public void OnStartDragMouse(MouseEventArgs mouse)
        {
            if (Handle.IsMouseOver)
            {
                startDragOffset = mouse.Position - Handle.Rectangle.Location;
                isDrag = true;
            }
        }

        public void OnDragMouse(MouseEventArgs mouse)
        {
            if (isDrag)
            {
                Value = FindNearestValue(mouse.Position);
            }
        }

        public void OnEndDragMouse(MouseEventArgs mouse)
        {
            isDrag = false;
        }
    }
}
