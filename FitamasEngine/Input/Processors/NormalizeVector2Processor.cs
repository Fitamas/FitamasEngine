using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Processors
{
    public class NormalizeVector2Processor : InputProcessor<Vector2>
    {
        public override Vector2 Process(Vector2 value)
        {
            return value.NormalizeF();
        }
    }
}
