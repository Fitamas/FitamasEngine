using Fitamas.DebugTools;
using Fitamas.Graphics;
using Fitamas.Math;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet
{
    public enum Handle
    {
        none,
        X,
        Y,
        XY
    }

    public static class HandleUtils
    {
        private static Vector2 offset = Vector2.Zero;
        private static Handle handle = Handle.none;

        public static Vector2 HandleSize = new Vector2(0.3f, 0.3f);

        public static Handle Handle => handle;

        public static Vector2 PositionHandle(Vector2 position, float rotation, Vector2 mousePosition)
        {
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                handle = GetHandle(position, rotation, mousePosition, out Vector2 localMousePosition);
                offset = localMousePosition;
            }
            else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
            {
                handle = Handle.none;
            }
            else if (ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                DrawAnchor(position, rotation, handle);
                return PlaneHandle(position, rotation, mousePosition, offset, handle);
            }
            else
            {
                handle = GetHandle(position, rotation, mousePosition, out Vector2 localMousePosition);
                DrawAnchor(position, rotation, handle);
            }

            return position;
        }

        public static Handle GetHandle(Vector2 position, float rotation, Vector2 mousePosition, out Vector2 localMousePosition)
        {
            Matrix matrix = Matrix.CreateRotationZ(rotation);
            matrix *= Matrix.CreateTranslation(position.X, position.Y, 0);

            Matrix.Invert(ref matrix, out Matrix invertMatrix);
            Vector2.Transform(ref mousePosition, ref invertMatrix, out localMousePosition);

            return GetHandle(localMousePosition);
        }

        public static Handle GetHandle(Vector2 localMousePosition)
        {
            Vector2 size = Gizmos.AnchorSize;
            Vector2 handleSize = HandleSize;
            RectangleF rectangleXY = new RectangleF(new Vector2(), handleSize);
            RectangleF rectangleX = new RectangleF(new Vector2(0, -size.Y / 2), new Vector2(size.X, size.Y * 2));
            RectangleF rectangleY = new RectangleF(new Vector2(-size.Y / 2, 0), new Vector2(size.Y * 2, size.X));

            if (rectangleXY.Contains(localMousePosition))
            {
                return Handle.XY;
            }
            else if (rectangleX.Contains(localMousePosition))
            {
                return Handle.X;
            }
            else if (rectangleY.Contains(localMousePosition))
            {
                return Handle.Y;
            }

            return Handle.none;
        }

        public static Vector2 PlaneHandle(Vector2 position, float rotation, Vector2 mousePosition, Vector2 offset, Handle handle)
        {
            switch (handle)
            {
                case Handle.XY:
                    return mousePosition - MathV.Rotate(offset, rotation);
                case Handle.X:
                    Vector2 direction = MathV.Rotate(new Vector2(1, 0), rotation);
                    return MathV.ProjectOnTo(position, position + direction, mousePosition - MathV.Rotate(offset, rotation));
                case Handle.Y:
                    direction = MathV.Rotate(new Vector2(0, 1), rotation);
                    return MathV.ProjectOnTo(position, position + direction, mousePosition - MathV.Rotate(offset, rotation));
            }

            return position;
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

            Gizmos.DrawLine(position + MathV.Rotate(new Vector2(0, HandleSize.Y / 2), rotation), 
                            position + MathV.Rotate(new Vector2(HandleSize.X, HandleSize.Y / 2), rotation), xy, HandleSize.Y);

            Gizmos.DrawLine(position, position + MathV.Rotate(new Vector2(0, Gizmos.AnchorSize.X), rotation), y, Gizmos.AnchorSize.Y);

            Gizmos.DrawLine(position, position + MathV.Rotate(new Vector2(Gizmos.AnchorSize.X, 0), rotation), x, Gizmos.AnchorSize.Y);
        }
    }
}
