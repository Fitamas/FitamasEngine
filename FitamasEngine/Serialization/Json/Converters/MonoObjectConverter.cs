using Fitamas.Core;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Fitamas.Serialization.Json.Converters
{
    public class MonoObjectConverter : JsonConverter<MonoContentObject>
    {
        private ContentManager manager;
        private IResourceManifest manifest;
        private MethodInfo loadMethod;
        private bool canRead = false;
        private bool canWrite = false;

        public MonoObjectConverter(GameEngine game, IResourceManifest manifest)
        {
            manager = game.Content;
            this.manifest = manifest;

            loadMethod = manager.GetType().GetMethod(nameof(manager.Load));
        }

        public override void WriteJson(JsonWriter writer, MonoContentObject value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if (canWrite && manifest.Contains(value.Guid))
            {
                writer.WriteValue(value.Guid);
            }
            else
            {
                canWrite = true;

                Type type = value.GetType();

                writer.WriteStartObject();

                serializer.SerializationBinder.BindToName(type, out string assemlyName, out string typeName);

                writer.WritePropertyName("$type");
                if (serializer.TypeNameAssemblyFormatHandling == TypeNameAssemblyFormatHandling.Simple)
                {
                    writer.WriteValue($"{type.FullName}, {type.Assembly.GetName().Name}");
                }
                else
                {
                    writer.WriteValue($"{typeName}, {assemlyName}");
                }

                writer.WritePropertyName("$id");
                writer.WriteValue(value.Guid);

                if (serializer.ContractResolver == null)
                {
                    return;
                }

                var contract = serializer.ContractResolver.ResolveContract(type) as JsonObjectContract;
                if (contract == null)
                {
                    return;
                }

                foreach (var property in contract.Properties)
                {
                    if (!property.ShouldSerialize?.Invoke(value) ?? false)
                        continue;

                    if (property.Ignored)
                        continue;

                    if (property.PropertyName == "$type")
                        continue;

                    var propValue = property.ValueProvider?.GetValue(value);

                    if (propValue == null && serializer.NullValueHandling == NullValueHandling.Ignore)
                        continue;

                    writer.WritePropertyName(property.PropertyName);
                    serializer.Serialize(writer, propValue);
                }

                writer.WriteEndObject();
            }
        }

        public override MonoContentObject ReadJson(JsonReader reader, Type objectType, MonoContentObject existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (canRead && manifest.Contains(reader.Value))
            {
                string path = manifest.GetPath(reader.Value);

                if (!string.IsNullOrEmpty(path))
                {
                    MethodInfo genericMethod = loadMethod.MakeGenericMethod(objectType);
                    return (MonoContentObject)genericMethod.Invoke(manager, [path]);
                }
            }
            else
            {
                canRead = true;

                var jObject = JObject.Load(reader);

                Type targetType = objectType;
                if (jObject.TryGetValue("$type", out JToken typeToken))
                {
                    string typeName = typeToken.Value<string>();
                    targetType = serializer.SerializationBinder.BindToType(null, typeName) ?? objectType;
                }

                MonoContentObject instance = (MonoContentObject)Activator.CreateInstance(targetType, nonPublic: true);

                string reference = jObject.GetValue("$id").Value<string>();
                instance.Guid = Guid.Parse(reference);

                serializer.ReferenceResolver.AddReference(null, reference, instance);

                serializer.Populate(jObject.CreateReader(), instance);

                return instance;
            }

            return null;
        }
    }
}
