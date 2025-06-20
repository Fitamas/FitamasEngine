using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Fitamas.Serialization.Json.Converters
{
    public class MonoObjectConverter : JsonConverter<MonoContentObject>
    {
        private ContentManager manager;
        private MethodInfo loadMethod;
        private bool canRead = false;
        private bool canWrite = false;

        public override bool CanRead
        {
            get
            {
                bool value = canRead;
                canRead = true;
                return value;
            }
        }

        public override bool CanWrite
        {
            get
            {
                bool value = canWrite;
                canWrite = true;
                return value;
            }
        }

        public MonoObjectConverter(ContentManager manager)
        {
            this.manager = manager;

            loadMethod = manager.GetType().GetMethod(nameof(manager.Load));
        }

        public override void WriteJson(JsonWriter writer, MonoContentObject value, JsonSerializer serializer)
        {
            if (value != null)
            {
                writer.WriteValue(value.Name);
            }
        }

        public override MonoContentObject ReadJson(JsonReader reader, Type objectType, MonoContentObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JValue value = new JValue(reader.Value);

            string path = (string)value.Value;

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            MethodInfo genericMethod = loadMethod.MakeGenericMethod(objectType);
            return (MonoContentObject)genericMethod.Invoke(manager, [path]);
        }
    }
}
