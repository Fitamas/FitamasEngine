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

using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System;
using Fitamas.Core;
using Fitamas.Serialization.Json.Converters;

namespace Fitamas.Serialization.Json
{
    public sealed class MonoGameJsonSerializer : JsonSerializer
    {
        public MonoGameJsonSerializer(ObjectManager objectManager)
        {
            Converters.Add(new RangeJsonConverter<int>());
            Converters.Add(new RangeJsonConverter<float>());
            Converters.Add(new ArrayReferencePreservngConverter());

            Converters.Add(new Vector2JsonConverter());
            Converters.Add(new RectangleFJsonConverter());
            Converters.Add(new RectangleJsonConverter());
            Converters.Add(new ColorJsonConverter());

            Converters.Add(new ContentManagerJsonConverter<Texture2D>(objectManager, (texture) => { return texture.Name; }));
            Converters.Add(new ContentManagerJsonConverter<SpriteFont>(objectManager, (font) => { return font.Texture.Name; }));

            Converters.Add(new MonoObjectConverter(objectManager));

            ReferenceResolver = new MonoObjectResolver();
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            ContractResolver = new MyContractResolver();
            NullValueHandling = NullValueHandling.Ignore;// Include;
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.Auto;
        }
    }

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
                Guid id = new Guid(reference);

                if (value is MonoObject monoObject)
                {
                    dictionary[id] = monoObject;
                }
            }
        }

        // This method is called during deserialize for $ref
        public object ResolveReference(object context, string reference)
        {
            if (!string.IsNullOrEmpty(reference))
            {
                Guid id = new Guid(reference);

                MonoObject monoObject;
                dictionary.TryGetValue(id, out monoObject);

                return monoObject;
            }

            return null;
        }

        // Returns false, so that $id is used, not $ref.
        public bool IsReferenced(object context, object value)
        {
            if (value is MonoObject monoObject)
            {
                return dictionary.ContainsKey(monoObject.GetGuid());
            }

            return false;
        }

        // Returns person name as value of $id
        public string GetReference(object context, object value)
        {
            if (value is MonoObject monoObject)
            {
                Guid guid = monoObject.GetGuid();
                dictionary[guid] = monoObject;
                return guid.ToString();
            }

            return null;
        }
    }

    public class MyContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<JsonProperty> properties = new List<JsonProperty>();
            FieldInfo[] fields = type.GetSerializedFields();

            foreach (FieldInfo field in fields)
            {
                JsonProperty prop = CreateProperty(field, memberSerialization);
                prop.Writable = true;
                prop.Readable = true;
                properties.Add(prop);
            }

            return properties;
        }
    }
}
