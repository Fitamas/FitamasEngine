using Fitamas.Audio;
using Fitamas.Core;
using Fitamas.Fonts;
using Fitamas.Graphics;
using Fitamas.ImGuiNet.Assets;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.ImGuiNet.Importers
{
    public interface IResourceImporter
    {
        HashSet<string> SupportedExtensions { get; }
        Asset Import(string path);
    }

    public abstract class ResourceImporter<T> : IResourceImporter
    {
        public abstract HashSet<string> SupportedExtensions { get; }

        public virtual Asset Import(string path)
        {
            return AssetSystem.ImportAsset(Guid.NewGuid(), path, typeof(T));
        }
    }


    public class Textture2DImporter : ResourceImporter<Texture2D>
    {
        public override HashSet<string> SupportedExtensions => TextureHelper.Extensions;
    }

    public class AudionWavImporter : ResourceImporter<AudioClip>
    {
        public override HashSet<string> SupportedExtensions => [".wav", ".ogg"];
    }

    public class FontAssetImporter : ResourceImporter<FontAsset>
    {
        public override HashSet<string> SupportedExtensions => [".ttf"];
    }

    public class MonoObjectImporter : IResourceImporter
    {
        public HashSet<string> SupportedExtensions => [".json"];

        public Asset Import(string path)
        {
            string absolutePath = Path.Combine(GameEngine.Instance.Content.RootDirectory, path);

            using (var reader = new StreamReader(absolutePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                JObject obj = JObject.Load(jsonReader);

                if (!obj.TryGetValue("$type", out JToken typeToken) ||
                    !obj.TryGetValue("$id", out JToken guidToken))
                {
                    return null;
                }

                string typeName = typeToken.Value<string>();
                Guid guid = Guid.Parse(guidToken.Value<string>());

                return AssetSystem.ImportAsset(guid, path, typeName);
            }
        }
    }
}
