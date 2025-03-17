using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WDL.Gameplay.DigitalLogic
{
    public class CircuitConverter : JsonConverter<Component>
    {
        private LogicComponentManager manager;

        public CircuitConverter(LogicComponentManager manager)
        {
            this.manager = manager;
        }

        public override Component ReadJson(JsonReader reader, Type objectType, Component existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Component component = new Component();
            JObject obj = JObject.Load(reader);
            JToken token;

            token = obj["Id"];
            if (token != null)
            {
                component.Id = token.ToObject<int>();
            }
            else
            {
                return existingValue;
            }

            string name = obj["Component"]?.ToObject<string>();
            LogicComponentDescription componentDescription = manager.GetComponent(name);
            if (componentDescription != null)
            {
                component.Description = componentDescription;
            }
            else
            {
                return existingValue;
            }

            token = obj["Position"];
            if (token != null)
            {
                component.Position = token.ToObject<Point>();
            }

            return component;
        }

        public override void WriteJson(JsonWriter writer, Component value, JsonSerializer serializer)
        {
            if (!value.NotNull)
            {
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName("Id");
            writer.WriteValue(value.Id);

            writer.WritePropertyName("Component");
            writer.WriteValue(value.Description.FullName);

            writer.WritePropertyName("Position");
            serializer.Serialize(writer, value.Position);

            writer.WriteEndObject();
        }
    }
}
