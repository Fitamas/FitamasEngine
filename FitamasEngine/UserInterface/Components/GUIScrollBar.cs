using Fitamas.Math2D;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUIScrollBar : GUISlider
    {
        [SerializableField] private float size;

        public float Size
        {
            get
            {
                return size;
            }
            set
            {
                size = MathV.Clamp01(value);
                int lenght = (int)(Lenght * size);
                HandleSize = new Point(lenght, HandleSize.Y);
            }
        }

        public GUIScrollBar(Rectangle rectangle) : base(rectangle, 0, 1)
        {

        }

        protected override Point CalculateSliderPosition()
        {
            float remainingSize = Lenght * (1 - size);
            int offset = HandleSize.X / 2 - Lenght / 2;
            Point position = IsHorizontal ? new Point(offset, 0) : new Point(0, offset);

            switch (Direction)
            {
                case GUISliderDirection.LeftToRight:
                    position.X += (int)CalculateSliderProgress(remainingSize, Value);
                    break;
                case GUISliderDirection.RightToLeft:
                    position.X += (int)CalculateSliderProgress(remainingSize, MaxValue - Value);
                    break;
                case GUISliderDirection.TopToBottom:
                    position.Y += (int)CalculateSliderProgress(remainingSize, MaxValue - Value);
                    break;
                case GUISliderDirection.BottomToTop:
                    position.Y += (int)CalculateSliderProgress(remainingSize, Value);
                    break;
            }

            return position;
        }

        protected override float FindNearestValue(Point position) 
        {
            Point localPosition = position - Rectangle.Location - startDragOffset;
            float remainingSize = Lenght * (1 - size);
            float result = 0;

            switch (Direction)
            {
                case GUISliderDirection.LeftToRight:
                    result = FindNearestValue(remainingSize, localPosition.X);
                    break;
                case GUISliderDirection.RightToLeft:
                    result = FindNearestValue(remainingSize, remainingSize - localPosition.X);
                    break;
                case GUISliderDirection.TopToBottom:
                    result = FindNearestValue(remainingSize, localPosition.Y);
                    break;
                case GUISliderDirection.BottomToTop:
                    result = FindNearestValue(remainingSize, remainingSize - localPosition.Y);
                    break;
            }

            return result;
        }
    }
}
