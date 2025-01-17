using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public static class GUIDebug
    {
        private static PrimitiveBatch primitiveBatch;
        private static PrimitiveDrawing primitiveDrawing;

        public static bool DebugModeOn = false;
        public static float centerScale = 4;

        public static Color EnableColor = Color.DarkGreen;
        public static Color DisableColor = Color.Red;

        public static void Create(GraphicsDevice graphicsDevice)
        {
            primitiveBatch = new PrimitiveBatch(graphicsDevice);
            primitiveDrawing = new PrimitiveDrawing(primitiveBatch);
        }

        public static void Render(GUIComponent component)
        {
            BeginBatch();

            RenderComponent(component);

            EndBatch();
        }

        private static void RenderComponent(GUIComponent component)
        {
            Color color = component.Enable ? EnableColor : DisableColor;
            Rectangle rectangle = component.Rectangle;
            Vector2 position = rectangle.Location.ToVector2();
            Vector2 centerPosition = rectangle.Center.ToVector2();
            primitiveDrawing.DrawRectangle(position, rectangle.Width, rectangle.Height, color);
            primitiveDrawing.DrawSolidCircle(centerPosition, centerScale, color);

            foreach (var child in component.ChildrensComponent)
            {
                RenderComponent(child);
            }
        }

        public static void BeginBatch()
        {
            Matrix view = Camera.Main.ViewportScaleMatrix;
            Matrix projection = Camera.GetProjectionMatrix();

            primitiveBatch.Begin(ref projection, ref view);
        }

        public static void EndBatch()
        {
            primitiveBatch.End();
        }
    }
}
