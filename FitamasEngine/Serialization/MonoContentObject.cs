using System;

namespace Fitamas.Serialization
{
    public abstract class MonoContentObject : MonoObject
    {
        public string Name { get; }

        public MonoContentObject(string name = "NewObject")
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }
    }
}
