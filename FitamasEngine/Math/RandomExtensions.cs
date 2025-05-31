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

using System;
using Microsoft.Xna.Framework;

namespace Fitamas.Math2D
{
    public static class RandomExtensions
    {
        public static int Next(this Random random, Range<int> range)
        {
            return random.Next(range.Min, range.Max);
        }

        public static float NextSingle(this Random random, float min, float max)
        {
            return (max - min)*NextSingle(random) + min;
        }

        public static float NextSingle(this Random random, float max)
        {
            return max*NextSingle(random);
        }

        public static float NextSingle(this Random random)
        {
            return (float) random.NextDouble();
        }

        public static float NextSingle(this Random random, Range<float> range)
        {
            return NextSingle(random, range.Min, range.Max);
        }

        public static float NextAngle(this Random random)
        {
            return NextSingle(random, -MathHelper.Pi, MathHelper.Pi);
        }

        public static void NextUnitVector(this Random random, out Vector2 vector)
        {
            var angle = NextAngle(random);
            vector = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }
    }
}