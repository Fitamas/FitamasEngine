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
using Microsoft.Xna.Framework.Graphics;
using System;
using Fitamas.Math;

namespace Fitamas.Graphics
{
    public class PrimitiveDrawing
    {
        private PrimitiveBatch _primitiveBatch;

        public const int CircleSegments = 32;

        public PrimitiveDrawing(PrimitiveBatch primitiveBatch)
        {
            _primitiveBatch = primitiveBatch;
        }

        public void DrawPoint(Vector2 center, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            //Add two points or the PrimitiveBatch acts up
            _primitiveBatch.AddVertex(center, color, PrimitiveType.LineList);
            _primitiveBatch.AddVertex(center, color, PrimitiveType.LineList);
        }

        public void DrawRectangle(Vector2 location, float width, float height, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            Vector2[] rectVerts = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(width, 0),
                new Vector2(width, height),
                new Vector2(0, height)
            };

            //Location is offset here
            DrawPolygon(location, rectVerts, color);
        }

        public void DrawSolidRectangle(Vector2 location, float width, float height, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            Vector2[] rectVerts = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(width, 0),
                new Vector2(width, height),
                new Vector2(0, height)
            };

            DrawSolidPolygon(location, rectVerts, color);
        }

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            const double increment = System.Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            for (int i = 0; i < CircleSegments; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)System.Math.Cos(theta), (float)System.Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)System.Math.Cos(theta + increment), (float)System.Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v1, color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(v2, color, PrimitiveType.LineList);

                theta += increment;
            }
        }

        public void DrawSolidCircle(Vector2 center, float radius, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            const double increment = System.Math.PI * 2.0 / CircleSegments;
            double theta = 0.0;

            Color colorFill = color * 0.5f;

            Vector2 v0 = center + radius * new Vector2((float)System.Math.Cos(theta), (float)System.Math.Sin(theta));
            theta += increment;

            for (int i = 1; i < CircleSegments - 1; i++)
            {
                Vector2 v1 = center + radius * new Vector2((float)System.Math.Cos(theta), (float)System.Math.Sin(theta));
                Vector2 v2 = center + radius * new Vector2((float)System.Math.Cos(theta + increment), (float)System.Math.Sin(theta + increment));

                _primitiveBatch.AddVertex(v0, colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v1, colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(v2, colorFill, PrimitiveType.TriangleList);

                theta += increment;
            }

            DrawCircle(center, radius, color);
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1f)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            Vector2 delta = end - start;

            float distance = Vector2.Distance(start, end);
            float angle = MathV.SignedAngle(new Vector2(1, 0), delta);

            float hight = thickness / 2;

            Vector2[] rectVerts = new Vector2[4]
            {
                new Vector2(0, -hight),
                new Vector2(distance, -hight),
                new Vector2(distance, hight),
                new Vector2(0, hight)
            };

            Matrix matrix = Matrix.Identity;

            matrix *= Matrix.CreateRotationZ(angle);

            for (int i = 0; i < 4; i++)
            {
                rectVerts[i] = Vector2.Transform(rectVerts[i], matrix);
            }

            DrawSolidPolygon(start, rectVerts, color);
        }

        public void DrawSegment(Vector2 start, Vector2 end, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            _primitiveBatch.AddVertex(start, color, PrimitiveType.LineList);
            _primitiveBatch.AddVertex(end, color, PrimitiveType.LineList);
        }

        public void DrawPolygon(Vector2 position, Vector2[] vertices, Color color, bool closed = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            int count = vertices.Length;

            for (int i = 0; i < count - 1; i++)
            {
                //translate the vertices according to the position passed
                _primitiveBatch.AddVertex(new Vector2(vertices[i].X + position.X, vertices[i].Y + position.Y), color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(new Vector2(vertices[i + 1].X + position.X, vertices[i + 1].Y + position.Y), color, PrimitiveType.LineList);
            }
            if (closed)
            {
                //TODO: verify closed is working as expected
                _primitiveBatch.AddVertex(new Vector2(vertices[count - 1].X + position.X, vertices[count - 1].Y + position.Y), color, PrimitiveType.LineList);
                _primitiveBatch.AddVertex(new Vector2(vertices[0].X + position.X, vertices[0].Y + position.Y), color, PrimitiveType.LineList);
            }
        }

        public void DrawSolidPolygon(Vector2 position, Vector2[] vertices, Color color, bool outline = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            int count = vertices.Length;

            if (count == 2)
            {
                DrawPolygon(position, vertices, color);
                return;
            }

            Color colorFill = color * (outline ? 0.5f : 1.0f);

            Vector2[] outVertices;
            int[] outIndices;
            Triangulator.Process(vertices, out outVertices, out outIndices);

            for (int i = 0; i < outIndices.Length - 2; i += 3)
            {
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i]].X + position.X, outVertices[outIndices[i]].Y + position.Y), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i + 1]].X + position.X, outVertices[outIndices[i + 1]].Y + position.Y), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i + 2]].X + position.X, outVertices[outIndices[i + 2]].Y + position.Y), colorFill, PrimitiveType.TriangleList);
            }

            if (outline)
                DrawPolygon(position, vertices, color);
        }

        public void DrawEllipse(Vector2 center, Vector2 radius, int sides, Color color)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            DrawPolygon(center, CreateEllipse(radius.X, radius.Y, sides), color);
        }

        public void DrawSolidEllipse(Vector2 center, Vector2 radius, int sides, Color color, bool outline = true)
        {
            if (!_primitiveBatch.IsReady())
                throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");

            Color colorFill = color * (outline ? 0.5f : 1.0f);

            Vector2[] vertices = CreateEllipse(radius.X, radius.Y, sides);

            Vector2[] outVertices;
            int[] outIndices;
            Triangulator.Process(vertices, out outVertices, out outIndices);

            for (int i = 0; i < outIndices.Length - 2; i += 3)
            {
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i]].X + center.X, outVertices[outIndices[i]].Y + center.Y), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i + 1]].X + center.X, outVertices[outIndices[i + 1]].Y + center.Y), colorFill, PrimitiveType.TriangleList);
                _primitiveBatch.AddVertex(new Vector2(outVertices[outIndices[i + 2]].X + center.X, outVertices[outIndices[i + 2]].Y + center.Y), colorFill, PrimitiveType.TriangleList);
            }
        }

        private static Vector2[] CreateEllipse(float rx, float ry, int sides)
        {
            var vertices = new Vector2[sides];

            var t = 0.0;
            var dt = 2.0 * System.Math.PI / sides;
            for (var i = 0; i < sides; i++, t += dt)
            {
                var x = (float)(rx * System.Math.Cos(t));
                var y = (float)(ry * System.Math.Sin(t));
                vertices[i] = new Vector2(x, y);
            }
            return vertices;
        }
    }
}
