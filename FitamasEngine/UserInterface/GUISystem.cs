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

        private static GUISystem instance;

        private SerializebleLayout layout;
        private GraphicsDevice graphics;
        private GUIRenderBatch guiRender;

        public IKeyboardEvent keyboardSubscriber { get; set; }
        public GUIComponent onMouse { get; set; }

        private List<IMouseEvent> clickedMouse = new List<IMouseEvent>();
        private List<IDragMouseEvent> dragMouse = new List<IDragMouseEvent>();

        public bool isKeyboardInput => keyboardSubscriber != null;
        public GraphicsDevice GraphicsDevice => graphics;
        public GUIRenderBatch GUIRender => guiRender;
        public SerializebleLayout CurrentLayout => layout;

        public bool MouseOnGUI
        {
            get
            {
                return onMouse != null;
            }
        }

        public GUISystem(GraphicsDevice graphicsDevice)
        {
            instance = this;

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
            GUIStyle.Init(world.Game);
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

            layout?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            layout?.Draw(gameTime);
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

        public static void LoadScreen(string name)
        {
            if (instance == null)
            {
                return;
            }

            SerializebleLayout selectScreen = Resources.Load<SerializebleLayout>(name);

            if (selectScreen != null)
            {
                instance.layout?.CloseScreen();
                instance.layout = selectScreen;
                instance.layout.OpenScreen(instance);
            }
        }

        public static SerializebleLayout GetActiveLayout()
        {
            if (instance == null)
            {
                return null;
            }

            return instance.layout;
        }

        public void Dispose()
        {

        }




        //private bool IsPointinOverUI()
        //{
        //    return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        //}

        //public void OnMouseDown()
        //{
        //    if (IsPointingOverUI() == false)
        //    {
        //    }

        //    if (IsPointingOverUIIgnors() == false)
        //    {
        //    }

        //}

        //private bool IsPointingOverUIIgnors()
        //{
        //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        //    pointerEventData.position = Input.mousePosition;

        //    List raycastResults = new List();
        //    EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        //    return raycastResults.Select(r => r.gameObject.TryGetComponent(out MouseUIClickthrough component) == false).Count() > 1;
        //}
    }

    //public class GameScreen
    //{
    //public static int itemId;
    //private EntityManager entityManager;

    //public static void OnInit(GUICanvas canvas)
    //{
    //GUIButton button = canvas.AddChild(GUI.CreateButton(new Rectangle(new Point(), new Point(200, 50)), "New game"));
    //button.Aligment = GUIAligment.Center;
    ////button.OnClicked = (e) => { LoadScreen<GameScreen>(); };

    //button = canvas.AddChild(GUI.CreateButton(new Rectangle(new Point(0, -60), new Point(200, 50)), "Level editor"));
    //button.Aligment = GUIAligment.Center;


    //GUIGridGroup group = canvas.AddChild(new GUIGridGroup(new Point(-10, -30), new Point(6, 2)));
    //group.CellSize = new Point(200, 50);
    //group.Spacing = new Point(20, 10);
    //group.Pivot = Vector2.Zero;


    //GUIButton button = group.AddChild(GUI.CreateButton(new Rectangle(new Point(), new Point(200, 50)), "None"));
    //button.Aligment = GUIAligment.Center;

    //GUIButton button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "None";
    //button.OnClicked += (e) => { itemId = 0; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Box";
    //button.OnClicked += (e) => { itemId = 1; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Board";
    ////button.OnClicked = (e) => { itemId = 9; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Pumpkin";
    ////button.OnClicked = (e) => { itemId = 2; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Log";
    ////button.OnClicked = (e) => { itemId = 3; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Mesh";
    ////button.OnClicked = (e) => { itemId = 4; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Destroy";
    ////button.OnClicked = (e) => { itemId = 5; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "Explosion";
    ////button.OnClicked = (e) => { itemId = 6; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "RevoltJoint";
    ////button.OnClicked = (e) => { itemId = 10; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point(200, 50))));
    //button.TextBlock.Text = "WheelJoint";
    ////button.OnClicked = (e) => { itemId = 7; };

    //button = group.AddChild(new GUIButton(new Rectangle(new Point(), new Point())));
    //button.TextBlock.Text = "RopeJoint";
    ////button.OnClicked = (e) => { itemId = 8; };


    //    button = canvas.AddChild(GUI.CreateButton(new Rectangle(new Point(-10, -30), new Point(200, 50)), "MainMenu"));
    //    button.Aligment = GUIAligment.RightUp;
    //    button.Pivot = new Vector2(1, 0);


    //    button = canvas.AddChild(GUI.CreateButton(new Rectangle(new Point(-10, -90), new Point(200, 50)), "Editor"));
    //    button.Aligment = GUIAligment.RightUp;
    //    button.Pivot = new Vector2(1, 0);
    //}

    //protected override void OnOpenScreen()
    //{
    //    SceneSystem.OpenScene("MainScene");

    //    Camera.Main.Color = Color.CornflowerBlue;
    //    Camera.Main.CameraType = CameraType.Game;

    //    entityManager = GameMain.Instance.World.EntityManager;
    //}

    //protected override void OnUpdate(GameTime time)
    //{
    //    if (InputSystem.actionMaps.Use.IsDown)
    //    {
    //        if (itemId == 0)
    //        {
    //            Vector2 pos = InputSystem.mousePositionInWorld;

    //            if (PhysicsDebugTools.IsConnected)
    //            {
    //                PhysicsDebugTools.RemoveDebugMouseJoint();
    //            }
    //            else
    //            {
    //                RayCastHit[] hits = Physics2D.OverlapCircle(pos, 0.3f);
    //                if (hits.Length > 0)
    //                {
    //                    PhysicsDebugTools.CreateDebugMousJoint(hits[0].body, pos);
    //                }
    //            }
    //        }
    //    }

    //    if (InputSystem.actionMaps.Use.IsPressed)
    //    {
    //        if (PhysicsDebugTools.IsConnected)
    //        {
    //            Vector2 pos = InputSystem.mousePositionInWorld;

    //            PhysicsDebugTools.SetDebugMousePosition(pos);
    //        }
    //    }

    //    if (InputSystem.actionMaps.Use.IsUp)
    //    {
    //        if (PhysicsDebugTools.IsConnected)
    //        {
    //            PhysicsDebugTools.RemoveDebugMouseJoint();
    //        }
    //    }

    //    if (InputSystem.actionMaps.Use.IsDown)
    //    {
    //        Vector2 pos = InputSystem.mousePositionInWorld;

    //        if (!GUI.MouseOnGUI)
    //        {
    //            if (itemId == 0)
    //            {

    //            }
    //            else if (itemId == 1)
    //            {
    //                entityManager.Create("box", pos);
    //            }
    //            else if (itemId == 9)
    //            {
    //                entityManager.Create("board", pos);
    //            }
    //            else if (itemId == 2)
    //            {
    //                entityManager.Create("pumkin", pos);
    //            }
    //            else if (itemId == 3)
    //            {
    //                entityManager.Create("log", pos);
    //            }
    //            else if (itemId == 4)
    //            {
    //                entityManager.Create("ground", pos);
    //            }
    //            else if (itemId == 5)
    //            {
    //                RayCastHit[] hits = Physics2D.OverlapCircle(pos, 0.3f);
    //                if (hits.Length > 0)
    //                {
    //                    if (Physics2D.TryGetEntity(hits[0].body, out Entity entity))
    //                    {
    //                        entity.Destroy();
    //                    }
    //                }
    //            }
    //            else if (itemId == 6)
    //            {
    //                Physics2D.Explosion(pos, 3, 0.3f);
    //                List<Entity> entities = new List<Entity>();

    //                foreach (var hit in Physics2D.OverlapCircle(pos, 2))
    //                {
    //                    if (Physics2D.TryGetEntity(hit.body, out Entity entity))
    //                    {
    //                        if (entity.Has<DestructiveObject>())
    //                        {
    //                            if (!entities.Contains(entity))
    //                            {
    //                                entities.Add(entity);
    //                                entity.Attach(new DestructiveRequest(pos));
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            else if (itemId == 10)
    //            {
    //                RayCastHit[] hits = Physics2D.OverlapCircle(pos, 0.3f);
    //                if (Physics2D.TryGetEntity(hits[0].body, out Entity entity))
    //                {
    //                    PhysicsDebugTools.CreateRevoltConnection(entity, pos);
    //                }
    //            }
    //            else if (itemId == 7)
    //            {
    //                RayCastHit[] hits = Physics2D.OverlapCircle(pos, 0.3f);
    //                if (hits.Length > 0)
    //                {
    //                    if (Physics2D.TryGetEntity(hits[0].body, out Entity entity))
    //                    {
    //                        PhysicsDebugTools.CreateWheelConnection(entityManager, "wheel", entity, pos, new Vector2(0, 1));
    //                    }
    //                }
    //            }
    //            else if (itemId == 8)
    //            {
    //                RayCastHit[] hits = Physics2D.OverlapCircle(pos, 0.3f);
    //                if (hits.Length > 0)
    //                {
    //                    if (Physics2D.TryGetEntity(hits[0].body, out Entity entity))
    //                    {
    //                        PhysicsDebugTools.CreateRopeConnection(entityManager, "rope", entity, pos);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //}
}
