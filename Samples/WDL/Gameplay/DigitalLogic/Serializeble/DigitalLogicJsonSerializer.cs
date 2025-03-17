using Fitamas.Serialization.Json;
using Fitamas.Serialization.Json.Converters;
using Newtonsoft.Json;
using System;

namespace WDL.Gameplay.DigitalLogic
{
    public class DigitalLogicJsonSerializer : JsonSerializer
    {
        public DigitalLogicJsonSerializer(LogicComponentManager manager)
        {
            Converters.Add(new RangeJsonConverter<int>());
            Converters.Add(new RangeJsonConverter<float>());
            Converters.Add(new ArrayReferencePreservngConverter());

            Converters.Add(new Vector2JsonConverter());
            Converters.Add(new RectangleFJsonConverter());
            Converters.Add(new RectangleJsonConverter());
            Converters.Add(new ColorJsonConverter());

            Converters.Add(new CircuitConverter(manager));

            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = new MyContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            TypeNameHandling = TypeNameHandling.Auto;
        }
    }
}
