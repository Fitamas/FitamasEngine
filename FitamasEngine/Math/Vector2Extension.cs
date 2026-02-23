using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Math
{
    public static class Vector2Extension
    {
        public static Vector2 FromNumerics(this System.Numerics.Vector2 v2)
        {
            return new Vector2(v2.X, v2.Y);
        }

        public static Vector3 FromNumerics(this System.Numerics.Vector3 v3)
        {
            return new Vector3(v3.X, v3.Y, v3.Z);
        }

        public static Vector4 FromNumerics(this System.Numerics.Vector4 v4)
        {
            return new Vector4(v4.X, v4.Y, v4.Z, v4.W);
        }

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
            return FMath.Angle(value);
        }

        public static float Angle(this Vector2 from, Vector2 to)
        {
            return FMath.Angle(from, to);
        }

        public static float AngleDegrees(this Vector2 from, Vector2 to)
        {
            return FMath.AngleDegrees(from, to);
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
