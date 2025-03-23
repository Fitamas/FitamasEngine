using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Fitamas.Core
{
    public abstract class Registry
    {
        public static T Register<T>(Registry<T> registry, ResourceKey<Registry<T>> resourceKey, T value)
        {
            return Register(registry, resourceKey.TypeId, value);
        }

        public static T Register<T>(Registry<T> registry, string typeId, T value)
        {
            return ((WritableRegistry<T>)registry).Register(typeId, value);
        }
    }

    public abstract class Registry<T> : Registry
    {
        protected Dictionary<string, T> registryMap = new Dictionary<string, T>();

        public T this[string key]
        {
            get
            {
                return registryMap[key];
            }
        }
    }

    public class WritableRegistry<T> : Registry<T>
    {
        public T Register(string key, T value)
        {
            registryMap[key] = value;
            return value;
        }
    }
}
