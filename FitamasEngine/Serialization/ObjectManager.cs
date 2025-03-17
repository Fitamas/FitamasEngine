using Fitamas.Serialization.Json;
using Fitamas.UserInterface.Serializeble;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.Serialization
{
    public class ObjectManager
    {
        private Game game;
        private string rootDirectory = string.Empty;
        private Dictionary<string, MonoObject> loadObjects = new Dictionary<string, MonoObject>();

        public bool ReadOnlyXNB { get; }

        public string RootDirectory
        {
            get
            {
                return rootDirectory;
            }
            set
            {
                rootDirectory = value;
            }
        }

        public Game Game => game;
        public Dictionary<string , MonoObject> LoadObjects => loadObjects;

        public ObjectManager(Game game, string root, bool readOnlyXNB = true)
        {
            this.game = game;
            rootDirectory = root;
            ReadOnlyXNB = readOnlyXNB;
        }

        public bool Contains(MonoObject asset)
        {
            return loadObjects.ContainsValue(asset);
        }

        public bool TryGetPath(MonoObject asset, out string path)
        {
            path = null;

            foreach (var item in loadObjects)
            {
                if (item.Value == asset)
                {
                    path = item.Key;
                    return true;
                }
            }

            return false;
        }

        public void UnloadAsset(MonoObject asset)
        {
            foreach (var item in loadObjects)
            {
                if (item.Value == asset)
                {
                    UnloadAsset(item.Key);
                    return;
                }
            }
        }

        public void UnloadAsset(string path)
        {
            loadObjects.Remove(path);
        }

        public T LoadAsset<T>(string path) where T : MonoObject
        {
            return (T)LoadAsset(path, typeof(T));
        }

        public MonoObject LoadAsset(string path)
        {
            string extension = Path.GetExtension(path);
            Type type = ExtensionUtils.GetType(extension);

            if (type == null) return null;

            return LoadAsset(path, type);
        }

        public MonoObject LoadAsset(string path, Type type)
        {
            if (loadObjects.TryGetValue(path, out MonoObject value))
            {
                return value;
            }
            else
            {
                if (!File.Exists(Path.Combine(rootDirectory, path)))
                {
                    return null;
                }

                MonoObject monoObject = (MonoObject)JsonUtility.Load(this, path, type);

                loadObjects.Add(path, monoObject);
                return monoObject;
            }
        }
    }
}
