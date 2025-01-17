using Fitamas.Entities;
using Fitamas.Graphics;
using Microsoft.Xna.Framework;
using Fitamas.Serializeble;
using Fitamas.Container;
using Fitamas.Animation;
using Fitamas.DebugTools;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Physics.Characters;
using Fitamas.Scene;
using Fitamas.Scripting;
using Fitamas.UserInterface;
using System.Collections.Generic;
using System;

namespace Fitamas.Main
{
    public class GameMain : Game
    {
        private GameWorld world;
        private float accamulatorTime;

        public const float FixedTimeStep = 0.02f;

        public GameWorld World => world;

        public static GameMain Instance { get; private set; }
        public DIContainer Container { get; }
        private ObjectManager ObjectManager { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        public GameMain()
        {
            Instance = this;

            Container = new DIContainer();

            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            ObjectManager = new ObjectManager(this, "Content");
        }

        protected override void Initialize()
        {
            Debug.Log("Load application settings");

            //TODO

            IsMouseVisible = true;

            GraphicsDeviceManager.PreferredBackBufferWidth = 1800;
            GraphicsDeviceManager.PreferredBackBufferHeight = 1080;
            GraphicsDeviceManager.ApplyChanges();

            Window.AllowUserResizing = true;

            Debug.Log("Initialize systems");

            world = CreateWorld(this);

            base.Initialize();
        }

        public virtual GameWorld CreateWorld(GameMain game)
        {

            return new WorldBuilder(game)
                //Load content
                .AddSystem(new ScriptingSystem())
                .AddSystem(new SceneSystem())

                //Input
                .AddSystem(new InputSystem())
                //.AddSystem(new PlayerSystem())

                //Phisics
                .AddSystem(new CharacterController())
                .AddSystem(new PhysicsSystem())

                //Animation
                .AddSystem(new AnimationSystem())

                //Game systems
                .AddSystem(AddSystems(game))

                //Render
                .AddSystem(new CameraSystem(game))
                .AddSystem(new RenderSystem(game.GraphicsDevice))
                .AddSystem(new GUISystem(game.GraphicsDevice))
                .AddSystem(new DebugRenderSystem(game.GraphicsDevice))

                .Build();
        }

        public virtual IEnumerable<ISystem> AddSystems(GameMain game)
        {
            return Array.Empty<ISystem>();
        }

        protected override void LoadContent()
        {
            Debug.Log("Load content");

            Resources.LoadContent(ObjectManager);

            world.LoadContent(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            accamulatorTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (accamulatorTime > FixedTimeStep)
            {
                accamulatorTime -= FixedTimeStep;

                world.FixedUpdate(FixedTimeStep);
            }

            world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Camera.Current = Camera.Main;

            world.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            world.Dispose();
        }
    }
}
