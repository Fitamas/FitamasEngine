using Fitamas.Entities;
using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Fitamas.Container;
using Fitamas.Animation;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Scene;
using Fitamas.UserInterface;
using Fitamas.Graphics.ViewportAdapters;

namespace Fitamas.Core
{
    public class GameEngine : Game
    {
        private GameWorld world;
        private LoadContentExecutor loadContentExecutor;
        private FixedUpdateExecutor fixedUpdateExecutor;
        private UpdateExecutor updateExecutor;
        private DrawExecutor drawExecutor;
        private DrawGizmosExecutor drawGizmosExecutor;
        private float accamulatorTime;

        public const float FixedTimeStep = 0.02f;

        public GameWorld World => world;

        public static GameEngine Instance { get; private set; }
        public DIContainer MainContainer { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }
        public WindowViewportAdapter WindowViewportAdapter { get; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            MainContainer = new DIContainer();
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            WindowViewportAdapter = new WindowViewportAdapter(Window, GraphicsDevice);
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

            InputManager manager = new InputManager(this);
            MainContainer.RegisterInstance(manager);

            Debug.Log("Load game content");

            base.Initialize();

            Debug.Log("Initialize systems");

            world = CreateWorldBuilder().Build();

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

                //Load content
                .AddSystem(new SceneSystem())

                //Animation
                .AddSystem(new AnimationSystem())

                //Render
                .AddSystem(new CameraSystem(this))
                .AddSystem(new SpriteRenderSystem(GraphicsDevice))
                .AddSystem(new MeshRenderSystem(GraphicsDevice))
                .AddSystem(new GUISystem(this))

                //Phisics
                .AddSystemAndRegister(new PhysicsWorldSystem(), MainContainer);
        }

        protected override void Update(GameTime gameTime)
        {
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

            drawExecutor.Draw(gameTime);
            
            drawGizmosExecutor.DrawGizmos();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            world.Dispose();
        }
    }
}
