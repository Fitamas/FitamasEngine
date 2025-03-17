using System;
using System.Collections.Generic;

namespace WDL.Gameplay.DigitalLogic
{
    public abstract class Connector
    {
        private int id;

        public Signal Signal { get; set; }

        public int Id => id;

        public Connector(int id)
        {
            this.id = id;
        }
    }

    public class ConnectorInput : Connector
    {
        public ConnectorInput(int id) : base(id)
        {

        }
    }

    public class ConnectorOutput : Connector
    {
        private List<ConnectorInput> connectors;

        public List<ConnectorInput> Connectors => connectors;

        public ConnectorOutput(int id) : base(id)
        {
            connectors = new List<ConnectorInput>();
        }

        public void ReceiveInput(Signal signal)
        {
            Signal = signal;

            foreach (Connector connector in connectors)
            {
                connector.Signal = signal;
            }
        }
    }
}
