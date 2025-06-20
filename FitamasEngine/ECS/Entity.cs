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
using System.Collections.Specialized;

namespace Fitamas.ECS
{
    public class Entity : IEntity, IEquatable<Entity>
    {
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        
        public string Name { get; set; }
        public int Id { get; }

        public BitVector32 ComponentBits => _entityManager.GetComponentBits(Id);

        internal Entity(int id, EntityManager entityManager, ComponentManager componentManager)
        {
            Name = "Entity";
            Id = id;

            _entityManager = entityManager;
            _componentManager = componentManager;
        }

        public void Attach(Type type, object component)
        {
            var mapper = _componentManager.GetMapper(type);
            mapper.Put(Id, component);
        }

        public void Attach<T>(T component) where T : class
        {
            var mapper = _componentManager.GetMapper<T>();
            mapper.Put(Id, component);
        }

        public void Detach<T>() where T : class
        {
            var mapper = _componentManager.GetMapper<T>();
            mapper.Delete(Id);
        }

        public T Get<T>() where T : class
        {
            var mapper = _componentManager.GetMapper<T>();
            return mapper.Get(Id);
        }


        public bool Has<T>() where T : class
        {
            ComponentMapper<T> mapper = _componentManager.GetMapper<T>();
            if (mapper == null)
            {
                return false;
            }
            else
            {
                return mapper.Has(Id);
            }
        }

        public bool TryGet<T>(out T item) where T : class
        {
            bool contain = Has<T>();

            if (contain)
            {
                item = Get<T>();
            }
            else
            {
                item = null;
            }

            return contain;
        }

        public void Destroy()
        {
            _entityManager.Destroy(Id);
        }

        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}
