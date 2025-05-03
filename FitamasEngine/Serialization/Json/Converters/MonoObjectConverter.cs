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

using Fitamas;
using Fitamas.Entities;
using Fitamas.Scene;
using Fitamas.Serialization;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Fitamas.Serialization.Json.Converters
{
    public class MonoObjectConverter<T> : JsonConverter where T : MonoObject
    {
        private ContentManager manager;
        private bool canRead = false;
        private bool canWrite = false;

        public MonoObjectConverter(ContentManager manager)
        {
            this.manager = manager;
        }

        public override bool CanRead
        {
            get
            {
                bool value = canRead;
                canRead = true;
                return value;
            }
        }

        public override bool CanWrite
        {
            get
            {
                bool value = canWrite;
                canWrite = true;
                return value;
            }
        }

        public sealed override bool CanConvert(Type objectType)
        {
            return typeof(MonoObject).IsAssignableFrom(objectType) &&
                  !(typeof(GameObject).IsAssignableFrom(objectType) ||
                   typeof(Component).IsAssignableFrom(objectType));
        }

        public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool flag = existingValue == null;
            if (!flag && !(existingValue is MonoObject))
            {
                throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.");
            }

            return ReadJson(reader, objectType, flag ? default : (MonoObject)existingValue, !flag, serializer);
        }

        public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                if (value is MonoObject monoObject)
                {
                    WriteJson(writer, monoObject, serializer);
                }
                else
                {
                    throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.");
                }
            }
        }

        public MonoObject ReadJson(JsonReader reader, Type objectType, MonoObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JValue value = new JValue(reader.Value);

            string path = (string)value.Value;

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return manager.Load<T>(path);
        }

        //public void WriteJson(JsonWriter writer, MonoObject value, JsonSerializer serializer)
        //{
        //    if (manager.TryGetPath(value, out string path))
        //    {
        //        serializer.Serialize(writer, path);
        //    }
        //}
    }
}
