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

using Fitamas;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended
{
    public static class RectangleExtensions
    {
        /// <summary>
        ///     Gets the corners of the rectangle in a clockwise direction starting at the top left.
        /// </summary>
        public static Point[] GetCorners(this Rectangle rectangle)
        {
            var corners = new Point[4];
            corners[0] = new Point(rectangle.Left, rectangle.Top);
            corners[1] = new Point(rectangle.Right, rectangle.Top);
            corners[2] = new Point(rectangle.Right, rectangle.Bottom);
            corners[3] = new Point(rectangle.Left, rectangle.Bottom);
            return corners;
        }

        /// <summary>
        ///     Gets the corners of the rectangle in a clockwise direction starting at the top left.
        /// </summary>
        public static Vector2[] GetCorners(this RectangleF rectangle)
        {
            var corners = new Vector2[4];
            corners[0] = new Vector2(rectangle.Left, rectangle.Top);
            corners[1] = new Vector2(rectangle.Right, rectangle.Top);
            corners[2] = new Vector2(rectangle.Right, rectangle.Bottom);
            corners[3] = new Vector2(rectangle.Left, rectangle.Bottom);
            return corners;
        }

        public static Rectangle ToRectangle(this RectangleF rectangle)
        {
            return new Rectangle((int) rectangle.X, (int) rectangle.Y, (int) rectangle.Width, (int) rectangle.Height);
        }

        public static RectangleF ToRectangleF(this Rectangle rectangle)
        {
            return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rectangle Clip(this Rectangle rectangle, Rectangle clippingRectangle)
        {
            var clip = clippingRectangle;
            rectangle.X = clip.X > rectangle.X ? clip.X : rectangle.X;
            rectangle.Y = clip.Y > rectangle.Y ? clip.Y : rectangle.Y;
            rectangle.Width = rectangle.Right > clip.Right ? clip.Right - rectangle.X : rectangle.Width;
            rectangle.Height = rectangle.Bottom > clip.Bottom ? clip.Bottom - rectangle.Y : rectangle.Height;

            if (rectangle.Width <= 0 || rectangle.Height <= 0)
                return Rectangle.Empty;

            return rectangle;
        }

        public static RectangleF Clip(this RectangleF rectangle, RectangleF clippingRectangle)
        {
            var clip = clippingRectangle;
            rectangle.X = clip.X > rectangle.X ? clip.X : rectangle.X;
            rectangle.Y = clip.Y > rectangle.Y ? clip.Y : rectangle.Y;
            rectangle.Width = rectangle.Right > clip.Right ? clip.Right - rectangle.X : rectangle.Width;
            rectangle.Height = rectangle.Bottom > clip.Bottom ? clip.Bottom - rectangle.Y : rectangle.Height;

            if(rectangle.Width <= 0 || rectangle.Height <= 0)
                return RectangleF.Empty;

            return rectangle;
        }

        //public static Rectangle Intersection(this Rectangle rectangle, Rectangle second)
        //{
        //    Intersection(rectangle, second, out Rectangle result);
        //    return result;
        //}

        //public static void Intersection(this Rectangle first, Rectangle second, out Rectangle result)
        //{
        //    //var firstMinimum = first.Center - first.HalfExtents;
        //    //var firstMaximum = first.Center + first.HalfExtents;
        //    //var secondMinimum = second.Center - second.HalfExtents;
        //    //var secondMaximum = second.Center + second.HalfExtents;

        //    var firstMinimum = new Point(first.Left, first.Bottom);
        //    var firstMaximum = new Point(first.Right, first.Top);
        //    var secondMinimum = new Point(second.Left, second.Bottom);
        //    var secondMaximum = new Point(second.Right, second.Top);

        //    var minimum = Maximum(firstMinimum, secondMinimum);
        //    var maximum = Minimum(firstMaximum, secondMaximum);

        //    if ((maximum.X < minimum.X) || (maximum.Y < minimum.Y))
        //        result = new Rectangle();
        //    else
        //        result = CreateFrom(minimum, maximum);

        //    Debug.Log(maximum + " " + minimum);
        //}

        //public static Point Minimum(Point first, Point second)
        //{
        //    return new Point(first.X < second.X ? first.X : second.X,
        //        first.Y < second.Y ? first.Y : second.Y);
        //}

        //public static Point Maximum(Point first, Point second)
        //{
        //    return new Point(first.X > second.X ? first.X : second.X,
        //        first.Y > second.Y ? first.Y : second.Y);
        //}

        //public static Rectangle CreateFrom(Point minimum, Point maximum)
        //{
        //    Point position = new Point(minimum.X, maximum.Y);
        //    Point scale = new Point(maximum.X - minimum.X, maximum.Y - minimum.Y);

        //    //Rectangle result = new Rectangle();
        //    //result.Center = new Point2((maximum.X + minimum.X) * 0.5f, (maximum.Y + minimum.Y) * 0.5f);
        //    //result.HalfExtents = new Vector2((maximum.X - minimum.X) * 0.5f, (maximum.Y - minimum.Y) * 0.5f);
        //    return new Rectangle(position, scale);
        //}
    }
}