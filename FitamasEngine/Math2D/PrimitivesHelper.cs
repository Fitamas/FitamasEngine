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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended
{
    internal class PrimitivesHelper
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static float SquaredDistanceToPointFromRectangle(Vector2 minimum, Vector2 maximum, Vector2 point)
        {
            // Real-Time Collision Detection, Christer Ericson, 2005. Chapter 5.1.3.1; Basic Primitive Tests - Closest-point Computations - Distance of Point to AABB.  pg 130-131
            var squaredDistance = 0.0f;

            // for each axis add up the excess distance outside the box

            // x-axis
            if (point.X < minimum.X)
            {
                var distance = minimum.X - point.X;
                squaredDistance += distance * distance;
            }
            else if (point.X > maximum.X)
            {
                var distance = maximum.X - point.X;
                squaredDistance += distance * distance;
            }

            // y-axis
            if (point.Y < minimum.Y)
            {
                var distance = minimum.Y - point.Y;
                squaredDistance += distance * distance;
            }
            else if (point.Y > maximum.Y)
            {
                var distance = maximum.Y - point.Y;
                squaredDistance += distance * distance;
            }
            return squaredDistance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ClosestPointToPointFromRectangle(Vector2 minimum, Vector2 maximum, Vector2 point, out Vector2 result)
        {
            // Real-Time Collision Detection, Christer Ericson, 2005. Chapter 5.1.2; Basic Primitive Tests - Closest-point Computations. pg 130-131

            result = point;

            // For each coordinate axis, if the point coordinate value is outside box, clamp it to the box, else keep it as is
            if (result.X < minimum.X)
                result.X = minimum.X;
            else if (result.X > maximum.X)
                result.X = maximum.X;

            if (result.Y < minimum.Y)
                result.Y = minimum.Y;
            else if (result.Y > maximum.Y)
                result.Y = maximum.Y;
        }

    }
}
