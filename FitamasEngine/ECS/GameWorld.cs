/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Fitamas.Collections;
using System;

namespace Fitamas.ECS
{
    public class GameWorld : IDisposable
    {
        private Bag<IExecutor> executors;

        public EntityManager EntityManager { get; }
        public ComponentManager ComponentManager { get; }

        public int EntityCount => EntityManager.ActiveCount;

        internal GameWorld(Bag<IExecutor> executors)
        {
            this.executors = executors;

            RegisterSystem(ComponentManager = new ComponentManager());
            RegisterSystem(EntityManager = new EntityManager(ComponentManager));
        }

        public void Dispose()
        {
            foreach (var executor in executors)
            {
                executor.Dispose();
            }

            executors.Clear();
        }

        internal void RegisterSystem(ISystem system)
        {
            foreach (var executor in executors)
            {
                executor.RegisterSystem(system);
            }

            system.Initialize(this);
        }

        public Entity GetEntity(int entityId)
        {
            return EntityManager.Get(entityId);
        }

        public bool Contains(Entity entity)
        {
            return EntityManager.Contains(entity);
        }

        public Entity CreateEntity()
        {
            return EntityManager.Create();
        }

        public Entity CreateEntity(string name)
        {
            return EntityManager.Create(name);
        }

        public void DestroyEntity(int entityId)
        {
            EntityManager.Destroy(entityId);
        }

        public void DestroyEntity(Entity entity)
        {
            EntityManager.Destroy(entity);
        }
    }
}
