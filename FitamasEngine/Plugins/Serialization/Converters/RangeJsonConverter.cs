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
using Fitamas.Math2D;
using Newtonsoft.Json;

namespace MonoGame.Extended.Serialization
{
    public class RangeJsonConverter<T> : JsonConverter where T : IComparable<T>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var range = (Range<T>) value;

            var formatting = writer.Formatting;
            writer.Formatting = Formatting.None;
            writer.WriteWhitespace(" ");
            writer.WriteStartArray();
            serializer.Serialize(writer, range.Min);
            serializer.Serialize(writer, range.Max);
            //writer.WriteValue(range.Min);
            //writer.WriteValue(range.Max);
            writer.WriteEndArray();
            writer.Formatting = formatting;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var values = reader.ReadAsMultiDimensional<T>();

            if (values.Length == 2)
            {
                if (values[0].CompareTo(values[1]) < 0)
                    return new Range<T>(values[0], values[1]);

                return new Range<T>(values[1], values[0]);
            }
                

            if (values.Length == 1)
                return new Range<T>(values[0], values[0]);

            throw new InvalidOperationException("Invalid range");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Range<T>);
        }
    }
}