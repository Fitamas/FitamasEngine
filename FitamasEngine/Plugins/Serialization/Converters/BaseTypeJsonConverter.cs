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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MonoGame.Extended.Serialization
{
    public abstract class BaseTypeJsonConverter<T> : JsonConverter
    {
        private readonly string _suffix;
        private readonly Dictionary<string, Type> _namesToTypes;
        private readonly Dictionary<Type, string> _typesToNames;
        private readonly CamelCaseNamingStrategy _namingStrategy = new CamelCaseNamingStrategy();

        protected BaseTypeJsonConverter(IEnumerable<TypeInfo> supportedTypes, string suffix)
        {
            _suffix = suffix;
            _namesToTypes = supportedTypes
                .ToDictionary(t => TrimSuffix(t.Name, suffix), t => t.AsType(), StringComparer.OrdinalIgnoreCase);
            _typesToNames = _namesToTypes.ToDictionary(i => i.Value, i => i.Key);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var properties = type.GetRuntimeProperties();

            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(_typesToNames[type]);

            foreach (var property in properties.Where(p => p.CanWrite)) // TODO: Technically an IList property is writable as well.
            {
                var propertyName = _namingStrategy.GetPropertyName(property.Name, false);
                writer.WritePropertyName(propertyName);
                serializer.Serialize(writer, property.GetValue(value));
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var key = jObject.GetValue("type", StringComparison.OrdinalIgnoreCase).ToObject<string>();
            Type type;

            if (_namesToTypes.TryGetValue(key, out type))
            {
                serializer.Converters.Remove(this);
                var value = jObject.ToObject(type, serializer);
                serializer.Converters.Add(this);
                return value;
            }

            throw new InvalidOperationException($"Unknown {_suffix} type '{key}'");
        }

        public override bool CanConvert(Type objectType)
        {
            if (_namesToTypes.ContainsValue(objectType))
                return true;

            return objectType == typeof(T);
        }

        private static string TrimSuffix(string input, string suffix)
        {
            if (input.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                return input.Substring(0, input.Length - suffix.Length);

            return input;
        }
    }
}