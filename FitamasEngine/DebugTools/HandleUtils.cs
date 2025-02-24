using Fitamas.Physics.Characters;
using Fitamas.DebugTools;
using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Editor
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

        public static Handle Handle => handle;

        //public static Vector2 PositionHandle(Vector2 position, float rotation, Vector2 mousePosition)
        //{
        //    Handle handle = GetHandle(position, rotation, mousePosition, out Vector2 localMousePosition);

        //    if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
        //    {
        //        HandleUtils.handle = handle;
        //        offset = localMousePosition;
        //    }
        //    if (ImGui.IsMouseDragging(ImGuiMouseButton.Left))
        //    {
        //        DebugGizmo.DrawAnchor(position, rotation, HandleUtils.handle);
        //        return PlaneHandle(position, rotation, mousePosition, offset, HandleUtils.handle);
        //    }
        //    else
        //    {
        //        DebugGizmo.DrawAnchor(position, rotation, handle);
        //    }

        //    return position;
        //}

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
            Vector2 size = DebugGizmo.AnchorSize;
            Vector2 handleSize = DebugGizmo.HandleSize;
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
    }
}
