using Newtonsoft.Json;
using Fitamas.Serialization.Json.Converters;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Fitamas.Serialization.Json
{
    public class MonoJsonSerializer : JsonSerializer
    {
        public MonoJsonSerializer(ContentManager manager)
        {
            Converters.Add(new RangeJsonConverter<int>());
            Converters.Add(new RangeJsonConverter<float>());
            Converters.Add(new ArrayReferencePreservngConverter());

            Converters.Add(new Vector2JsonConverter());
            Converters.Add(new RectangleFJsonConverter());
            Converters.Add(new RectangleJsonConverter());
            Converters.Add(new ColorJsonConverter());

            Converters.Add(new ContentManagerJsonConverter<Texture2D>(manager, (texture) => { return texture.Name; }));
            Converters.Add(new ContentManagerJsonConverter<SpriteFont>(manager, (font) => { return font.Texture.Name; }));

            Converters.Add(new MonoObjectConverter(manager));

            ReferenceResolver = new MonoObjectResolver();
            PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            ContractResolver = new MonoContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.Auto;
        }
    }
}
