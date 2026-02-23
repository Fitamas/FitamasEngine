using Fitamas.Math;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Processors
{
    public class DeadzoneVector2Processor : InputProcessor<Vector2>
    {
        public float Min = 0;
        public float Max = 0;

        public override Vector2 Process(Vector2 value)
        {
            return new Vector2(FMath.ClosestIfBetween(value.X, Min, Max),
                               FMath.ClosestIfBetween(value.Y, Min, Max));
        }
    }
}
