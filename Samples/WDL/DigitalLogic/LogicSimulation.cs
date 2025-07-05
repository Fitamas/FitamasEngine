using R3;
using Fitamas;
using ObservableCollections;
using WDL.DigitalLogic.Components;
using Microsoft.Xna.Framework;

namespace WDL.DigitalLogic
{
    public class LogicSimulation
    {
        private LogicComponentManager manager;
        private LogicComponentDescription description;
        private ObservableList<LogicComponent> components;
        private ObservableList<LogicConnection> connections;
        private ReactiveProperty<int> componentCount;
        private ReactiveProperty<int> connectionCount;
        private ReactiveProperty<int> connectorCount;

        public LogicComponentDescription Description => description;
        public IObservableCollection<LogicComponent> Components => components;
        public IObservableCollection<LogicConnection> Connections => connections;

        public LogicSimulation(LogicComponentManager manager, LogicComponentDescription description)
        {
            this.manager = manager;
            this.description = description;

            componentCount = new ReactiveProperty<int>(description.ComponentCount);
            componentCount.Subscribe(value => description.ComponentCount = value);

            connectionCount = new ReactiveProperty<int>(description.ConnectionCount);
            connectionCount.Subscribe(value => description.ConnectionCount = value);

            connectorCount = new ReactiveProperty<int>(description.ConnectorCount);
            connectorCount.Subscribe(value => description.ConnectorCount = value);

            components = new ObservableList<LogicComponent>();
            foreach (var item in description.Components)
            {
                AddComponent(item);
            }
            components.ObserveAdd().Subscribe(component =>
            {
                description.Components.Add(component.Value.Data);

                if (component.Value.Description == LogicComponents.Input)
                {
                    int id = connectorCount.Value++;
                    description.InputConnectors.Add(new LogicConnectorData(id, component.Value.Id, $"In{id}"));
                }
                else if (component.Value.Description == LogicComponents.Output)
                {
                    int id = connectorCount.Value++;
                    description.OutputConnectors.Add(new LogicConnectorData(id, component.Value.Id, $"Out{id}"));
                }
            });
            components.ObserveRemove().Subscribe(component =>
            {
                description.Components.Remove(component.Value.Data);

                if (component.Value.Description == LogicComponents.Input)
                {
                    description.InputConnectors.RemoveAll(x => x.ComponentID == component.Value.Id);
                }
                else if (component.Value.Description == LogicComponents.Output)
                {
                    description.OutputConnectors.RemoveAll(x => x.ComponentID == component.Value.Id);
                }
            });

            connections = new ObservableList<LogicConnection>();
            foreach (var item in description.Connections)
            {
                AddConnection(item);
            }
            connections.ObserveAdd().Subscribe(connector =>
            {
                description.Connections.Add(connector.Value.Data);
            });
            connections.ObserveRemove().Subscribe(connector =>
            {
                description.Connections.Remove(connector.Value.Data);
            });
        }

        private void AddComponent(LogicComponentData data)
        {
            LogicComponentDescription description = manager.GetComponent(data.TypeId);

            if (description != null)
            {
                LogicComponent component = description.CreateComponent(manager, data);
                components.Add(component);
            }
        }

        private void AddConnection(LogicConnectionData connection)
        {
            LogicComponent outputComponent = GetComponent(connection.OutputComponentId);
            LogicComponent inputComponent = GetComponent(connection.InputComponentId);

            if (outputComponent == null || inputComponent == null)
            {
                return;
            }

            LogicConnectorOutput outputConnector = outputComponent.GetOutputFromId(connection.OutputId);
            LogicConnectorInput inputConnector = inputComponent.GetInputFromId(connection.InputId);

            if (outputConnector == null || inputConnector == null)
            {
                return;
            }

            LogicConnection logicConnection = new LogicConnection(connection, inputConnector, outputConnector);
            outputConnector.Connect(logicConnection);
            inputConnector.Connect(logicConnection);
            connections.Add(logicConnection);
        }

        public void Update()
        {
            foreach (LogicComponent component in components)
            {
                component.Update();
            }
        }

        public LogicComponent GetComponent(int id)
        {
            foreach (var component in components)
            {
                if (component.Id == id)
                {
                    return component;
                }
            }

            return null;
        }

        public LogicConnection GetConnection(int id)
        {
            foreach (var connection in connections)
            {
                if (connection.Id == id)
                {
                    return connection;
                }
            }

            return null;
        }

        public LogicConnection CreateConnection(int componentOutputId, int outputId, int componentInputId, int inputId)
        {
            LogicConnectorOutput output = GetComponent(componentOutputId).GetOutputFromId(outputId);
            LogicConnectorInput input = GetComponent(componentInputId).GetInputFromId(inputId);

            return CreateConnection(output, input);
        }

        public LogicConnection CreateConnection(LogicConnectorOutput output, LogicConnectorInput input)
        {
            if (output.CanConnect(input) && input.CanConnect(output))
            {
                LogicConnectionData data = new LogicConnectionData()
                {
                    Id = connectionCount.Value,
                    OutputComponentId = output.Component.Id,
                    OutputId = output.Id,
                    InputComponentId = input.Component.Id,
                    InputId = input.Id,
                };
                LogicConnection connection = new LogicConnection(data, input, output);
                output.Connect(connection);
                input.Connect(connection);
                connections.Add(connection);
                connectionCount.Value++;
                return connection;
            }

            return null;
        }

        public void DestroyConnection(int componentOutputId, int outputId, int componentInputId, int inputId)
        {
            LogicConnectorOutput output = GetComponent(componentInputId).GetOutputFromId(inputId);
            LogicConnectorInput input = GetComponent(componentOutputId).GetInputFromId(outputId);

            DestroyConnection(output, input);
        }

        public void DestroyConnection(LogicConnectorOutput output, LogicConnectorInput input)
        {
            foreach (var connection in connections)
            {
                if (connection.Output == output && connection.Input == input)
                {
                    DestroyConnection(connection);
                    break;
                }
            }
        }

        public void DestroyConnection(int id)
        {
            DestroyConnection(GetConnection(id));
        }

        public void DestroyConnection(LogicConnection connection)
        {
            connection.Output.Destroy(connection);
            connection.Input.Destroy(connection);
            connections.Remove(connection);
        }

        public LogicComponent CreateComponent(LogicComponentDescription description, Point position)
        {
            if (Description == description)
            {
                throw new System.Exception("simulation == description");
            }

            LogicComponentData data = new LogicComponentData()
            {
                TypeId = description.FullName,
                Id = componentCount.Value
            };
            LogicComponent component = description.CreateComponent(manager, data);
            component.Position.Value = position;
            components.Add(component);
            componentCount.Value++;

            return component;
        }

        public void DestroyComponent(int id)
        {
            DestroyComponent(GetComponent(id));
        }

        public void DestroyComponent(LogicComponent component)
        {
            if (components.Contains(component))
            {
                for (int i = 0; i < component.InputCount; i++)
                {
                    LogicConnectorInput input = component.GetInputFromIndex(i);
                    input.DestroyAllConnects();
                }

                for (int i = 0; i < component.OutputCount; i++)
                {
                    LogicConnectorOutput output = component.GetOutputFromIndex(i);
                    output.DestroyAllConnects();
                }

                components.Remove(component);
            }
        }
    }
}
