using Fitamas.Core;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Input;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface
{
    public class GUIManager : IInitializable, Core.IUpdateable, Core.IDrawable, IDrawableGizmos
    {
        private GUIRenderBatch renderBatch;
        private GUICanvas canvas;
        private GUIRoot root;
        private List<GUIComponent> onMouse = new List<GUIComponent>();

        public GUIMouse Mouse { get; }
        public GUIKeyboard Keyboard { get; }

        public GUIRenderBatch RenderBatch => renderBatch;
        public GUICanvas Canvas => canvas;
        public GUIRoot Root => root;

        public GUIManager(GameEngine game)
        {
            renderBatch = new GUIRenderBatch(game.GraphicsDevice);

            canvas = new GUICanvas();
            root = new GUIRoot();
            root.SetAlignment(GUIAlignment.Stretch);
            canvas.AddChild(root);

            GUIRootBinder rootBinder = new GUIRootBinder(this);
            game.MainContainer.RegisterInstance(ApplicationKey.GUIRootBinder, rootBinder);

            GUIRootViewModel rootViewModel = new GUIRootViewModel();
            rootBinder.Bind(rootViewModel);
            game.MainContainer.RegisterInstance(ApplicationKey.GUIRootViewModel, rootViewModel);

            InputManager inputManager = game.InputManager;

            Mouse = new GUIMouse(inputManager.Mouse);
            Keyboard = new GUIKeyboard(inputManager.Keyboard);
        }
        
        public void Initialize()
        {
            canvas.Init(this);
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
            Rectangle rectangle = new Rectangle(Point.Zero, RenderBatch.GetViewportSize());
            canvas.Draw(gameTime, new GUIContextRender(rectangle, 1f));
        }

        public void DrawGizmos()
        {
            if (GUIDebug.Active)
            {
                GUIDebug.Render(canvas);
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
    }
}