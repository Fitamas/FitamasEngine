using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Fitamas.Serialization.Json
{
    public class MonoObjectResolver : IReferenceResolver
    {
        private Dictionary<Guid, MonoObject> dictionary = new Dictionary<Guid, MonoObject>();

        public MonoObjectResolver()
        {

        }

        // This method is called during deserialize for $id
        public void AddReference(object context, string reference, object value)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                if (value is MonoObject monoObject)
                {
                    Guid id = new Guid(reference);
                    dictionary[id] = monoObject;
                    monoObject.Guid = id;
                }
            }
        }

        // This method is called during deserialize for $ref
        public object ResolveReference(object context, string reference)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                Guid id = new Guid(reference);

                dictionary.TryGetValue(id, out MonoObject monoObject);

                return monoObject;
            }

            return null;
        }

        // Returns false, so that $id is used, not $ref.
        public bool IsReferenced(object context, object value)
        {
            if (value is MonoObject monoObject)
            {
                return dictionary.ContainsKey(monoObject.Guid);
            }

            return false;
        }

        // Returns person name as value of $id
        public string GetReference(object context, object value)
        {
            if (value is MonoObject monoObject)
            {
                Guid guid = monoObject.Guid;
                dictionary[guid] = monoObject;
                return guid.ToString();
            }

            return null;
        }
    }
}
