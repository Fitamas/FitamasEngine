using Fitamas.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.Serialization.Json
{
    public class MonoContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<JsonProperty> properties = new List<JsonProperty>();
            FieldInfo[] fields = type.GetSerializedFields();

            foreach (FieldInfo field in fields)
            {
                JsonProperty prop = CreateProperty(field, memberSerialization);
                prop.Writable = true;
                prop.Readable = true;
                properties.Add(prop);
            }

            return properties;
        }
    }
}
