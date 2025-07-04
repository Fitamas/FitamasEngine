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
using System.Diagnostics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace Fitamas.Math
{
    // Real-Time Collision Detection, Christer Ericson, 2005. Chapter 4.2; Bounding Volumes - Axis-aligned Bounding Boxes (AABBs). pg 77

    /// <summary>
    ///     An axis-aligned, four sided, two dimensional box defined by a top-left position (<see cref="X" /> and
    ///     <see cref="Y" />) and a size (<see cref="Width" /> and <see cref="Height" />).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         An <see cref="RectangleF" /> is categorized by having its faces oriented in such a way that its
    ///         face normals are at all times parallel with the axes of the given coordinate system.
    ///     </para>
    ///     <para>
    ///         The bounding <see cref="RectangleF" /> of a rotated <see cref="RectangleF" /> will be equivalent or larger
    ///         in size than the original depending on the angle of rotation.
    ///     </para>
    /// </remarks>
    /// <seealso cref="IEquatable{T}" />
    /// <seealso cref="IEquatableByRef{T}" />
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        ///     The <see cref="RectangleF" /> with <see cref="X" />, <see cref="Y" />, <see cref="Width" /> and
        ///     <see cref="Height" /> all set to <code>0.0f</code>.
        /// </summary>
        public static readonly RectangleF Empty = new RectangleF();

        /// <summary>
        ///     The x-coordinate of the top-left corner position of this <see cref="RectangleF" />.
        /// </summary>
        [DataMember] public float X;

        /// <summary>
        ///     The y-coordinate of the top-left corner position of this <see cref="RectangleF" />.
        /// </summary>
        [DataMember] public float Y;

        /// <summary>
        ///     The width of this <see cref="RectangleF" />.
        /// </summary>
        [DataMember] public float Width;

        /// <summary>
        ///     The height of this <see cref="RectangleF" />.
        /// </summary>
        [DataMember] public float Height;

        /// <summary>
        ///     Gets the x-coordinate of the left edge of this <see cref="RectangleF" />.
        /// </summary>
        public float Left => X;

        /// <summary>
        ///     Gets the x-coordinate of the right edge of this <see cref="RectangleF" />.
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        ///     Gets the y-coordinate of the top edge of this <see cref="RectangleF" />.
        /// </summary>
        public float Top => Y;

        /// <summary>
        ///     Gets the y-coordinate of the bottom edge of this <see cref="RectangleF" />.
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        ///     Gets a value indicating whether this <see cref="RectangleF" /> has a <see cref="X" />, <see cref="Y" />,
        ///     <see cref="Width" />,
        ///     <see cref="Height" /> all equal to <code>0.0f</code>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty => Width.Equals(0) && Height.Equals(0) && X.Equals(0) && Y.Equals(0);

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the the top-left of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the center of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 Center => new Vector2(X + Width * 0.5f, Y + Height * 0.5f);

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the top-left of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 TopLeft => new Vector2(X, Y);

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the top-right of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 TopRight => new Vector2(X + Width, Y);

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the bottom-left of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 BottomLeft => new Vector2(X, Y + Height);

        /// <summary>
        ///     Gets the <see cref="Point2" /> representing the bottom-right of this <see cref="RectangleF" />.
        /// </summary>
        public Vector2 BottomRight => new Vector2(X + Width, Y + Height);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RectangleF" /> structure from the specified top-left xy-coordinate
        ///     <see cref="float" />s, width <see cref="float" /> and height <see cref="float" />.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RectangleF" /> structure from the specified top-left
        ///     <see cref="Point2" /> and the extents <see cref="Size2" />.
        /// </summary>
        /// <param name="position">The top-left point.</param>
        /// <param name="size">The extents.</param>
        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        /// <summary>
        ///     Computes the <see cref="RectangleF" /> that contains the two specified
        ///     <see cref="RectangleF" /> structures.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <param name="result">The resulting rectangle that contains both the <paramref name="first" /> and the
        ///     <paramref name="second" />.</param>
        public static void Union(ref RectangleF first, ref RectangleF second, out RectangleF result)
        {
            result.X = System.Math.Min(first.X, second.X);
            result.Y = System.Math.Min(first.Y, second.Y);
            result.Width = System.Math.Max(first.Right, second.Right) - result.X;
            result.Height = System.Math.Max(first.Bottom, second.Bottom) - result.Y;
        }

        /// <summary>
        ///     Computes the <see cref="RectangleF" /> that contains the two specified
        ///     <see cref="RectangleF" /> structures.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>
        ///     An <see cref="RectangleF" /> that contains both the <paramref name="first" /> and the
        ///     <paramref name="second" />.
        /// </returns>
        public static RectangleF Union(RectangleF first, RectangleF second)
        {
            RectangleF result;
            Union(ref first, ref second, out result);
            return result;
        }

        /// <summary>
        ///     Computes the <see cref="RectangleF" /> that contains both the specified <see cref="RectangleF" /> and this <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        ///     An <see cref="RectangleF" /> that contains both the <paramref name="rectangle" /> and
        ///     this <see cref="RectangleF" />.
        /// </returns>
        public RectangleF Union(RectangleF rectangle)
        {
            RectangleF result;
            Union(ref this, ref rectangle, out result);
            return result;
        }

        /// <summary>
        ///     Determines whether the two specified <see cref="RectangleF" /> structures intersect.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="first" /> intersects with the <see cref="second" />; otherwise, <c>false</c>.
        /// </returns>
        public static bool Intersects(ref RectangleF first, ref RectangleF second)
        {
            return first.X < second.X + second.Width && first.X + first.Width > second.X &&
                   first.Y < second.Y + second.Height && first.Y + first.Height > second.Y;
        }

        /// <summary>
        ///     Determines whether the two specified <see cref="RectangleF" /> structures intersect.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="first" /> intersects with the <see cref="second" />; otherwise, <c>false</c>.
        /// </returns>
        public static bool Intersects(RectangleF first, RectangleF second)
        {
            return Intersects(ref first, ref second);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="RectangleF" /> intersects with this
        ///     <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The bounding rectangle.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="rectangle" /> intersects with this
        ///     <see cref="RectangleF" />; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public bool Intersects(RectangleF rectangle)
        {
            return Intersects(ref this, ref rectangle);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="RectangleF" /> contains the specified
        ///     <see cref="Point2" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="point">The point.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="rectangle" /> contains the <paramref name="point" />; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public static bool Contains(ref RectangleF rectangle, ref Vector2 point)
        {
            return rectangle.X <= point.X && point.X < rectangle.X + rectangle.Width && rectangle.Y <= point.Y && point.Y < rectangle.Y + rectangle.Height;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="RectangleF" /> contains the specified
        ///     <see cref="Point2" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="point">The point.</param>
        /// <returns>
        ///     <c>true</c> if the <paramref name="rectangle" /> contains the <paramref name="point" />; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public static bool Contains(RectangleF rectangle, Vector2 point)
        {
            return Contains(ref rectangle, ref point);
        }

        /// <summary>
        ///     Determines whether this <see cref="RectangleF" /> contains the specified
        ///     <see cref="Point2" />.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        ///     <c>true</c> if the this <see cref="RectangleF"/> contains the <paramref name="point" />; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public bool Contains(Vector2 point)
        {
            return Contains(ref this, ref point);
        }

        /// <summary>
        ///     Computes the squared distance from this <see cref="RectangleF"/> to a <see cref="Point2"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The squared distance from this <see cref="RectangleF"/> to the <paramref name="point"/>.</returns>
        public float SquaredDistanceTo(Vector2 point)
        {
            return PrimitivesHelper.SquaredDistanceToPointFromRectangle(TopLeft, BottomRight, point);
        }

        /// <summary>
        ///     Computes the distance from this <see cref="RectangleF"/> to a <see cref="Point2"/>.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The distance from this <see cref="RectangleF"/> to the <paramref name="point"/>.</returns>
        public float DistanceTo(Vector2 point)
        {
            return (float)System.Math.Sqrt(SquaredDistanceTo(point));
        }

        /// <summary>
        ///     Computes the closest <see cref="Point2" /> on this <see cref="RectangleF" /> to a specified
        ///     <see cref="Point2" />.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The closest <see cref="Point2" /> on this <see cref="RectangleF" /> to the <paramref name="point" />.</returns>
        public Vector2 ClosestPointTo(Vector2 point)
        {
            Vector2 result;
            PrimitivesHelper.ClosestPointToPointFromRectangle(TopLeft, BottomRight, point, out result);
            return result;
        }

        public void Inflate(float horizontalAmount, float verticalAmount)
        {
            X -= horizontalAmount;
            Y -= verticalAmount;
            Width += horizontalAmount * 2;
            Height += verticalAmount * 2;
        }

        public void Offset(float offsetX, float offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }

        public void Offset(Vector2 amount)
        {
            X += amount.X;
            Y += amount.Y;
        }

        /// <summary>
        ///     Compares two <see cref="RectangleF" /> structures. The result specifies whether the values of the
        ///     <see cref="X" />, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height" /> fields of the two <see cref="RectangleF" /> structures
        ///     are equal.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>
        ///     <c>true</c> if the values of the
        ///     <see cref="X" />, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height" /> fields of the two <see cref="RectangleF" /> structures
        ///     are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(RectangleF first, RectangleF second)
        {
            return first.Equals(ref second);
        }

        /// <summary>
        ///     Compares two <see cref="RectangleF" /> structures. The result specifies whether the values of the
        ///     <see cref="X" />, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height" /> fields of the two <see cref="RectangleF" /> structures
        ///     are unequal.
        /// </summary>
        /// <param name="first">The first rectangle.</param>
        /// <param name="second">The second rectangle.</param>
        /// <returns>
        ///     <c>true</c> if the values of the
        ///     <see cref="X" />, <see cref="Y"/>, <see cref="Width"/> and <see cref="Height" /> fields of the two <see cref="RectangleF" /> structures
        ///     are unequal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(RectangleF first, RectangleF second)
        {
            return !(first == second);
        }

        /// <summary>
        ///     Indicates whether this <see cref="RectangleF" /> is equal to another <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        ///     <c>true</c> if this <see cref="RectangleF" /> is equal to the <paramref name="rectangle" />; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(RectangleF rectangle)
        {
            return Equals(ref rectangle);
        }

        /// <summary>
        ///     Indicates whether this <see cref="RectangleF" /> is equal to another <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        ///     <c>true</c> if this <see cref="RectangleF" /> is equal to the <paramref name="rectangle" />; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ref RectangleF rectangle)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return X == rectangle.X && Y == rectangle.Y && Width == rectangle.Width && Height == rectangle.Height;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        /// <summary>
        ///     Returns a value indicating whether this <see cref="RectangleF" /> is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to make the comparison with.</param>
        /// <returns>
        ///     <c>true</c> if this <see cref="RectangleF" /> is equal to <paramref name="obj" />; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is RectangleF && Equals((RectangleF)obj);
        }

        /// <summary>
        ///     Returns a hash code of this <see cref="RectangleF" /> suitable for use in hashing algorithms and data
        ///     structures like a hash table.
        /// </summary>
        /// <returns>
        ///     A hash code of this <see cref="RectangleF" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Performs an implicit conversion from a <see cref="Rectangle" /> to a <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        ///     The resulting <see cref="RectangleF" />.
        /// </returns>
        public static implicit operator RectangleF(Rectangle rectangle)
        {
            return new RectangleF
            {
                X = rectangle.X,
                Y = rectangle.Y,
                Width = rectangle.Width,
                Height = rectangle.Height
            };
        }

        /// <summary>
        ///     Performs an implicit conversion from a <see cref="Rectangle" /> to a <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>
        ///     The resulting <see cref="RectangleF" />.
        /// </returns>
        /// <remarks>
        ///     <para>A loss of precision may occur due to the truncation from <see cref="float" /> to <see cref="int" />.</para>
        /// </remarks>
        public static explicit operator Rectangle(RectangleF rectangle)
        {
            return new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this <see cref="RectangleF" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this <see cref="RectangleF" />.
        /// </returns>
        public override string ToString()
        {
            return $"{{X: {X}, Y: {Y}, Width: {Width}, Height: {Height}";
        }
    }
}
