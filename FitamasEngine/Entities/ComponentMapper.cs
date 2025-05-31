using Fitamas.Collections;
using System;

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

namespace Fitamas.Entities
{
    public abstract class ComponentMapper
    {
        public Action<int> OnPut;
        public Action<int> OnDelete;
        public Action<int> OnCompositionChanged;


        public ComponentMapper(int id, Type componentType, Action<int> onCompositionChanged)
        {
            OnCompositionChanged = onCompositionChanged;
            Id = id;
            ComponentType = componentType;
        }

        public int Id { get; }
        public Type ComponentType { get; }

        public abstract void Put(int entityId, object component);

        public abstract bool Has(int entityId);

        public abstract object GetObject(int entityId);

        public abstract void Delete(int entityId);
    }

    public class ComponentMapper<T> : ComponentMapper where T : class
    {
        protected Bag<T> Components { get; }

        public ComponentMapper(int id, Action<int> onCompositionChanged) 
            : base(id, typeof(T), onCompositionChanged)
        {
            Components = new Bag<T>();
        }

        public override void Put(int entityId, object component)
        {
            Put(entityId, (T)component);
        }

        public void Put(int entityId, T component)
        {
            Components[entityId] = component;
            OnCompositionChanged?.Invoke(entityId);
            OnPut?.Invoke(entityId);
        }

        public override bool Has(int entityId)
        {
            if (entityId >= Components.Count)
                return false;

            return Components[entityId] != null;
        }

        public override object GetObject(int entityId)
        {
            return Components[entityId];
        }

        public T Get(Entity entity)
        {
            return Get(entity.Id);
        }

        public T Get(int entityId)
        {
            return Components[entityId];
        }

        public bool TryGet(int entityId, out T output)
        {
            if (entityId < Components.Count && Components[entityId] != null)
            {
                output = Components[entityId];
                return true;
            }
            else
            {
                output = null;
                return false;
            }
        }

        public override void Delete(int entityId)
        {
            Components[entityId] = null;
            OnCompositionChanged?.Invoke(entityId);
            OnDelete?.Invoke(entityId);
        }
    }
}
