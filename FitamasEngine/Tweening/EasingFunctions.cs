/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Tweening
{
    public static class EasingFunctions
    {
        public static float Linear(float value) => value;

        public static float CubicIn(float value) => Power.In(value, 3);
        public static float CubicOut(float value) => Power.Out(value, 3);
        public static float CubicInOut(float value) => Power.InOut(value, 3);

        public static float QuadraticIn(float value) => Power.In(value, 2);
        public static float QuadraticOut(float value) => Power.Out(value, 2);
        public static float QuadraticInOut(float value) => Power.InOut(value, 2);

        public static float QuarticIn(float value) => Power.In(value, 4);
        public static float QuarticOut(float value) => Power.Out(value, 4);
        public static float QuarticInOut(float value) => Power.InOut(value, 4);

        public static float QuinticIn(float value) => Power.In(value, 5);
        public static float QuinticOut(float value) => Power.Out(value, 5);
        public static float QuinticInOut(float value) => Power.InOut(value, 5);

        public static float SineIn(float value) => (float)System.Math.Sin(value * MathHelper.PiOver2 - MathHelper.PiOver2) + 1;
        public static float SineOut(float value) => (float)System.Math.Sin(value * MathHelper.PiOver2);
        public static float SineInOut(float value) => (float)(System.Math.Sin(value * MathHelper.Pi - MathHelper.PiOver2) + 1) / 2;

        public static float ExponentialIn(float value) => (float)System.Math.Pow(2, 10 * (value - 1));
        public static float ExponentialOut(float value) => Out(value, ExponentialIn);
        public static float ExponentialInOut(float value) => InOut(value, ExponentialIn);

        public static float CircleIn(float value) => (float)-(System.Math.Sqrt(1 - value * value) - 1);
        public static float CircleOut(float value) => (float)System.Math.Sqrt(1 - (value - 1) * (value - 1));
        public static float CircleInOut(float value) => (float)(value <= .5 ? (System.Math.Sqrt(1 - value * value * 4) - 1) / -2 : (System.Math.Sqrt(1 - (value * 2 - 2) * (value * 2 - 2)) + 1) / 2);

        public static float ElasticIn(float value)
        {
            const int oscillations = 1;
            const float springiness = 3f;
            var e = (System.Math.Exp(springiness * value) - 1) / (System.Math.Exp(springiness) - 1);
            return (float)(e * System.Math.Sin((MathHelper.PiOver2 + MathHelper.TwoPi * oscillations) * value));
        }

        public static float ElasticOut(float value) => Out(value, ElasticIn);
        public static float ElasticInOut(float value) => InOut(value, ElasticIn);

        public static float BackIn(float value)
        {
            const float amplitude = 1f;
            return (float)(System.Math.Pow(value, 3) - value * amplitude * System.Math.Sin(value * MathHelper.Pi));
        }

        public static float BackOut(float value) => Out(value, BackIn);
        public static float BackInOut(float value) => InOut(value, BackIn);

        public static float BounceOut(float value) => Out(value, BounceIn);
        public static float BounceInOut(float value) => InOut(value, BounceIn);

        public static float BounceIn(float value)
        {
            const float bounceConst1 = 2.75f;
            var bounceConst2 = (float)System.Math.Pow(bounceConst1, 2);

            value = 1 - value; //flip x-axis

            if (value < 1 / bounceConst1) // big bounce
                return 1f - bounceConst2 * value * value;

            if (value < 2 / bounceConst1)
                return 1 - (float)(bounceConst2 * System.Math.Pow(value - 1.5f / bounceConst1, 2) + .75);

            if (value < 2.5 / bounceConst1)
                return 1 - (float)(bounceConst2 * System.Math.Pow(value - 2.25f / bounceConst1, 2) + .9375);

            //small bounce
            return 1f - (float)(bounceConst2 * System.Math.Pow(value - 2.625f / bounceConst1, 2) + .984375);
        }


        private static float Out(float value, Func<float, float> function)
        {
            return 1 - function(1 - value);
        }

        private static float InOut(float value, Func<float, float> function)
        {
            if (value < 0.5f)
                return 0.5f * function(value * 2);

            return 1f - 0.5f * function(2 - value * 2);
        }

        private static class Power
        {
            public static float In(double value, int power)
            {
                return (float)System.Math.Pow(value, power);
            }

            public static float Out(double value, int power)
            {
                var sign = power % 2 == 0 ? -1 : 1;
                return (float)(sign * (System.Math.Pow(value - 1, power) + sign));
            }

            public static float InOut(double s, int power)
            {
                s *= 2;

                if (s < 1)
                    return In(s, power) / 2;

                var sign = power % 2 == 0 ? -1 : 1;
                return (float)(sign / 2.0 * (System.Math.Pow(s - 2, power) + sign * 2));
            }
        }
    }
}
