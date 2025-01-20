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
    public class GameEngine : Game
    {
        private GameWorld world;
        private float accamulatorTime;

        public const float FixedTimeStep = 0.02f;

        public GameWorld World => world;

        public static GameEngine Instance { get; private set; }
        public DIContainer Container { get; }
        public ObjectManager ObjectManager { get; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; }

        public GameEngine()
        {
            Content.RootDirectory = "Content";
            Instance = this;

            Container = new DIContainer();
            ObjectManager = new ObjectManager(this, "Content");
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
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

            world = CreateWorldBuilder(this).Build();

            base.Initialize();
        }

        public virtual WorldBuilder CreateWorldBuilder(GameEngine game)
        {

            return new WorldBuilder(game)
                //Load content
                .AddSystem(new ScriptingSystem())
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
                .AddSystem(new CameraSystem(game))
                .AddSystem(new RenderSystem(game.GraphicsDevice))
                .AddSystem(new GUISystem(game.GraphicsDevice))
                .AddSystem(new DebugRenderSystem(game.GraphicsDevice));
        }

        protected override void LoadContent()
        {
            Debug.Log("Load content");

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
