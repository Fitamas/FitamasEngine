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
using Fitamas.Scenes;
using Fitamas.Serialization;
using Fitamas.Serialization.Json;
using Fitamas.Tweening;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using R3;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.Core
{
    public class GameEngine : Game
    {
        public const float FixedTimeStep = 0.02f;

        private IEnumerable<IUpdateable> updateables;

        private GameWorld gameWorld;
        private PhysicsWorld physicsWorld;

        private LoadContentExecutor loadContentExecutor;
        private FixedUpdateExecutor fixedUpdateExecutor;
        private UpdateExecutor updateExecutor;
        private DrawExecutor drawExecutor;
        private DrawGizmosExecutor drawGizmosExecutor;

        private WindowViewportAdapter defaultViewportAdapter;
        private float accamulatorTime;

        public GameWorld GameWorld => gameWorld;
        public PhysicsWorld PhysicsWorld => physicsWorld;

        public static GameEngine Instance { get; private set; }
        public DIContainer MainContainer { get; }
        public InputManager InputManager { get; }
        public AudioManager AudioManager { get; }
        public TweenManager TweenManager { get; }
        public RenderManager RenderManager { get; }
        public GUIManager GUIManager { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }
        public ReactiveProperty<ViewportAdapter> ViewportAdapterProperty { get; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            MainContainer = new DIContainer();

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                //GraphicsProfile = GraphicsProfile.HiDef,
                //SynchronizeWithVerticalRetrace = false,
            };
            GraphicsDeviceManager.ApplyChanges();

            defaultViewportAdapter = new WindowViewportAdapter(Window, GraphicsDevice);
            ViewportAdapterProperty = new ReactiveProperty<ViewportAdapter>(defaultViewportAdapter);

            InputManager = new InputManager(this);
            MainContainer.RegisterInstance(InputManager, true);

            AudioManager = new AudioManager(this);
            MainContainer.RegisterInstance(AudioManager, true);

            TweenManager = new TweenManager(this);
            MainContainer.RegisterInstance(TweenManager, true);

            RenderManager = new RenderManager(this);
            MainContainer.RegisterInstance(RenderManager, true);

            GUIManager = new GUIManager(this);
            MainContainer.RegisterInstance(GUIManager, true);
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
            //IsFixedTimeStep = false;
            //GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            GraphicsDeviceManager.ApplyChanges();

            //string path = Path.Combine(Content.RootDirectory, "Assets", "order.resource");
            //if (File.Exists(path))
            //{
            //    ResourceManifest = JsonUtility.Load<ResourceManifest>(path);
            //}
            //else
            //{
            //    ResourceManifest = new ResourceManifest();
            //}

            Debug.Log("Create game world");
            gameWorld = CreateWorldBuilder().Build();

            //RenderManager.AddRendererFeature(new LightingRendererFeature(GraphicsDevice));
            RenderManager.AddRendererFeature(new WorldRendererFeature(GraphicsDevice));
            RenderManager.AddRendererFeature(new PostRendererFeature(this, drawExecutor));
            RenderManager.AddRendererFeature(new GizmosRendererFeature(this, drawGizmosExecutor));

            Debug.Log("Initialize systems");
            foreach (var initializable in MainContainer.ResolveAll<IInitializable>())
            {
                initializable.Initialize();
            }
            updateables = MainContainer.ResolveAll<IUpdateable>();

            Debug.Log("Load game content");
            base.Initialize();
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
                .AddSystem(new MeshDrawSystem(RenderManager));
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

            foreach (var update in updateables)
            {
                update.Update(gameTime);
            }

            updateExecutor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            RenderManager.Render(gameTime);
        }

        public void SetDefaultViewportAdapter()
        {
            ViewportAdapterProperty.Value = defaultViewportAdapter;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            gameWorld.Dispose();
            MainContainer.Dispose();
        }
    }
}
