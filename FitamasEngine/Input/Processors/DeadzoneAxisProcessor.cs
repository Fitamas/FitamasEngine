using Fitamas.Math;
using System;

namespace Fitamas.Input.Processors
{
    public class DeadzoneAxisProcessor : InputProcessor<float>
    {
        public float Min = 0;
        public float Max = 0;

        public override float Process(float value)
        {
            return FMath.ClosestIfBetween(value, Min, Max);
        }
    }
}
