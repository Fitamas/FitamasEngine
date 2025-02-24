using Fitamas.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Fitamas.Serialization.Json
{
    public static class JsonUtility
    {
        public static T Load<T>(ObjectManager manager, string path)
        {
            return (T)Load(manager, path, typeof(T));
        }

        public static object Load(ObjectManager manager, string path, Type type)
        {
            JsonContentLoader loader = new JsonContentLoader();

            return loader.Load(manager, path, type);
        }

        public static void Save(ObjectManager manager, string path, object data)
        {
            string contentPath = Path.Combine(manager.RootDirectory, path);
            var serializer = new MonoGameJsonSerializer(manager);

            using (var sw = new StreamWriter(contentPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
