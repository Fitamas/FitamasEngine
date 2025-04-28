using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Input;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface
{
    public class GUISystem : ILoadContentSystem, IUpdateSystem, IDrawSystem
    {
        private GraphicsDevice graphics;
        private GUIRenderBatch render;
        private GUIDebug debug;
        private GUICanvas canvas;
        private GUIRoot root;
        private List<GUIComponent> onMouse = new List<GUIComponent>();

        public GUIMouse Mouse { get; }
        public GUIKeyboard Keyboard { get; }

        public bool MouseOnGUI => onMouse.Count != 0;
        public GraphicsDevice GraphicsDevice => graphics;
        public GUIRenderBatch Render => render;
        public GUICanvas Canvas => canvas;
        public GUIRoot Root => root;

        public GUISystem(GraphicsDevice graphicsDevice)
        {
            debug = new GUIDebug(graphicsDevice);

            graphics = graphicsDevice;
            render = new GUIRenderBatch(graphicsDevice);

            canvas = new GUICanvas();
            root = new GUIRoot();
            root.SetAlignment(GUIAlignment.Stretch);
            canvas.AddChild(root);

            Mouse = new GUIMouse();
            Keyboard = new GUIKeyboard();
        }

        public void Initialize(GameWorld world)
        {
            canvas.Init(this);
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {
            Mouse.Update(gameTime);
            Keyboard.Update(gameTime);

            Point mousePosition = InputSystem.mouse.MousePosition;

            GUIComponent onMouseOld = onMouse.LastOrDefault();
            onMouse.Clear();
            RaycastAll(mousePosition, onMouse);
            GUIComponent onMouseNew = onMouse.LastOrDefault();

            if (onMouseOld != onMouseNew)
            {
                if (onMouseOld != null)
                {
                    onMouseOld.IsMouseOver = false;
                }

                if (onMouseNew != null)
                {
                    onMouseNew.IsMouseOver = true;
                }

                Mouse.MouseOver = onMouseNew;
            }
        }

        public void Draw(GameTime gameTime)
        {
            Rectangle rectangle = new Rectangle(new Point(0, 0), Render.GetViewportSize());
            canvas.Draw(gameTime, new GUIContextRender(rectangle));

            if (GUIDebug.Active)
            {
                debug.Render(canvas);
            }
        }

        public void RaycastAll(Point point, List<GUIComponent> result)
        {
            canvas.RaycastAll(point, result);
        }

        public void AddComponent(GUIComponent component)
        {
            root.Screen.AddChild(component);
        }

        public GUIComponent GetComponentFromId(string id)
        {
            return canvas.GetComponentFromName(id);
        }

        public void Dispose()
        {

        }
    }
}