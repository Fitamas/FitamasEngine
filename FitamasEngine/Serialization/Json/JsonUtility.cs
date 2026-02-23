using Fitamas.Core;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fitamas.Serialization.Json
{
    public static class JsonUtility
    {
        public static T Load<T>(string path)
        {
            return (T)Load(path, typeof(T));
        }

        public static object Load(string path, Type type = null)
        {
            using (var reader = new StreamReader(path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new MonoJsonSerializer(GameEngine.Instance);
                return serializer.Deserialize(jsonReader, type);
            }
        }

        public static void LoadToObject(string path, object target)
        {
            using (var reader = new StreamReader(path))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var serializer = new MonoJsonSerializer(GameEngine.Instance);
                serializer.Populate(jsonReader, target);
            }
        }

        public static void Save(string path, object data)
        {
            using (var writer = new StreamWriter(path))
            using (JsonWriter jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new MonoJsonSerializer(GameEngine.Instance);
                serializer.Serialize(jsonWriter, data);
            }
        }
    }
}
