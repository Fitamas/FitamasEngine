using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public enum GUISliderDirection
    {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    }

    public class GUITrackBar : GUISlider
    {
        public bool WholeNumbers;

        public GUITrackBar(Rectangle rectangle, int minValue = 0, int maxValue = 10) : base(rectangle, minValue, maxValue)
        {

        }

        protected override float FindNearestValue(Point position)
        {
             if (WholeNumbers)
            {
                float value = base.FindNearestValue(position);

                return MathF.Round(value);
            }

            return base.FindNearestValue(position);
        }
    }
}
