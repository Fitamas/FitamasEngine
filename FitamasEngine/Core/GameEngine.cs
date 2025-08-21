using Fitamas.Animation;
using Fitamas.Audio;
using Fitamas.Container;
using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.ECS.Transform2D;
using Fitamas.Graphics;
using Fitamas.Graphics.PostProcessors;
using Fitamas.Graphics.RendererFeatures;
using Fitamas.Graphics.ViewportAdapters;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Scene;
using Fitamas.Tweening;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Fitamas.Core
{
    public class GameEngine : Game
    {
        public const float FixedTimeStep = 0.02f;

        private GameWorld gameWorld;
        private PhysicsWorld physicsWorld;

        private LoadContentExecutor loadContentExecutor;
        private FixedUpdateExecutor fixedUpdateExecutor;
        private UpdateExecutor updateExecutor;
        private DrawExecutor drawExecutor;
        private DrawGizmosExecutor drawGizmosExecutor;

        private float accamulatorTime;

        public GameWorld GameWorld => gameWorld;
        public PhysicsWorld PhysicsWorld => physicsWorld;

        public static GameEngine Instance { get; private set; }
        public DIContainer MainContainer { get; }
        public InputManager InputManager { get; }
        public AudioManager AudioManager { get; }
        public TweenManager TweenManager { get; }
        public RenderManager RenderManager { get; private set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }
        public WindowViewportAdapter WindowViewportAdapter { get; private set; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            MainContainer = new DIContainer();

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,
                SynchronizeWithVerticalRetrace = false,
            };
            GraphicsDeviceManager.ApplyChanges();

            WindowViewportAdapter = new WindowViewportAdapter(Window, GraphicsDeviceManager.GraphicsDevice);

            InputManager = new InputManager(this);
            Components.Add(InputManager);
            MainContainer.RegisterInstance(InputManager);

            AudioManager = new AudioManager(this);
            Components.Add(AudioManager);
            MainContainer.RegisterInstance(AudioManager);

            TweenManager = new TweenManager(this);
            Components.Add(TweenManager);
            MainContainer.RegisterInstance(TweenManager);

            RenderManager = new RenderManager(this);
            Components.Add(RenderManager);
            MainContainer.RegisterInstance(RenderManager);
        }

        protected override void Initialize()
        {
            Debug.Log("Load application settings");

            PlayerPrefs.Load();

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            //TODO
            GraphicsDeviceManager.PreferredBackBufferWidth = 1800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 920;
            GraphicsDeviceManager.ApplyChanges();

            Debug.Log("Load game content");

            base.Initialize();

            Debug.Log("Initialize systems");

            gameWorld = CreateWorldBuilder().Build();

            RenderManager.AddRendererFeature(new LightingRendererFeature(GraphicsDevice));
            RenderManager.AddRendererFeature(new WorldRendererFeature(GraphicsDevice));
            RenderManager.AddRendererFeature(new PostRendererFeature(GraphicsDevice, drawExecutor));
            RenderManager.AddRendererFeature(new GizmosRendererFeature(GraphicsDevice, drawGizmosExecutor));

            RenderManager.AddPostProcessor(new VignettePostProcessor(0));

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
                .AddSystem(new SpriteDrawSystem(RenderManager))
                .AddSystem(new MeshDrawSystem(RenderManager))
                .AddSystem(new GUISystem(this));
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

            RenderManager.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            gameWorld.Dispose();
            AudioManager.Dispose();
        }
    }
}
