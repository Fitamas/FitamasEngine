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

namespace MonoGame.Extended
{
    /// <summary>
    ///     A random number generator that uses a fast algorithm to generate random values.
    ///     The speed comes at the price of true 'randomness' though, there are noticeable
    ///     patterns & it compares quite unfavourably to other algorithms in that respect.
    ///     It's a good choice in situations where speed is more desirable than a
    ///     good random distribution, and a poor choice when random distribution is important.
    /// </summary>
    public class FastRandom
    {
        private int _state;

        public FastRandom()
            : this(1)
        {
        }

        public FastRandom(int seed)
        {
            if (seed < 1)
                throw new ArgumentOutOfRangeException(nameof(seed), "seed must be greater than zero");

            _state = seed;
        }

        /// <summary>
        ///     Gets the next random integer value.
        /// </summary>
        /// <returns>A random positive integer.</returns>
        public int Next()
        {
            _state = 214013*_state + 2531011;
            return (_state >> 16) & 0x7FFF;
        }

        /// <summary>
        ///     Gets the next random integer value which is greater than zero and less than or equal to
        ///     the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random integer value to return.</param>
        /// <returns>A random integer value between zero and the specified maximum value.</returns>
        public int Next(int max)
        {
            return (int) (max*NextSingle() + 0.5f);
        }

        /// <summary>
        ///     Gets the next random integer between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        public int Next(int min, int max)
        {
            return (int) ((max - min)*NextSingle() + 0.5f) + min;
        }

        /// <summary>
        ///     Gets the next random integer between the specified range of values.
        /// </summary>
        /// <param name="range">A range representing the inclusive minimum and maximum values.</param>
        /// <returns>A random integer between the specified minumum and maximum values.</returns>
        public int Next(Range<int> range)
        {
            return Next(range.Min, range.Max);
        }

        /// <summary>
        ///     Gets the next random single value.
        /// </summary>
        /// <returns>A random single value between 0 and 1.</returns>
        public float NextSingle()
        {
            return Next()/(float) short.MaxValue;
        }

        /// <summary>
        ///     Gets the next random single value which is greater than zero and less than or equal to
        ///     the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random single value to return.</param>
        /// <returns>A random single value between zero and the specified maximum value.</returns>
        public float NextSingle(float max)
        {
            return max*NextSingle();
        }

        /// <summary>
        ///     Gets the next random single value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public float NextSingle(float min, float max)
        {
            return (max - min)*NextSingle() + min;
        }

        /// <summary>
        ///     Gets the next random single value between the specified range of values.
        /// </summary>
        /// <param name="range">A range representing the inclusive minimum and maximum values.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public float NextSingle(Range<float> range)
        {
            return NextSingle(range.Min, range.Max);
        }

        /// <summary>
        ///     Gets the next random angle value.
        /// </summary>
        /// <returns>A random angle value.</returns>
        public float NextAngle()
        {
            return NextSingle(-MathHelper.Pi, MathHelper.Pi);
        }

        public void NextUnitVector(out Vector2 vector)
        {
            var angle = NextAngle();
            vector = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
        }
    }
}