using Fitamas.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using LibTessDotNet;
using System;

namespace Fitamas.Math2D
{
    public enum CounterType
    {
        Clockwise,
        CounterClockwise
    }

    public static class Triangulator
    {
        public static void Process(Vector2[] shapes, out Vector2[] verts, out int[] ind, CounterType type = CounterType.CounterClockwise)
        {
            Process(new Vector2[][] { shapes }, out verts, out ind, type);
        }

        public static void Process(Vector2[][] shapes, out Vector2[] verts, out int[] ind, CounterType type = CounterType.CounterClockwise)
        {
            ContourOrientation orientation;
            if (type == CounterType.CounterClockwise)
            {
                orientation = ContourOrientation.CounterClockwise;
            }
            else
            {
                orientation = ContourOrientation.Clockwise;
            }
            

            Tess tess = new Tess();

            foreach (var shape in shapes)
            {
                tess.AddContour(shape.ToContourVertices(), orientation);
            }

            tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

            if (tess.Vertices != null)//(tess.NoEmptyPolygons)
            {
                verts = tess.Vertices.ToVector2();
                ind = tess.Elements;
            }
            else
            {
                verts = new Vector2[0];
                ind = new int[0];
            }
        }

        public static Vector2[] ToVector2(this ContourVertex[] vertices)
        {
            Vector2[] vectors = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                vectors[i] = vertices[i].Position.ToVector2();
            }

            return vectors;
        }

        public static ContourVertex[] ToContourVertices(this Vector2[] points)
        {
            ContourVertex[] vertices = new ContourVertex[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                vertices[i].Position = points[i].ToVec3();
            }

            return vertices;
        }

        public static Vec3 ToVec3(this Vector2 point)
        {
            return new Vec3(point.X, point.Y, 0);
        }

        public static Vector2 ToVector2(this Vec3 vec)
        {
            return new Vector2(vec.X, vec.Y);
        }
    }
}
