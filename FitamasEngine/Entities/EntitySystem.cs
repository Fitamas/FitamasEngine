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

namespace Fitamas.Entities
{
    public abstract class EntitySystem : ISystem
    {
        private AspectBuilder _aspectBuilder;
        private EntitySubscription _subscription;
        private GameWorld _world;

        public Bag<int> ActiveEntities => _subscription.ActiveEntities;

        protected EntitySystem(AspectBuilder aspectBuilder)
        {
            _aspectBuilder = aspectBuilder;
        }

        public void Dispose()
        {
            if (_world != null)
            {
                _world.EntityManager.EntityAdded -= OnEntityAdded;
                _world.EntityManager.EntityRemoved -= OnEntityRemoved;
            }
        }

        protected virtual void OnEntityChanged(int entityId) { }
        protected virtual void OnEntityAdded(int entityId) { }
        protected virtual void OnEntityRemoved(int entityId) { }

        public virtual void Initialize(GameWorld world)
        {
            _world = world;

            var aspect = _aspectBuilder.Build(_world.ComponentManager);
            _subscription = new EntitySubscription(_world.EntityManager, aspect);
            _world.EntityManager.EntityAdded += OnEntityAdded;
            _world.EntityManager.EntityRemoved += OnEntityRemoved;
            _world.EntityManager.EntityChanged += OnEntityChanged;

            Initialize(world.ComponentManager);
        }

        public abstract void Initialize(IComponentMapperService mapperService);

        protected void DestroyEntity(int entityId)
        {
            _world.DestroyEntity(entityId);
        }

        protected void DestroyEntity(Entity entity)
        {
            _world.DestroyEntity(entity);
        }

        protected Entity CreateEntity()
        {
            return _world.CreateEntity();
        }

        protected Entity GetEntity(int entityId)
        {
            return _world.GetEntity(entityId);
        }

        protected bool Contains(Entity entity)
        {
            return _world.Contains(entity);
        }
    }
}
