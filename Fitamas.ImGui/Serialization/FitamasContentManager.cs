using Fitamas.ImGuiNet.Readers;
using Fitamas.Serialization;
using Fitamas.Serialization.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.ImGuiNet.Serialization
{
    public class FitamasContentManager : ContentManager
    {
        private EditorResourceManifest manifest;
        private List<IResourceReader> readers;
        private bool disposed = false;

        internal IEnumerable<object> InternalLoadedAssets => LoadedAssets.Values;

        public ICollection<IResourceReader> Readers => readers;

        public FitamasContentManager(IServiceProvider provider, string rootDirectory) : base(provider, rootDirectory)
        {
            manifest = (EditorResourceManifest)Resources.Manifest;

            readers = new List<IResourceReader>()
            {
                new Texture2DReader(),
                new MonoObjectReader(),
            };
        }

        public void Register(string assetName, object value)
        {
            string key = assetName.Replace('\\', '/');
            LoadedAssets[key] = value;

            Debug.Log($"Registering {value.GetType()}, {key}");
        }

        public override T Load<T>(string assetName)
        {
            return (T)LoadObject(assetName, typeof(T));
        }

        public object LoadObject(string assetName, Type type)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException(nameof(assetName));
            }

            if (disposed)
            {
                throw new ObjectDisposedException(nameof(FitamasContentManager));
            }

            if (Path.IsPathRooted(assetName))
            {
                throw new Exception();
            }

            string key = assetName.Replace('\\', '/');
            if (LoadedAssets.TryGetValue(key, out object value))
            {
                return value;
            }

            var resourceInfo = manifest.GetResource(assetName);

            string absolutePath = Path.Combine(RootDirectory, resourceInfo.Path);
            if (!File.Exists(absolutePath))
            {
                return null;
            }

            if (resourceInfo.Guid != Guid.Empty)
            {
                Type targetType = MonoJsonSerializer.DefaultSerializationBinder.BindToType(resourceInfo.Type);
                value = ReadAsset(absolutePath, targetType);
            }
            else
            {
                return null;
            }

            Register(key, value);

            if (value is GraphicsResource graphicsResource)
            {
                graphicsResource.Name = assetName;
            }
            else if (value is MonoContentObject contentObject)
            {
                contentObject.Guid = resourceInfo.Guid;
                contentObject.Name = assetName;
                contentObject.OnPostLoad();
            }

            return value;
        }

        protected object ReadAsset(string absolutePath, Type type)
        {
            foreach (var reader in readers)
            {
                if (reader.CanRead(type))
                {
                    return reader.Read(this, absolutePath, type);
                }
            }

            throw new ContentLoadException($"Could not load asset at path: {absolutePath}, of type: {type.FullName}");
        }

        protected override void ReloadAsset<T>(string originalAssetName, T currentAsset)
        {
            //TODO update assets

            base.ReloadAsset(originalAssetName, currentAsset);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            disposed = true;
        }
    }
}
