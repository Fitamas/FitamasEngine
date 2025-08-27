/*
    MIT License

    Contacts:
    Email: andreyvavilichev@gmail.com
    LinkedIn: https://www.linkedin.com/in/vavilichevgd/

    Copyright (c) 2024 Andrey Vavilichev

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

namespace Fitamas.Container
{
    public class DIContainer : IDisposable
    {
        private readonly DIContainer _parentContainer;
        private readonly Dictionary<(string, Type), DIEntry> _entriesMap = new();
        private readonly HashSet<(string, Type)> _resolutionsCache = new();

        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }

        public DIEntry RegisterFactory<T>(Func<DIContainer, T> factory)
        {
            return RegisterFactory(null, factory);
        }

        public DIEntry RegisterFactory<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            
            if (_entriesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"DI: Factory with tag {key.Item1} and type {key.Item2.FullName} has already registered");
            }

            var diEntry = new DIEntry<T>(this, factory);

            _entriesMap[key] = diEntry;

            return diEntry;
        }

        public void RegisterInstance<T>(T instance, bool withInterfaces = false)
        {
            RegisterInstance(null, instance, withInterfaces);
        }

        public void RegisterInstance<T>(string tag, T instance, bool withInterfaces = false)
        {
            var key = (tag, typeof(T));
            
            if (_entriesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"DI: Instance with tag {key.Item1} and type {key.Item2.FullName} has already registered");
            }

            var diEntry = new DIEntry<T>(this, instance, withInterfaces);

            _entriesMap[key] = diEntry;
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_resolutionsCache.Contains(key))
            {
                throw new Exception($"DI: Cyclic dependency for tag {key.tag} and type {key.Item2.FullName}");
            }

            _resolutionsCache.Add(key);

            try
            {
                if (_entriesMap.TryGetValue(key, out var diEntry))
                {
                    return diEntry.Resolve<T>();
                }

                if (_parentContainer != null)
                {
                    return _parentContainer.Resolve<T>(tag);
                }
            }
            finally
            {
                _resolutionsCache.Remove(key);
            } 
            
            throw new Exception($"Couldn't find dependency for tag {tag} and type {key.Item2.FullName}");
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            Type type = typeof(T);  
            List<T> values = new List<T>();

            foreach (var entry in _entriesMap.Values)
            {
                if (entry.ContractTypes.Contains(type))
                {
                    values.Add(entry.Resolve<T>());
                }
            }

            return values;
        }

        public void Dispose()
        {
            var entries = _entriesMap.Values;
            
            foreach (var entry in entries)
            {
                entry.Dispose();
            }
        }
    }
}