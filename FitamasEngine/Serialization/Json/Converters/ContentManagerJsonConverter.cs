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
using System.IO;
using Fitamas.Serialization;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;

namespace Fitamas.Serialization.Json.Converters
{
    /// <summary>
    /// Loads content from a JSON file into the <see cref="ContentManager"/> using the asset name
    /// </summary>
    /// <typeparam name="T">The type of content to load</typeparam>
    public class ContentManagerJsonConverter<T> : JsonConverter
    {
        private readonly ObjectManager _objectManager;
        private readonly ContentManager _contentManager;
        private readonly Func<T, string> _getAssetName;

        public ContentManagerJsonConverter(ObjectManager objectManager, Func<T, string> getAssetName)
        {
            _objectManager = objectManager;
            _contentManager = objectManager.Game.Content;
            _getAssetName = getAssetName;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var asset = (T)value;
            var assetName = _getAssetName(asset);
            writer.WriteValue(assetName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string assetName = (string)reader.Value;

            if (string.IsNullOrEmpty(assetName))
            {
                return null;
            }

            if (_objectManager.ReadOnlyXNB)
            {
                assetName = Path.ChangeExtension(assetName, null);
            }

            return _contentManager.Load<T>(assetName);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}