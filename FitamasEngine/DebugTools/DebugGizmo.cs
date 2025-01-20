using Fitamas.Editor;
using Fitamas.Entities;
using Fitamas.Graphics;
using Fitamas.Math2D;
using Fitamas.Physics;
using Fitamas.Physics.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;

namespace Fitamas.DebugTools
{
    public static class DebugGizmo
    {
        private static PrimitiveBatch primitiveBatch;
        private static PrimitiveDrawing primitiveDrawing;

        public static Vector2 AnchorSize = new Vector2(1, 0.05f);
        public static Vector2 HandleSize = new Vector2(0.3f, 0.3f);

        private static void Create(GraphicsDevice graphicsDevice)
        {
            primitiveBatch = new PrimitiveBatch(graphicsDevice);
            primitiveDrawing = new PrimitiveDrawing(primitiveBatch);
        }

        public static void BeginBatch(GraphicsDevice graphicsDevice)
        {
            if (primitiveBatch == null)
            {
                Create(graphicsDevice);
            }

            if (Camera.Current == null)
            {
                return;
            }

            Matrix view = Camera.Current.GetViewMatrix();
            Matrix projection = Camera.GetProjectionMatrix();
            
            primitiveBatch.Begin(ref projection, ref view);
        }

        public static void EndBatch()
        {
            if (Camera.Current == null)
            {
                return;
            }

            primitiveBatch.End();
        }

        public static void DrawAnchor(Transform transform)
        {
            Color x = Color.Green;
            Color y = Color.Red;

            Vector2 position = transform.Position;
            float rotation = transform.Rotation;

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

        public static void DrawSprite(Transform transform, SpriteRender sprite)
        {
            Vector2[] points = sprite.GetAbsoluteSpriteRectangle(transform);
            primitiveDrawing.DrawPolygon(Vector2.Zero, points, Color.Blue);
        }

        public static void DrawRenderMesh(Transform transform, Mesh mesh)
        {
            for (int i = 0; i < mesh.TriangleCount; i++)
            {
                Vector2[] points = new Vector2[3];
                for (var j = 0; j < 3; j++)
                {
                    int index = mesh.Ind[i * 3 + j];
                    Vector2 pos = mesh.Vertices[index];
                    points[j] = transform.ToAbsolutePosition(pos);
                }
                primitiveDrawing.DrawPolygon(Vector2.Zero, points, Color.Blue);
            }
        }

        public static void DrawCollider(Transform transform, Collider collider)
        {
            Color color = Color.GreenYellow;

            switch (collider.ColliderType)
            {
                case ColliderType.Box:
                    {
                        RectangleF rectangle = collider.Rectangle;
                        Vector2[] points = rectangle.GetCorners();

                        for (int i = 0; i < points.Length; i++)
                        {
                            points[i] = transform.ToAbsolutePosition(points[i] - rectangle.Center + collider.Offset);
                        }

                        primitiveDrawing.DrawPolygon(Vector2.Zero, points, color);
                        break;
                    }
                case ColliderType.Circle:
                    {
                        primitiveDrawing.DrawCircle(transform.Position, collider.Radius, color);
                        break;
                    }
                case ColliderType.Polygon:
                    {
                        if (collider.CompositeShape != null)
                        {
                            foreach (var polygon in collider.CompositeShape)
                            {
                                Vector2[] points = polygon.ToArray();
                                for (int i = 0; i < points.Length; i++)
                                {
                                    points[i] = collider.ToAbsolutePosition(points[i]);
                                }
                                primitiveDrawing.DrawPolygon(Vector2.Zero, points, color);
                            }
                        }
                        break;
                    }
                case ColliderType.Capsule:
                    {
                        Vector2 scale = collider.Scale;
                        RectangleF rectangle = new RectangleF(Vector2.Zero, new Vector2(scale.X * 2, scale.Y));
                        Vector2[] points = rectangle.GetCorners();
                        for (int i = 0; i < points.Length; i++)
                        {
                            points[i] = collider.ToAbsolutePosition(points[i] - rectangle.Center);
                        }
                        primitiveDrawing.DrawPolygon(Vector2.Zero, points, color);

                        Vector2 posUp = collider.ToAbsolutePosition(new Vector2(0, scale.Y / 2));
                        Vector2 posDown = collider.ToAbsolutePosition(new Vector2(0, -scale.Y / 2));

                        primitiveDrawing.DrawCircle(posUp, scale.X, color);
                        primitiveDrawing.DrawCircle(posDown, scale.X, color);
                        break;
                    }
            }
        }

        public static void DrawCharacter(Transform transform, Character character)
        {
            Vector2 position = transform.Position;
            primitiveDrawing.DrawLine(position, position + character.currentVelocity.NormalizeF(), Color.Yellow, 0.05f);
        }

        public static void DrawCamera(Camera camera)
        {
            RectangleF rectangle = camera.WorldBounds();

            primitiveDrawing.DrawRectangle(rectangle.TopLeft, rectangle.Width, rectangle.Height, Color.Yellow);
        }
    }
}
