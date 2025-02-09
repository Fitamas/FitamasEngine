using Fitamas.Graphics;
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
            Matrix view = Camera.Main.ViewportScaleMatrix;
            Matrix projection = Camera.GetProjectionMatrix();

            primitiveBatch.Begin(ref projection, ref view);

            RenderComponent(component);

            primitiveBatch.End();
        }

        private void RenderComponent(GUIComponent component)
        {
            Color color = component.Enable ? EnableColor : DisableColor;
            Rectangle rectangle = component.Rectangle;
            Vector2 position = rectangle.Location.ToVector2();
            Vector2 centerPosition = rectangle.Location.ToVector2() + rectangle.Size.ToVector2() * component.Pivot;
            primitiveDrawing.DrawRectangle(position, rectangle.Width, rectangle.Height, color);
            primitiveDrawing.DrawSolidCircle(centerPosition, centerScale, color);

            foreach (var child in component.ChildrensComponent)
            {
                RenderComponent(child);
            }
        }
    }
}
