using Assimp;
using Fitamas.Container;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Input;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Scripting;
using Fitamas.UserInterface.Serializeble;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface
{
    public class GUISystem : ILoadContentSystem, IUpdateSystem, IDrawSystem
    {
        public const string DefoultLayout = "Layouts\\MainMenu.xml";

        private GraphicsDevice graphics;
        private GUIRenderBatch guiRender;
        private DIContainer container;
        private GUIDebug debug;
        private GUIRoot root;
        private GUIScripting scripting;

        public GUIComponent Focused { get; set; }
        public GUIComponent OnMouse { get; set; }

        private List<IMouseEvent> clickedMouse = new List<IMouseEvent>();
        private List<IDragMouseEvent> dragMouse = new List<IDragMouseEvent>();

        public bool IsFocused => Focused != null;
        public GraphicsDevice GraphicsDevice => graphics;
        public GUIRenderBatch Render => guiRender;
        public DIContainer Container => container;
        public GUIRoot Root => root;

        public bool MouseOnGUI
        {
            get
            {
                return OnMouse != null;
            }
        }

        public GUISystem(GraphicsDevice graphicsDevice, DIContainer container)
        {
            this.container = container;
            this.container.RegisterInstance("gui_system", this);

            debug = new GUIDebug(graphicsDevice);
            root = new GUIRoot();

            graphics = graphicsDevice;
            guiRender = new GUIRenderBatch(graphicsDevice);

            root.Init(this);
        }

        public void LoadContent(ContentManager content)
        {
            LoadScreen(DefoultLayout);
        }

        public void Initialize(GameWorld world)
        {
            InputSystem.keyboard.KeyTyped += (s, e) =>
            {
                //keyboardSubscriber?.OnKeyDown(e);
            };
            InputSystem.keyboard.KeyReleased += (s, e) =>
            {
                //keyboardSubscriber?.OnKeyUP(e);
            };
            InputSystem.keyboard.KeyPressed += (s, e) =>
            {
                //keyboardSubscriber?.OnKey(e);
            };
            InputSystem.mouse.MouseDown += (s, e) =>
            {
                foreach (var clicked in clickedMouse.ToList())
                {
                    clicked.OnClickedMouse(e);
                }
            };
            InputSystem.mouse.MouseUp += (s, e) =>
            {
                foreach (var release in clickedMouse.ToList())
                {
                    release.OnReleaseMouse(e);
                }
            };
            InputSystem.mouse.MouseWheelMoved += (s, e) =>
            {
                foreach (var scroll in clickedMouse.ToList())
                {
                    scroll.OnScrollMouse(e);
                }
            };
            InputSystem.mouse.MouseDragStart += (s, e) =>
            {
                foreach (var drag in dragMouse.ToList())
                {
                    drag.OnStartDragMouse(e);
                }
            };
            InputSystem.mouse.MouseDrag += (s, e) =>
            {
                foreach (var drag in dragMouse.ToList())
                {
                    drag.OnDragMouse(e);
                }
            };
            InputSystem.mouse.MouseDragEnd += (s, e) =>
            {
                foreach (var drag in dragMouse.ToList())
                {
                    drag.OnEndDragMouse(e);
                }
            };
        }

        public void Update(GameTime gameTime)
        {
            Point mousePosition = InputSystem.mouse.MousePosition;
            List<GUIComponent> result = new List<GUIComponent>();

            RaycastAll(mousePosition, result);

            GUIComponent onMouse = result.FirstOrDefault();

            if (OnMouse != onMouse)
            {
                if (OnMouse != null)
                {
                    OnMouse.IsMouseOver = false;
                }

                OnMouse = onMouse;

                if (OnMouse != null)
                {
                    OnMouse.IsMouseOver = true;
                }
            }

            scripting?.Update();

            root.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            root.Draw(gameTime);

            if (GUIDebug.Active)
            {
                debug.Render(root);
            }
        }

        public void RaycastAll(Point point, List<GUIComponent> result)
        {
            root.RaycastAll(point, result);
        }

        public void AddComponent(GUIComponent component)
        {
            root.AddComponent(component);
        }

        public GUIComponent GetComponentFromId(string id)
        {
            return root.GetComponentFromName(id);
        }

        public void SubscribeInput(GUIComponent component)
        {
            if (component is IMouseEvent clicked)
            {
                clickedMouse.Add(clicked);
            }

            if (component is IDragMouseEvent drag)
            {
                dragMouse.Add(drag);
            }
        }

        public void UnsubscribeInput(GUIComponent component)
        {
            if (component is IMouseEvent clicked)
            {
                clickedMouse.Remove(clicked);
            }

            if (component is IDragMouseEvent drag)
            {
                dragMouse.Remove(drag);
            }
        }

        public void LoadScreen(string name)
        {
            SerializebleLayout selectScreen = Resources.Load<SerializebleLayout>(name);

            if (selectScreen != null)
            {
                //if (layout != null)
                {
                    // TODO remove all components

                    //layout.Scripting.OnClose();
                    //foreach (var component in selectScreen.Components)
                    //{
                    //    RemoveComponent(component);
                    //}
                }

                //layout = selectScreen;
                scripting = selectScreen.Scripting;

                foreach (var component in selectScreen.Components)
                {
                    AddComponent(component);
                }
                selectScreen.Scripting.OnOpen(this);
            }
        }

        public void Dispose()
        {

        }
    }
}