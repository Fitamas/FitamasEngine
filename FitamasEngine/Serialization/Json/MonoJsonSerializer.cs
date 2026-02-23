using Newtonsoft.Json;
using Fitamas.Serialization.Json.Converters;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Fitamas.Core;
using Newtonsoft.Json.Serialization;
using System;

namespace Fitamas.Serialization.Json
{
    public class MonoJsonSerializer : JsonSerializer
    {
        public static readonly MonoSerializationBinder DefaultSerializationBinder = new MonoSerializationBinder();

        public MonoJsonSerializer(GameEngine game)
        {
            Converters.Add(new RangeJsonConverter<int>());
            Converters.Add(new RangeJsonConverter<float>());
            Converters.Add(new ArrayReferencePreservngConverter());

            Converters.Add(new Vector2JsonConverter());
            Converters.Add(new RectangleFJsonConverter());
            Converters.Add(new RectangleJsonConverter());
            Converters.Add(new ColorJsonConverter());

            Converters.Add(new ContentManagerJsonConverter<Texture2D>(game.Content, (texture) => { return texture.Name; }));
            Converters.Add(new ContentManagerJsonConverter<SpriteFont>(game.Content, (font) => { return font.Texture.Name; }));

            Converters.Add(new MonoObjectConverter(game, Resources.Manifest));

            ReferenceResolver = new MonoObjectResolver();
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            ContractResolver = new MonoContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.Auto;

            SerializationBinder = DefaultSerializationBinder;
        }
    }

    public class MonoSerializationBinder : DefaultSerializationBinder
    {
        public Type BindToType(string fullType)
        {
            string[] strings = fullType.Split(',', StringSplitOptions.RemoveEmptyEntries);
            string typeName = strings[0];
            if (strings.Length == 1)
            {
                return BindToType(string.Empty, typeName);
            }
            else
            {
                string assembly = strings[1];
                return BindToType(assembly, typeName);
            }
        }

        public void BindToName(Type serializedType, out string fullType)
        {
            fullType = $"{serializedType.FullName}, {serializedType.Assembly.GetName().Name}";
        }
    }
}