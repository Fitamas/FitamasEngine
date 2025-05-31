using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Input;
using Fitamas.UserInterface.ViewModel;
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

        public GraphicsDevice GraphicsDevice => graphics;
        public GUIRenderBatch Render => render;
        public GUICanvas Canvas => canvas;
        public GUIRoot Root => root;

        public GUISystem(GameEngine game)
        {
            debug = new GUIDebug(game.GraphicsDevice);

            graphics = game.GraphicsDevice;
            render = new GUIRenderBatch(game.GraphicsDevice);

            canvas = new GUICanvas();
            root = new GUIRoot();
            root.SetAlignment(GUIAlignment.Stretch);
            canvas.AddChild(root);

            game.MainContainer.RegisterInstance(ApplicationKey.GUISystem, this);

            GUIRootBinder rootBinder = new GUIRootBinder(this);
            game.MainContainer.RegisterInstance(ApplicationKey.GUIRootBinder, rootBinder);

            GUIRootViewModel rootViewModel = new GUIRootViewModel();
            rootBinder.Bind(rootViewModel);
            game.MainContainer.RegisterInstance(ApplicationKey.GUIRootViewModel, rootViewModel);

            KeyboardListenerSettings settings = new KeyboardListenerSettings();
            KeyboardListener keyboard = new KeyboardListener(settings);
            MouseListener mouse = new MouseListener(Camera.Main.ViewportAdapter);

            InputListenerComponent component = new InputListenerComponent(game, mouse, keyboard);
            game.Components.Add(component);

            Mouse = new GUIMouse(mouse);
            Keyboard = new GUIKeyboard(keyboard);
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
            onMouse.Clear();
            Point mousePosition = Mouse.Position;
            RaycastAll(mousePosition, onMouse);
            Mouse.MouseOver = onMouse.LastOrDefault();
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