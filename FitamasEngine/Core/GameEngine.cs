using Fitamas.ECS;
using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Fitamas.Container;
using Fitamas.Animation;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Scene;
using Fitamas.UserInterface;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.DebugTools;
using Fitamas.Audio;
using Fitamas.Tweening;
using Fitamas.ECS.Transform2D;

namespace Fitamas.Core
{
    public class GameEngine : Game
    {
        private GameWorld gameWorld;
        private PhysicsWorld physicsWorld;
        private LoadContentExecutor loadContentExecutor;
        private FixedUpdateExecutor fixedUpdateExecutor;
        private UpdateExecutor updateExecutor;
        private DrawExecutor drawExecutor;
        private DrawGizmosExecutor drawGizmosExecutor;
        private float accamulatorTime;

        public const float FixedTimeStep = 0.02f;

        public GameWorld GameWorld => gameWorld;
        public PhysicsWorld PhysicsWorld => physicsWorld;

        public static GameEngine Instance { get; private set; }
        public DIContainer MainContainer { get; }
        public InputManager InputManager { get; }
        public AudioManager AudioManager { get; }
        public TweenManager TweenManager { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }
        public WindowViewportAdapter WindowViewportAdapter { get; private set; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            MainContainer = new DIContainer();
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.DeviceCreated += (s, a) =>
            {
                WindowViewportAdapter = new WindowViewportAdapter(Window, GraphicsDeviceManager.GraphicsDevice);
            };

            InputManager = new InputManager(this);
            Components.Add(InputManager);
            MainContainer.RegisterInstance(InputManager);

            AudioManager = new AudioManager(this);
            Components.Add(AudioManager);
            MainContainer.RegisterInstance(AudioManager);

            TweenManager = new TweenManager(this);
            Components.Add(TweenManager);
            MainContainer.RegisterInstance(TweenManager);
        }

        protected override void Initialize()
        {
            Debug.Log("Load application settings");

            PlayerPrefs.Load();

            IsMouseVisible = true;
            //TODO
            GraphicsDeviceManager.PreferredBackBufferWidth = 1800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
            GraphicsDeviceManager.ApplyChanges();
            Window.AllowUserResizing = true;

            Debug.Log("Load game content");

            base.Initialize();

            Debug.Log("Initialize systems");

            gameWorld = CreateWorldBuilder().Build();

            Debug.Log("Load systems content");

            loadContentExecutor.LoadContent(Content);
        }

        protected virtual WorldBuilder CreateWorldBuilder()
        {
            return new WorldBuilder(this)
                //Executors
                .AddExecutor(loadContentExecutor = new LoadContentExecutor())
                .AddExecutor(fixedUpdateExecutor = new FixedUpdateExecutor())
                .AddExecutor(updateExecutor = new UpdateExecutor())
                .AddExecutor(drawExecutor = new DrawExecutor())
                .AddExecutor(drawGizmosExecutor = new DrawGizmosExecutor())

                //Core
                .AddSystem(new SceneSystem())
                .AddSystem(new TransformSystem())

                //Phisics
                .AddSystem(physicsWorld = new PhysicsWorld())

                //Animation
                .AddSystem(new AnimationSystem())
                .AddSystem(new TweenSystem(TweenManager))
                
                //Audio
                .AddSystem(new AudioSystem(AudioManager))

                //Render
                .AddSystem(new CameraSystem(this))
                .AddSystem(new SpriteRenderSystem(GraphicsDevice))
                .AddSystem(new MeshRenderSystem(GraphicsDevice))
                .AddSystem(new GUISystem(this));
        }

        protected override void Update(GameTime gameTime)
        {
            TweenManager.Update(gameTime);

            base.Update(gameTime);

            accamulatorTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (accamulatorTime > FixedTimeStep)
            {
                accamulatorTime -= FixedTimeStep;

                fixedUpdateExecutor.FixedUpdate(FixedTimeStep);
            }

            if (!IsActive)
            {
                return;
            }

            updateExecutor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Camera.Current = Camera.Main;
            if (Camera.Current != null)
            {
                GraphicsDevice.Clear(Camera.Current.Color);
            }
            else
            {
                return;
            }

            drawExecutor.Draw(gameTime);

            Gizmos.Begin();
            drawGizmosExecutor.DrawGizmos();
            Gizmos.End();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            gameWorld.Dispose();
            AudioManager.Dispose();
        }
    }
}
