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

namespace Fitamas.ECS
{
    public class EntitySubscription
    {
        private readonly Bag<int> _activeEntities;
        private readonly EntityManager _entityManager;
        private readonly Aspect _aspect;
        private bool _rebuildActives;

        internal EntitySubscription(EntityManager entityManager, Aspect aspect)
        {
            _entityManager = entityManager;
            _aspect = aspect;
            _activeEntities = new Bag<int>(entityManager.Capacity);
            _rebuildActives = true;

            _entityManager.EntityAdded += OnEntityAdded;
            _entityManager.EntityRemoved += OnEntityRemoved;
            _entityManager.EntityChanged += OnEntityChanged;
        }

        private void OnEntityAdded(int entityId)
        {
            if (_aspect.IsInterested(_entityManager.GetComponentBits(entityId)))
                _activeEntities.Add(entityId);
        }

        private void OnEntityRemoved(int entityId) => _rebuildActives = true;
        private void OnEntityChanged(int entityId) => _rebuildActives = true;

        public void Dispose()
        {
            _entityManager.EntityAdded -= OnEntityAdded;
            _entityManager.EntityRemoved -= OnEntityRemoved;
        }

        public Bag<int> ActiveEntities
        {
            get
            {
                if (_rebuildActives)
                    RebuildActives();

                return _activeEntities;
            }
        }

        private void RebuildActives()
        {
            _activeEntities.Clear();

            foreach (var entity in _entityManager.Entities)
                OnEntityAdded(entity);

            _rebuildActives = false;
        }
    }
}
