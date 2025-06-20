using Fitamas.Math;
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

    public class GUISlider : GUIRange
    {
        public static readonly DependencyProperty<bool> WholeNumbersProperty = new DependencyProperty<bool>(false);

        public bool WholeNumbers
        {
            get
            {
                return GetValue(WholeNumbersProperty);
            }
            set
            {
                SetValue(WholeNumbersProperty, value);
            }
        }

        public GUISlider()
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
