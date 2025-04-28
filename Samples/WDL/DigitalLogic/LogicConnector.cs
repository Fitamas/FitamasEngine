using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using WDL.DigitalLogic.Components;

namespace WDL.DigitalLogic
{
    public abstract class LogicConnector
    {
        private int id;

        public int Id => id;
        public LogicComponent Component { get; }

        public LogicConnector( int id, LogicComponent component)
        {
            this.id = id;
            Component = component;
        }

        public abstract bool CanConnect(LogicConnector connector);

        public abstract bool Contain(LogicConnector connector);

        public abstract void Connect(LogicConnection connection);

        public abstract void Destroy(LogicConnection connection);

        public abstract void DestroyAllConnects();
    }

    public class LogicConnectorInput : LogicConnector
    {
        private LogicConnection connection;

        public LogicSignal Signal
        {
            get
            {
                if (connection != null)
                {
                    return connection.Signal.Value;
                }

                return new LogicSignal(false);
            }
        }

        public LogicConnectorInput(int id, LogicComponent component) : base(id, component)
        {

        }

        public override bool CanConnect(LogicConnector connector)
        {
            return connector is LogicConnectorOutput && !Contain(connector);
        }

        public override bool Contain(LogicConnector connector)
        {
            return connection != null && connection.Output == connector;
        }

        public override void Connect(LogicConnection connection)
        {
            this.connection = connection;
        }

        public override void Destroy(LogicConnection connection)
        {
            this.connection = null;
        }

        public override void DestroyAllConnects()
        {
            Destroy(connection);
        }
    }

    public class LogicConnectorOutput : LogicConnector
    {
        private List<LogicConnection> connections;

        public LogicConnectorOutput(int id, LogicComponent component) : base(id, component)
        {
            connections = new List<LogicConnection>();
        }

        public override bool CanConnect(LogicConnector connector)
        {
            return connector is LogicConnectorInput && !Contain(connector);
        }

        public override bool Contain(LogicConnector connector)
        {
            return connections.Any(c => c.Input == connector);
        }

        public override void Connect(LogicConnection connection)
        {
            connections.Add(connection);
        }

        public override void Destroy(LogicConnection connection)
        {
            connections.Remove(connection);
        }

        public override void DestroyAllConnects()
        {
            connections.Clear();
        }

        public void ReceiveInput(LogicSignal signal)
        {
            foreach (var connector in connections)
            {
                connector.Signal.Value = signal;
            }
        }
    }
}
