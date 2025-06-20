using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Math
{
    public static class Vector2Extension
    {
        public static Vector2 ToXY(this Vector3 v3)
        {
            return new Vector2(v3.X, v3.Y);
        }

        public static Vector3 ToXYZ(this Vector2 v2)
        {
            return new Vector3(v2.X, v2.Y, 0);
        }

        public static Vector2 PerpendicularClockwise(this Vector2 value)
        {
            return new Vector2(value.Y, -value.X);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 value)
        {
            return new Vector2(-value.Y, value.X);
        }

        public static float Angle(this Vector2 value)
        {
            return MathV.Angle(value);
        }

        public static float Angle(this Vector2 from, Vector2 to)
        {
            return MathV.Angle(from, to);
        }

        public static float AngleDegrees(this Vector2 from, Vector2 to)
        {
            return MathV.AngleDegrees(from, to);
        }

        public static Vector2 NormalizeF(this Vector2 value)
        {
            if (value == Vector2.Zero)
            {
                return Vector2.Zero;
            }

            float num = 1f / MathF.Sqrt(value.X * value.X + value.Y * value.Y);
            value.X *= num;
            value.Y *= num;
            return value;
        }

        public static bool IsNan(this Vector2 vector)
        {
            return float.IsNaN(vector.X) || float.IsNaN(vector.Y);
        }
    }
}
