using Fitamas.Math;
using System;

namespace Fitamas.Input.Processors
{
    public class NormalizeAxisProcessor : InputProcessor<float>
    {
        public override float Process(float value)
        {
            return MathV.Clamp01(value);
        }
    }
}
