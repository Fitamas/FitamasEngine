using System;

namespace Fitamas.Core
{
    public class ResourceKey<T>
    {
        public string TypeId { get; }

        public ResourceKey(string typeId)
        {
            TypeId = typeId;
        }
    }
}
