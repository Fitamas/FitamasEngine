using Fitamas.Collections;
using Fitamas.Extended.Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ECS
{
    public abstract class Executor<T> : IExecutor where T : ISystem
    {
        protected Bag<T> systems = new Bag<T>();

        public void RegisterSystem(ISystem system)
        {
            if (system is T res)
            {
                systems.Add(res);
            }
        }

        public void Dispose()
        {
            foreach (var system in systems)
            {
                system.Dispose();
            }

            systems.Clear();
        }
    }

    public class DrawExecutor : Executor<IDrawSystem>
    {
        public void Draw(GameTime gameTime)
        {
            foreach (var system in systems)
            {
                system.Draw(gameTime);
            }
        }
    }

    public class DrawGizmosExecutor : Executor<IDrawGizmosSystem>
    {
        public void DrawGizmos()
        {
            foreach (var system in systems)
            {
                system.DrawGizmos();
            }
        }
    }

    public class UpdateExecutor : Executor<IUpdateSystem>
    {
        public void Update(GameTime gameTime)
        {
            foreach (var system in systems)
            {
                system.Update(gameTime);
            }
        }
    }

    public class FixedUpdateExecutor : Executor<IFixedUpdateSystem>
    {
        public void FixedUpdate(float deltaTime)
        {
            foreach (var system in systems)
            {
                system.FixedUpdate(deltaTime);
            }
        }
    }

    public class LoadContentExecutor : Executor<ILoadContentSystem>
    {
        public void LoadContent(ContentManager manager)
        {
            foreach (var system in systems)
            {
                system.LoadContent(manager);
            }
        }
    }
}
