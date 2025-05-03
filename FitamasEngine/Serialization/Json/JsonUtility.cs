using Fitamas.Core;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Fitamas.Serialization.Json
{
    public static class JsonUtility
    {
        public static T Load<T>(string path)
        {
            return (T)Load(path, typeof(T));
        }

        public static object Load(string path, Type type)
        {
            ContentManager manager = GameEngine.Instance.Content;
            using (var reader = new StreamReader(path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new MonoJsonSerializer(manager);
                return serializer.Deserialize(jsonReader, type);
            }
        }

        public static void Save(string path, object data)
        {
            ContentManager manager = GameEngine.Instance.Content;
            using (var writer = new StreamWriter(path))
            using (JsonWriter jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new MonoJsonSerializer(manager);
                serializer.Serialize(jsonWriter, data);
            }
        }
    }
}
