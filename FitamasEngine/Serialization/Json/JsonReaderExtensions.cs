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
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitamas.Serialization.Json
{
    public static class JsonReaderExtensions
    {
        private static readonly Dictionary<Type, Func<string, object>> _stringParsers = new Dictionary<Type, Func<string, object>>
        {
            {typeof(int), s => int.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
            {typeof(float), s => float.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
        };

        public static T[] ReadAsMultiDimensional<T>(this JsonReader reader)
        {
            var tokenType = reader.TokenType;

            switch (tokenType)
            {
                case JsonToken.StartArray:
                    return reader.ReadAsJArray<T>();

                case JsonToken.String:
                    return reader.ReadAsDelimitedString<T>();

                case JsonToken.Integer:
                case JsonToken.Float:
                    return reader.ReadAsSingleValue<T>();

                default:
                    throw new NotSupportedException($"{tokenType} is not currently supported in the multi dimensional parser");
            }
        }

        private static T[] ReadAsSingleValue<T>(this JsonReader reader)
        {
            return new[] { JToken.Load(reader).ToObject<T>() };
        }

        private static T[] ReadAsJArray<T>(this JsonReader reader)
        {
            var jArray = JArray.Load(reader);
            var items = new List<T>();

            foreach (var token in jArray)
            {
                if (token.Type == JTokenType.String)
                {
                    var stringParser = _stringParsers[typeof(T)];
                    var s = token.Value<string>();
                    items.Add((T)stringParser(s));
                }
                else
                {
                    items.Add(token.Value<T>());
                }
            }

            return items.ToArray();
        }

        private static T[] ReadAsDelimitedString<T>(this JsonReader reader)
        {
            var value = (string)reader.Value;
            var parser = _stringParsers[typeof(T)];
            return value.Split(' ')
                .Select(i => parser(i))
                .Cast<T>()
                .ToArray();
        }
    }
}