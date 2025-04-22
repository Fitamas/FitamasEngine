using System;

namespace WDL.DigitalLogic
{
    public static class LogicComponents
    {

        public static readonly LogicComponentDescription And = new LogicComponentDescription((manager, descriptoin, data) => new LogicAnd(descriptoin, data))
        {
            TypeId = "And",
            Namespace = "Default",
            Connectors =
            {
                { 0, "InA" },
                { 1, "InB" },
                { 2, "Out" },
            }
        };

        public static readonly LogicComponentDescription Not = new LogicComponentDescription((manager, descriptoin, data) => new LogicNot(descriptoin, data))
        {
            TypeId = "Not",
            Namespace = "Default",
            Connectors =
            {
                { 0, "In" },
                { 1, "Out" },
            }
        };

        public static readonly LogicComponentDescription Input = new LogicComponentDescription((manager, descriptoin, data) => new LogicInput(descriptoin, data))
        {
            TypeId = "Input",
            Namespace = "Default",
            Connectors =
            {
                { 0, "Out" },
            }
        };

        public static readonly LogicComponentDescription Output = new LogicComponentDescription((manager, descriptoin, data) => new LogicOutput(descriptoin, data))
        {
            TypeId = "Output",
            Namespace = "Default",
            Connectors =
            {
                { 0, "In" },
            }
        };
    }
}
