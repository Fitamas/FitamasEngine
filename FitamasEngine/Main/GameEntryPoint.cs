using Fitamas.Container;
using Fitamas.Animation;
using Fitamas.DebugTools;
using Fitamas.Entities;
using Fitamas.Gameplay.Characters;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Scene;
using Fitamas.Scripting;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Fitamas.Main
{
    public class GameEntryPoint
    {
        public GameEntryPoint()
        {

        }

        public virtual void LoadSettings(GameMain game, DIContainer container)
        {
            GraphicsDeviceManager manager = game.GraphicsDeviceManager;

            manager.PreferredBackBufferWidth = 1800;
            manager.PreferredBackBufferHeight = 1080;
            manager.ApplyChanges();

            game.Window.AllowUserResizing = true;
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
                //.AddSystem(new DestructiveEntitySystem())
                //.AddSystem(new CharacterController())
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
    }
}
