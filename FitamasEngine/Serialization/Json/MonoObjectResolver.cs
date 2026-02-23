using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Fitamas.Serialization.Json
{
    public class MonoObjectResolver : IReferenceResolver
    {
        private readonly IDictionary<Guid, object> _referenceObjects;
        private readonly IDictionary<object, Guid> _objectReferences;

        public MonoObjectResolver()
        {
            _referenceObjects = new Dictionary<Guid, object>();
            _objectReferences = new Dictionary<object, Guid>();
        }

        // This method is called during deserialize for $id
        public void AddReference(object context, string reference, object value)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                Guid id = Guid.Parse(reference);
                _referenceObjects[id] = value;
                _objectReferences[value] = id;
                if (value is MonoObject monoObject)
                {
                    monoObject.Guid = id;
                }
            }
        }

        // This method is called during deserialize for $ref
        public object ResolveReference(object context, string reference)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                Guid id = Guid.Parse(reference);

                _referenceObjects.TryGetValue(id, out object monoObject);

                return monoObject;
            }

            return null;
        }

        // Returns false, so that $id is used, not $ref.
        public bool IsReferenced(object context, object value)
        {
            return _objectReferences.ContainsKey(value);
        }

        // Returns person name as value of $id
        public string GetReference(object context, object value)
        {
            if (_objectReferences.TryGetValue(value, out Guid guid))
            {
                return guid.ToString();
            }

            if (value is MonoObject monoObject)
            {
                guid = monoObject.Guid;
            }
            else
            {
                guid = Guid.NewGuid();
            }

            _objectReferences[value] = guid;
            _referenceObjects[guid] = value;

            return guid.ToString();
        }
    }
}
