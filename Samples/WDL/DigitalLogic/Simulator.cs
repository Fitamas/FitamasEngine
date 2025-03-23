using R3;
using Fitamas;
using ObservableCollections;

namespace WDL.DigitalLogic
{
    public class Simulator
    {
        private LogicComponentManager manager;
        private LogicComponentDescription component;
        private ObservableList<LogicComponent> components;
        private ObservableList<Connector> connectors;

        private int componentCount = 0;
        private int connectCount = 0;

        public Simulator(LogicComponentManager manager, LogicComponentDescription description)
        {
            this.manager = manager;
            this.component = description;
            components = new ObservableList<LogicComponent>();
            connectors = new ObservableList<Connector>();

            foreach (var item in description.Components)
            {
                AddComponent(item);
            }

            foreach (var item in description.Connections)
            {
                AddConnection(item);
            }

            components.ObserveAdd().Subscribe(component =>
            {
                description.Components.Add(component.Value.Data);
            });

            components.ObserveRemove().Subscribe(component =>
            {
                description.Components.Add(component.Value.Data);
            });

            connectors.ObserveAdd().Subscribe(connector =>
            {
                //description.Connections.Add(connector.Value.Data);
            });

            connectors.ObserveRemove().Subscribe(connector =>
            {
                //description.Connections.Add(connector.Value.Data);
            });
        }

        private void AddComponent(LogicComponentData data)
        {
            LogicComponentDescription description = manager.GetComponent(data.TypeId);
            LogicComponent component = description.CreateComponent(manager, data);
            components.Add(component);
        }

        private void AddConnection(LogicConnectionData connection)
        {
            LogicComponent outputComponent = components[connection.OutputComponentId];
            LogicComponent inputComponent = components[connection.InputComponentId];

            ConnectorOutput outputConnector = outputComponent.GetOutput(connection.OutputIndex);
            ConnectorInput inputConnector = inputComponent.GetInput(connection.InputIndex);

            CreateConnect(outputConnector, inputConnector);
        }

        public void Update()
        {
            foreach (LogicComponent component in components)
            {
                component.Update();
            }
        }

        public Connector GetConnector(int id)
        {
            foreach (var connector in connectors)
            {
                if (connector.Id == id)
                {
                    return connector;
                }
            }

            return null;
        }

        public LogicComponent GetComponent(Connector connector)
        {
            foreach (var component in components)
            {
                if (component.Contain(connector))
                {
                    return component;
                }
            }

            return null;
        }

        public void CreateConnect(int outputId, int inputId)
        {
            Connector con1 = GetConnector(outputId);
            Connector con2 = GetConnector(inputId);

            if (con1 is ConnectorOutput output && con2 is ConnectorInput input)
            {
                CreateConnect(output, input);
            }
        }

        public void CreateConnect(ConnectorOutput output, ConnectorInput input)
        {
            output.Connectors.Add(input);
        }

        public void RemoveConnect(int outputId, int inputId)
        {
            Connector con1 = GetConnector(outputId);
            Connector con2 = GetConnector(inputId);

            if (con1 is ConnectorOutput output && con2 is ConnectorInput input)
            {
                CreateConnect(output, input);
            }
        }

        public void RemoveConnect(ConnectorOutput output, ConnectorInput input)
        {
            output.Connectors.Remove(input);
        }

        public void AddComponent(LogicComponent component)
        {
            if (!components.Contains(component))
            {
                componentCount++;
                components.Add(component);

                for (int i = 0; i < component.InputCount; i++)
                {
                    var connector = new ConnectorInput(connectCount);
                    component.SetInput(connector, i);
                    connectors.Add(connector);
                    connectCount++;
                }

                for (int i = 0; i < component.OutputCount; i++)
                {
                    var connector = new ConnectorOutput(connectCount);
                    component.SetOutput(connector, i);
                    connectors.Add(connector);
                    connectCount++;
                }
            }
        }

        public void RemoveComponent(LogicComponent component)
        {
            if (components.Contains(component))
            {
                components.Remove(component);

                for (int i = 0; i < component.InputCount; i++)
                {
                    ConnectorInput input = component.GetInput(i);
                    foreach (var component1 in components)
                    {
                        for (int j = 0; j < component1.OutputCount; j++)
                        {
                            RemoveConnect(component1.GetOutput(j), input);
                        }
                    }
                    connectors.Remove(input);
                }

                for (int i = 0; i < component.OutputCount; i++)
                {
                    ConnectorOutput output = component.GetOutput(i);
                    connectors.Remove(output);
                }
            }
        }
    }
}
