using Fitamas.Core;
using Fitamas.Editor;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Math;
using Fitamas.Physics;
using Microsoft.Xna.Framework;

namespace Fitamas.DebugTools
{
    public static class Gizmos
    {
        private static PrimitiveBatch primitiveBatch;
        private static PrimitiveDrawing primitiveDrawing;

        public static Vector2 AnchorSize = new Vector2(1, 0.05f);
        public static Vector2 HandleSize = new Vector2(0.3f, 0.3f);

        internal static void Begin()
        {
            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.Current.GetProjectionMatrix();

            Begin(ref projection, ref view);
        }

        internal static void Begin(ref Matrix projection, ref Matrix view)
        {
            if (primitiveBatch == null)
            {
                primitiveBatch = new PrimitiveBatch(GameEngine.Instance.GraphicsDevice);
                primitiveDrawing = new PrimitiveDrawing(primitiveBatch);
            }

            primitiveBatch.Begin(ref projection, ref view);
        }

        internal static void End()
        {
            primitiveBatch.End();
        }

        public static void DrawAnchor(Vector2 position, float rotation)
        {
            Color x = Color.Green;
            Color y = Color.Red;

            primitiveDrawing.DrawSegment(position, position + MathV.Rotate(new Vector2(0, AnchorSize.X), rotation), y);

            primitiveDrawing.DrawSegment(position, position + MathV.Rotate(new Vector2(AnchorSize.X, 0), rotation), x);
        }

        public static void DrawAnchor(Vector2 position, float rotation, Handle handle = Handle.none)
        {
            Color x = Color.Green;
            Color y = Color.Red;
            Color xy = Color.Blue;

            switch (handle)
            {
                case Handle.none:
                    break;
                case Handle.XY:
                    xy = Color.Yellow;
                    x = Color.Yellow;
                    y = Color.Yellow;
                    break;
                case Handle.X:
                    x = Color.Yellow;
                    break;
                case Handle.Y:
                    y = Color.Yellow;
                    break;
            }

            primitiveDrawing.DrawLine(position + MathV.Rotate(new Vector2(0, HandleSize.Y / 2), rotation), 
                                      position + MathV.Rotate(new Vector2(HandleSize.X, HandleSize.Y / 2), rotation), xy, HandleSize.Y);

            primitiveDrawing.DrawLine(position, position + MathV.Rotate(new Vector2(0, AnchorSize.X), rotation), y, AnchorSize.Y);

            primitiveDrawing.DrawLine(position, position + MathV.Rotate(new Vector2(AnchorSize.X, 0), rotation), x, AnchorSize.Y);
        }

        public static void DrawRectangle(Vector2 position, float rotation, Vector2 size, Color color)
        {
            Vector2 vector1 = MathV.Rotate(new Vector2(size.X / 2, size.Y / 2), rotation);
            Vector2 vector2 = MathV.Rotate(new Vector2(-size.X / 2, size.Y / 2), rotation);
            Vector2[] points = [vector1, vector2, -vector1, -vector2];

            primitiveDrawing.DrawPolygon(position, points, color);
        }

        public static void DrawCircle(Vector2 position, float radius, Color color)
        {
            primitiveDrawing.DrawCircle(position, radius, color);
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            primitiveDrawing.DrawPolygon(Vector2.Zero, [start, end], color);
        }

        public static void DrawPolygon(Vector2 position, float rotation, MeshComponent mesh, Color color)
        {
            Matrix matrix = Matrix.CreateRotationZ(rotation) * 
                            Matrix.CreateTranslation(position.X, position.Y, 0);

            for (int i = 0; i < mesh.TriangleCount; i++)
            {
                Vector2[] points = new Vector2[3];
                for (var j = 0; j < 3; j++)
                {
                    int index = mesh.Ind[i * 3 + j];
                    Vector2 pos = mesh.Vertices[index];

                    points[j] = Vector2.Transform(pos, matrix);
                }
                primitiveDrawing.DrawPolygon(Vector2.Zero, points, color);
            }
        }

        public static void DrawPolygon(Vector2 position, float rotation, Vector2[] points, Color color)
        {
            Matrix matrix = Matrix.CreateRotationZ(rotation) *
                Matrix.CreateTranslation(position.X, position.Y, 0);

            Vector2[] result = new Vector2[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                result[i] = Vector2.Transform(points[i], matrix);
            }
            primitiveDrawing.DrawPolygon(Vector2.Zero, result, color);
        }
    }
}