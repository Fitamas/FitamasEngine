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
using Fitamas.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace Fitamas.Entities
{
    public class GameWorld : IDisposable
    {
        private readonly Bag<IFixedUpdateSystem> _fixedUpdateSystems;
        private readonly Bag<IUpdateSystem> _updateSystems;
        private readonly Bag<IDrawSystem> _drawSystems;
        private readonly Bag<ILoadContentSystem> _loadSystem;

        public EntityManager EntityManager { get; }
        public ComponentManager ComponentManager { get; }
        public Game Game { get; }

        public int EntityCount => EntityManager.ActiveCount;

        internal GameWorld(Game game)
        {
            Game = game;

            _fixedUpdateSystems = new Bag<IFixedUpdateSystem>();
            _updateSystems = new Bag<IUpdateSystem>();
            _drawSystems = new Bag<IDrawSystem>();
            _loadSystem = new Bag<ILoadContentSystem>();

            RegisterSystem(ComponentManager = new ComponentManager());
            RegisterSystem(EntityManager = new EntityManager(ComponentManager));
        }

        public void Dispose()
        {
            foreach (var fixedUpdateSystem in _fixedUpdateSystems)
            {
                fixedUpdateSystem.Dispose();
            }

            foreach (var updateSystem in _updateSystems)
            {
                updateSystem.Dispose();
            }
   
            foreach (var drawSystem in _drawSystems)
            {
                drawSystem.Dispose();
            }

            foreach (var loadSystem in _loadSystem)
            {
                loadSystem.Dispose();
            }

            _fixedUpdateSystems.Clear();
            _updateSystems.Clear();
            _drawSystems.Clear();
            _loadSystem.Clear();
        }

        internal void RegisterSystem(ISystem system)
        {
            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                _fixedUpdateSystems.Add(fixedUpdateSystem);
            }

            if (system is IUpdateSystem updateSystem)
            {
                _updateSystems.Add(updateSystem);
            }

            if (system is IDrawSystem drawSystem)
            {
                _drawSystems.Add(drawSystem);
            }

            if (system is ILoadContentSystem loadSystem)
            {
                _loadSystem.Add(loadSystem);
            }

            system.Initialize(this);
        }

        public Entity GetEntity(int entityId)
        {
            return EntityManager.Get(entityId);
        }

        public Entity CreateEntity()
        {
            return EntityManager.Create();
        }

        public void DestroyEntity(int entityId)
        {
            EntityManager.Destroy(entityId);
        }

        public void DestroyEntity(Entity entity)
        {
            EntityManager.Destroy(entity);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var system in _loadSystem)
            {
                system.LoadContent(content);
            }
        }

        public void FixedUpdate(float delta)
        {
            foreach (var system in _fixedUpdateSystems)
            {
                system.FixedUpdate(delta);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var system in _updateSystems)
            {
                system.Update(gameTime);
            }  
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var system in _drawSystems)
            {
                system.Draw(gameTime);
            }
        }
    }
}
