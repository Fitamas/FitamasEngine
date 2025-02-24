﻿using Fitamas.Graphics;
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
            Vector2 pivot = component.Pivot;
            Vector2 position = new Vector2(rectangle.X, (int)Camera.Current.VirtualSize.Y - rectangle.Y);
            primitiveDrawing.DrawRectangle(position - new Vector2(0, rectangle.Height), rectangle.Width, rectangle.Height, Color.Blue);

            Rectangle rectangle1 = component.VisibleRectangle;
            Vector2 position1 = new Vector2(rectangle1.X, (int)Camera.Current.VirtualSize.Y - rectangle1.Y);
            primitiveDrawing.DrawRectangle(position1 - new Vector2(0, rectangle1.Height), rectangle1.Width, rectangle1.Height, color);

            Vector2 centerPosition = position + new Vector2(rectangle.Width * pivot.X, -rectangle.Height * pivot.Y);
            primitiveDrawing.DrawSolidCircle(centerPosition, centerScale, color);

            foreach (var child in component.ChildrensComponent)
            {
                RenderComponent(child);
            }
        }
    }
}
