using Fitamas.Entities;
using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Fitamas.Serialization;
using Fitamas.Container;
using Fitamas.Animation;
using Fitamas.DebugTools;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Physics.Characters;
using Fitamas.Scene;
using Fitamas.Scripting;
using Fitamas.UserInterface;
using Fitamas.UserInterface.ViewModel;
using System.ComponentModel;

namespace Fitamas.Core
{
    public class GameEngine : Game
    {
        private GameWorld world;
        private float accamulatorTime;

        public const float FixedTimeStep = 0.02f;

        public GameWorld World => world;

        public static GameEngine Instance { get; private set; }
        public DIContainer Container { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            Container = new DIContainer();
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            Debug.Log("Load application settings");

            PlayerPrefs.Load();

            IsMouseVisible = true;
            GraphicsDeviceManager.PreferredBackBufferWidth = 1800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
            GraphicsDeviceManager.ApplyChanges();
            Window.AllowUserResizing = true;

            Debug.Log("Load game content");

            base.Initialize();

            Debug.Log("Initialize systems");

            world = CreateWorldBuilder().Build();

            Debug.Log("Load systems content");

            world.LoadContent(Content);
        }

        protected virtual WorldBuilder CreateWorldBuilder()
        {
            return new WorldBuilder(this)
                //Load content
                .AddSystem(new SceneSystem())

                //Input
                .AddSystem(new InputSystem())
                //.AddSystem(new PlayerSystem())

                //Phisics
                .AddSystem(new PhysicsSystem())
                .AddSystem(new CharacterController())

                //Animation
                .AddSystem(new AnimationSystem())

                //Render
                .AddSystem(new CameraSystem(this))
                .AddSystem(new RenderSystem(GraphicsDevice))
                .AddSystem(new DebugRenderSystem(GraphicsDevice))
                
                //User interface
                .AddSystem(CreateGUISystem());
        }

        protected virtual GUISystem CreateGUISystem()
        {
            GUISystem system = new GUISystem(GraphicsDevice);
            Container.RegisterInstance(ApplicationKey.GUISystem, system);

            GUIRootBinder rootBinder = new GUIRootBinder(system);
            Container.RegisterInstance(ApplicationKey.GUIRootBinder, rootBinder);

            GUIRootViewModel rootViewModel = new GUIRootViewModel();
            rootBinder.Bind(rootViewModel);
            Container.RegisterInstance(ApplicationKey.GUIRootViewModel, rootViewModel);

            return system;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            accamulatorTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (accamulatorTime > FixedTimeStep)
            {
                accamulatorTime -= FixedTimeStep;

                world.FixedUpdate(FixedTimeStep);
            }

            if (!IsActive)
            {
                return;
            }

            world.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Camera.Current = Camera.Main;

            world.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            world.Dispose();
        }
    }
}
