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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Xna.Framework;
using Fitamas.Collections;
using Fitamas.Serialization;

namespace Fitamas.Entities
{
    public class EntityManager : UpdateSystem
    {
        private readonly ComponentManager componentManager;
        private int _nextId;

        public int Capacity => entityBag.Capacity;
        public IEnumerable<int> Entities => entityBag.Where(e => e != null).Select(e => e.Id);
        public int ActiveCount { get; private set; }

        private readonly Bag<Entity> entityBag;
        private readonly Pool<Entity> entityPool;
        private readonly Bag<int> addedEntities;
        private readonly Bag<int> removedEntities;
        private readonly Bag<int> changedEntities;
        private readonly Bag<BitVector32> entityToComponentBits;

        public event Action<int> EntityAdded;
        public event Action<int> EntityRemoved;
        public event Action<int> EntityChanged;

        public EntityManager(ComponentManager manager) 
        {
            componentManager = manager;
            addedEntities = new Bag<int>();
            removedEntities = new Bag<int>();
            changedEntities = new Bag<int>();
            entityToComponentBits = new Bag<BitVector32>();
            componentManager.ComponentsChanged += OnComponentsChanged;

            entityBag = new Bag<Entity>();
            entityPool = new Pool<Entity>(() => new Entity(_nextId++, this, componentManager));
        }

        private void OnComponentsChanged(int entityId)
        {
            changedEntities.Add(entityId);
            entityToComponentBits[entityId] = componentManager.CreateComponentBits(entityId);
            EntityChanged?.Invoke(entityId);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in addedEntities)
            {
                entityToComponentBits[entityId] = componentManager.CreateComponentBits(entityId);
                ActiveCount++;
                EntityAdded?.Invoke(entityId);
            }

            foreach (var entityId in changedEntities)
            {
                entityToComponentBits[entityId] = componentManager.CreateComponentBits(entityId);
                EntityChanged?.Invoke(entityId);
            }

            foreach (var entityId in removedEntities)
            {
                EntityRemoved?.Invoke(entityId);

                var entity = entityBag[entityId];
                entityBag[entityId] = null;
                componentManager.Destroy(entityId);
                entityToComponentBits[entityId] = default(BitVector32);
                ActiveCount--;
                entityPool.Free(entity);
            }

            addedEntities.Clear();
            removedEntities.Clear();
            changedEntities.Clear();
        }

        public BitVector32 GetComponentBits(int entityId)
        {
            return entityToComponentBits[entityId];
        }
        public Entity Create(Entity fromEntity)
        {
            var entity = Create();
            entity.Name = fromEntity.Name;

            foreach (var mapper in componentManager.ComponentMappers)
            {
                if (mapper != null && mapper.Has(fromEntity.Id))
                {
                    if (mapper.GetObject(fromEntity.Id) is Component component)
                    {
                        MonoObject instantiate = MonoObject.Instantiate(component);
                        entity.Attach(mapper.ComponentType, instantiate);
                    }
                }
            }
            return entity;
        }

        public Entity Create()
        {
            var entity = entityPool.Obtain();
            entity.Name = "Entity";
            var id = entity.Id;
            entityBag[id] = entity;
            addedEntities.Add(id);
            entityToComponentBits[id] = new BitVector32(0);
            return entity;
        }

        public Entity Get(int entityId)
        {
            return entityBag[entityId];
        }

        public void Destroy(int entityId)
        {
            if (!removedEntities.Contains(entityId))
                removedEntities.Add(entityId);
        }

        public void Destroy(Entity entity)
        {
            Destroy(entity.Id);
        }

        public void InstantDestroy(int entityId)
        {
            var entity = entityBag[entityId];
            entityBag[entityId] = null;
            componentManager.Destroy(entityId);
            entityToComponentBits[entityId] = default(BitVector32);
            ActiveCount--;
            entityPool.Free(entity);
        }

        public void InstantDestroy(Entity entity)
        {
            InstantDestroy(entity.Id);
        }
    }
}
