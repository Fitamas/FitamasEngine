using Fitamas.DebugTools;
using Fitamas.Graphics;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;

namespace Fitamas.UserInterface
{
    public static class GUIDebug
    {
        public static bool Active = false;
        public static float centerScale = 4;

        public static Color EnableColor = Color.DarkGreen;
        public static Color DisableColor = Color.Red;

        public static void Render(GUIComponent component)
        {
            Matrix view = Camera.Current.ViewportScaleMatrix;
            Matrix projection = Camera.Current.GetProjectionMatrix();

            Gizmos.End();
            Gizmos.Begin(ref projection, ref view);

            RenderComponent(component, EnableColor);
        }

        private static void RenderComponent(GUIComponent component, Color color)
        {
            if (!component.Enable)
            {
                color = DisableColor;
            }

            Rectangle rectangle = component.Rectangle;
            Vector2 pivot = component.Pivot;
            Vector2 position = rectangle.Center.ToVector2();
            Vector2 size = rectangle.Size.ToVector2();
            Gizmos.DrawRectangle(position, 0, size, Color.Blue);

            Vector2 centerPosition = rectangle.Location.ToVector2() + new Vector2(rectangle.Width * pivot.X, rectangle.Height * pivot.Y);
            Gizmos.DrawCircle(centerPosition, centerScale, color);

            Rectangle rectangle1 = component.VisibleRectangle;
            Vector2 position1 = rectangle1.Center.ToVector2();
            Vector2 size1 = rectangle1.Size.ToVector2();
            Gizmos.DrawRectangle(position1, 0, size1, color);

            foreach (var child in component.ChildrensComponent)
            {
                RenderComponent(child, color);
            }
        }
    }
}
