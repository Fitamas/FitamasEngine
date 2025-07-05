using System;

namespace WDL.DigitalLogic
{
    public class LogicConnectorData
    {
        public int Id;
        public int ComponentID;
        public string Name;

        public LogicConnectorData(int id, int componentId, string name)
        {
            Id = id;
            ComponentID = componentId;
            Name = name;
        }
    }
}
