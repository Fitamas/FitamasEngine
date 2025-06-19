using Fitamas.Graphics;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public class GUIDebug
    {
        private static PrimitiveBatch primitiveBatch;
        private static PrimitiveDrawing primitiveDrawing;

        public static bool Active = false;
        public static float centerScale = 4;

        public static Color EnableColor = Color.DarkGreen;
        public static Color DisableColor = Color.Red;

        public GUIDebug(GraphicsDevice graphicsDevice)
        {
            primitiveBatch = new PrimitiveBatch(graphicsDevice);
            primitiveDrawing = new PrimitiveDrawing(primitiveBatch);
        }

        public void Render(GUIComponent component)
        {
            if (Camera.Current == null)
            {
                return;
            }

            Matrix view = Camera.Current.ViewportScaleMatrix;
            Matrix projection = Camera.Current.GetProjectionMatrix();

            primitiveBatch.Begin(ref projection, ref view);

            RenderComponent(component, EnableColor);

            primitiveBatch.End();
        }

        private void RenderComponent(GUIComponent component, Color color)
        {
            if (!component.Enable)
            {
                color = DisableColor;
            }

            Rectangle rectangle = component.Rectangle;
            Vector2 pivot = component.Pivot;
            Vector2 position = new Vector2(rectangle.X, rectangle.Y);
            primitiveDrawing.DrawRectangle(position, rectangle.Width, rectangle.Height, Color.Blue);

            Vector2 centerPosition = position + new Vector2(rectangle.Width * pivot.X, rectangle.Height * pivot.Y);
            primitiveDrawing.DrawSolidCircle(centerPosition, centerScale, color);

            Rectangle rectangle1 = component.VisibleRectangle;
            Vector2 position1 = new Vector2(rectangle1.X, rectangle1.Y);
            primitiveDrawing.DrawRectangle(position1, rectangle1.Width, rectangle1.Height, color);

            foreach (var child in component.ChildrensComponent)
            {
                RenderComponent(child, color);
            }
        }
    }
}
