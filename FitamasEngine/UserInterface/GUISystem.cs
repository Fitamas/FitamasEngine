using Assimp;
using Fitamas.Container;
using Fitamas.Entities;
using Fitamas.Extended.Entities;
using Fitamas.Input;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Serializeble;
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

        //TODO window and popup
        private SerializebleLayout layout;
        private GraphicsDevice graphics;
        private GUIRenderBatch guiRender;
        private DIContainer container;

        public IKeyboardEvent keyboardSubscriber { get; set; }
        public GUIComponent onMouse { get; set; }

        private List<IMouseEvent> clickedMouse = new List<IMouseEvent>();
        private List<IDragMouseEvent> dragMouse = new List<IDragMouseEvent>();

        public bool isKeyboardInput => keyboardSubscriber != null;
        public GraphicsDevice GraphicsDevice => graphics;
        public GUIRenderBatch GUIRender => guiRender;
        public SerializebleLayout CurrentLayout => layout;
        public DIContainer Container => container;

        public bool MouseOnGUI
        {
            get
            {
                return onMouse != null;
            }
        }

        public GUISystem(GraphicsDevice graphicsDevice, DIContainer container)
        {
            this.container = new DIContainer(container);
            this.container.RegisterInstance("gui_system", this);

            GUIDebug.Create(graphicsDevice);

            graphics = graphicsDevice;
            guiRender = new GUIRenderBatch(graphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            LoadScreen(DefoultLayout);
        }

        public void Initialize(GameWorld world)
        {
            InputSystem.keyboard.KeyTyped += (s, e) =>
            {
                keyboardSubscriber?.OnKeyDown(e);
            };
            InputSystem.keyboard.KeyReleased += (s, e) =>
            {
                keyboardSubscriber?.OnKeyUP(e);
            };
            InputSystem.keyboard.KeyPressed += (s, e) =>
            {
                keyboardSubscriber?.OnKey(e);
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
            onMouse = null;

            RaycastAll(mousePosition, result);

            foreach (var component in result)
            {
                if (onMouse != null)
                {
                    if (component.Layer > onMouse.Layer)
                    {
                        onMouse = component;
                    }
                }
                else
                {
                    onMouse = component;
                }
            }

            if (layout != null)
            {
                layout.Scripting.Update();

                layout.Canvas.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (layout != null)
            {

                GUIRender.Begin();

                layout.Canvas.Draw(gameTime);

                GUIRender.End();

                if (GUIDebug.DebugModeOn)
                {
                    GUIDebug.Render(layout.Canvas);
                }
            }
        }

        public void RaycastAll(Point point, List<GUIComponent> result)
        {
            if (layout != null)
            {
                layout.Canvas.RaycastAll(point, result);
            }
        }

        public void AddComponent(GUIComponent component)
        {
            if (layout != null)
            {
                layout.Canvas.AddChild(component);
            }
        }

        public void RemoveComponent(GUIComponent component)
        {
            if (layout != null)
            {
                layout.Canvas.RemoveChild(component);
            }
        }

        public GUIComponent GetComponentFromId(string id)
        {
            if (layout != null)
            {
                return layout.Canvas.GetComponentFromId(id);
            }

            return null;
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
                layout?.CloseScreen();
                layout = selectScreen;
                layout.OpenScreen(this);
            }
        }

        public void Dispose()
        {
            layout?.CloseScreen();
            layout = null;
        }
    }
}