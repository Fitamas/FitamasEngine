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

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Fitamas.Collections;

namespace Fitamas.Entities
{
    public class ComponentManager : UpdateSystem, IComponentMapperService
    {
        public ComponentManager()
        {
            _componentMappers = new Bag<ComponentMapper>();
            _componentTypes = new Dictionary<Type, int>();
        }

        private readonly Bag<ComponentMapper> _componentMappers;
        private readonly Dictionary<Type, int> _componentTypes;

        public Action<int> ComponentsChanged;
        public Bag<ComponentMapper> ComponentMappers => _componentMappers;

        //private ComponentMapper CreateMapperForType(Type componentType, int componentTypeId)
        //{
        //    if (componentTypeId >= 32)
        //        throw new InvalidOperationException("Component type limit exceeded. We currently only allow 32 component types for performance reasons.");

        //    var mapper = new ComponentMapper(componentTypeId, componentType, ComponentsChanged);
        //    _componentMappers[componentTypeId] = mapper;
        //    return mapper;
        //}

        private ComponentMapper<T> CreateMapperForType<T>(int componentTypeId) where T : class
        {
            // TODO: We can probably do better than this without a huge performance penalty by creating our own bit vector that grows after the first 32 bits.
            if (componentTypeId >= 32)
                throw new InvalidOperationException("Component type limit exceeded. We currently only allow 32 component types for performance reasons.");

            var mapper = new ComponentMapper<T>(componentTypeId, ComponentsChanged);
            _componentMappers[componentTypeId] = mapper;
            return mapper;
        }

        public ComponentMapper GetMapper(int componentTypeId)
        {
            return _componentMappers[componentTypeId];
        }

        public ComponentMapper<T> GetMapper<T>() where T : class
        {
            var componentTypeId = GetComponentTypeId(typeof(T));

            if (_componentMappers[componentTypeId] != null)
                return _componentMappers[componentTypeId] as ComponentMapper<T>;

            return CreateMapperForType<T>(componentTypeId);
        }

        public ComponentMapper GetMapper(Type type)
        {
            //var componentTypeId = GetComponentTypeId(type);

            //if (_componentMappers[componentTypeId] != null)
            //    return _componentMappers[componentTypeId];

            //return CreateMapperForType(type, componentTypeId);

            throw new NotImplementedException();
        }

        public int GetComponentTypeId(Type type)
        {
            if (_componentTypes.TryGetValue(type, out var id))
                return id;

            id = _componentTypes.Count;
            _componentTypes.Add(type, id);
            return id;
        }

        public BitVector32 CreateComponentBits(int entityId)
        {
            var componentBits = new BitVector32();
            var mask = BitVector32.CreateMask();

            for (var componentId = 0; componentId < _componentMappers.Count; componentId++)
            {
                componentBits[mask] = _componentMappers[componentId]?.Has(entityId) ?? false;
                mask = BitVector32.CreateMask(mask);
            }

            return componentBits;
        }

        public void Destroy(int entityId)
        {
            foreach (var componentMapper in _componentMappers)
                componentMapper?.Delete(entityId);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
