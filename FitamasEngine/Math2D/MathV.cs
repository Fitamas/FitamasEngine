using LibTessDotNet;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Math2D
{
    public static class MathV
    {
        private const float toRadian = MathF.PI / 180;
        private const float toDegrees = 180 / MathF.PI;

        public static float ToRadian
        {
            get
            {
                return toRadian;
            }
        }

        public static float ToDegrees
        {
            get
            {
                return toDegrees;
            }
        }

        public static float Sign(float value)
        {
            if (value >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public static float Lerp(float value1, float value2, float amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        public static float Clamp01(float value)
        {
            return Clamp(value, 0.0f, 1.0f);
        }

        public static float Angle(Vector2 value)
        {
            return MathF.Atan2(value.X, -value.Y);
        }

        public static float Angle(Vector2 from, Vector2 to)
        {
            float num = MathF.Sqrt(from.LengthSquared() * to.LengthSquared());
            if (num < 1E-15f)
            {
                return 0f;
            }

            float num2 = Clamp(Vector2.Dot(from, to) / num, -1f, 1f);
            return MathF.Acos(num2);
        }

        public static float SignedAngle(Vector2 from, Vector2 to)
        {
            float num = Angle(from, to);
            float num2 = Sign(from.X * to.Y - from.Y * to.X);
            return num * num2;
        }

        public static float AngleDegrees(Vector2 from, Vector2 to)
        {
            return Angle(from, to) * ToDegrees;
        }

        public static float SignedAngleDegrees(Vector2 from, Vector2 to)
        {
            return SignedAngle(from, to) * ToDegrees;
        }

        public static Vector2 ProjectOnTo(Vector2 a, Vector2 b, Vector2 point)
        {
            var direction = b - a;
            var vector = point - a;
            return a + ProjectOnTo(vector, direction);
        }

        public static Vector2 ProjectOnTo(Vector2 v1, Vector2 v2)
        {
            float v2_ls = v2.LengthSquared();
            return v2 * (Vector2.Dot(v2, v1) / v2_ls);
        }

        public static Vector2 Project(Vector2 direction, Vector2 normal)
        {
            return direction - Vector2.Dot(direction, normal) * normal;
        }

        public static Vector2 RotateDegree(Vector2 vector, float angle)
        {
            return Rotate(vector, angle / 180 * MathF.PI);
        }

        public static Vector2 Rotate(Vector2 vector, float radians)
        {
            Vector2 result = new Vector2();
            result.X = vector.X * MathF.Cos(radians) - vector.Y * MathF.Sin(radians);
            result.Y = vector.X * MathF.Sin(radians) + vector.Y * MathF.Cos(radians);
            return result;
        }

        public static Vector2 LinesIntersection(out bool canIntersect, Vector2 line1V1, Vector2 line1V2, Vector2 line2V1, Vector2 line2V2)
        {
            //Line1
            float A1 = line1V2.Y - line1V1.Y;
            float B1 = line1V1.X - line1V2.X;
            float C1 = A1 * line1V1.X + B1 * line1V1.Y;

            //Line2
            float A2 = line2V2.Y - line2V1.Y;
            float B2 = line2V1.X - line2V2.X;
            float C2 = A2 * line2V1.X + B2 * line2V1.Y;

            Vector2 result = new Vector2();
            float delta = A1 * B2 - A2 * B1;

            if (delta == 0)
            {
                canIntersect = false;
                return result;
            }

            result.X = (B2 * C1 - B1 * C2) / delta;
            result.Y = (A1 * C2 - A2 * C1) / delta;
            canIntersect = true;
            return result;
        }

        public static float DistancePointToSegment(Vector2 a, Vector2 b, Vector2 point)
        {
            float sqrDistannce = Vector2.DistanceSquared(a, b);
            if (sqrDistannce == 0)
            {
                return Vector2.Distance(point, a);
            }

            float num = Vector2.Dot(point - a, b - a) / sqrDistannce;
            if (num < 0)
            {
                return Vector2.Distance(point, a);
            }
            else if (num > 1f)
            {
                return Vector2.Distance(point, b);
            }
            else
            {
                return Vector2.Distance(point, a + (b - a) * num);
            }
        }
    }
}
