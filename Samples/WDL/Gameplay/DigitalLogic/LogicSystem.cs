using Fitamas;
using Fitamas.Entities;
using Fitamas.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDL.Gameplay.DigitalLogic
{
    public class LogicSystem : EntityUpdateSystem
    {
        private LogicComponentDescription currentComponent;
        private Simulator simulator;
        private LogicComponentManager manager;

        public static LogicSystem Instance { get; private set; }

        public LogicComponentDescription CurrentComponent => currentComponent;
        public Simulator Simulator => simulator;
        public LogicComponentManager Manager => manager;

        public LogicSystem() : base(Aspect.All())
        {
            Instance = this;
            manager = new LogicComponentManager();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            CreateSimulator();
            manager.LoadAll();
        }

        public override void Update(GameTime gameTime)
        {
            simulator.Update();
        }

        public void CreateSimulator()
        {
            LogicComponentDescription description = new LogicComponentDescription(typeof(LogicCircuit));
            description.Name = Guid.NewGuid().ToString();
            CreateSimulator(description);
        }

        public void CreateSimulator(LogicComponentDescription description,
            MonoAction<ComponentEventArgs> createComponent = null,
            MonoAction<ConnectionEventArgs> createConnection = null)
        {
            currentComponent = description;
            simulator = new Simulator();

            if (description == null)
            {
                Debug.LogError("LogicSystem: description is null");
                return;
            }

            Dictionary<int, LogicComponent> components = new Dictionary<int, LogicComponent>();

            foreach (var component in description.Components)
            {
                if (!component.NotNull)
                {
                    continue;
                }

                LogicComponentDescription componentDescription = component.Description;
                LogicComponent logicComponent = componentDescription.CreateComponent();
                simulator.AddComponent(logicComponent);
                components[component.Id] = logicComponent;
                ComponentEventArgs args = new ComponentEventArgs()
                {
                    Component = component,
                    LogicComponent = logicComponent
                };
                createComponent?.Invoke(args);
            }

            foreach (var connection in description.Connections)
            {
                if (!(components.ContainsKey(connection.OutputComponentId) && components.ContainsKey(connection.InputComponentId)))
                {
                    continue;
                }

                LogicComponent outputComponent = components[connection.OutputComponentId];
                LogicComponent inputComponent = components[connection.InputComponentId];

                ConnectorOutput outputConnector = outputComponent.GetOutput(connection.OutputIndex);
                ConnectorInput inputConnector = inputComponent.GetInput(connection.InputIndex);

                simulator.CreateConnect(outputConnector, inputConnector);

                ConnectionEventArgs args = new ConnectionEventArgs()
                {
                    Connection = connection,
                    OutputComponent = outputComponent,
                    InputComponent = inputComponent,
                    Output = outputConnector,
                    Input = inputConnector,
                };
                createConnection?.Invoke(args);
            }
        }

        public LogicComponentDescription CreateDescription(
            MonoAction<ComponentEventArgs> createComponent = null,
            MonoAction<ConnectionEventArgs> createConnection = null)
        {
            if (currentComponent == null)
            {
                Debug.LogError("LogicSystem: description is null");
                return null;
            }

            LogicComponentDescription description = currentComponent;
            Dictionary<LogicComponent, int> components = new Dictionary<LogicComponent, int>();
            Random random = new Random();

            description.PinInputName.Clear();
            description.PinOutputName.Clear();
            description.Components.Clear();
            description.Connections.Clear();

            foreach (var logicComponent in simulator.Components)
            {
                int id = random.Next(100000, 999999);
                components[logicComponent] = id;
                Component component = new Component()
                {
                    Id = id,
                    Description = logicComponent.Description,
                };

                ComponentEventArgs args = new ComponentEventArgs()
                {
                    Component = component,
                    LogicComponent = logicComponent,
                };
                createComponent?.Invoke(args);

                description.Components.Add(args.Component);
            }

            foreach (var outputComponent in components.Keys)
            {
                for (int i = 0; i < outputComponent.OutputCount; i++)
                {
                    ConnectorOutput outputConnector = outputComponent.GetOutput(i);

                    foreach (var inputConnector in outputConnector.Connectors)
                    {
                        LogicComponent inputComponent = simulator.GetComponent(inputConnector);

                        if (inputComponent == null)
                        {
                            Debug.LogError("Cannto create connection: " + outputConnector.Id + " and " + inputConnector.Id);
                            continue;
                        }

                        Connection connection = new Connection();
                        connection.OutputComponentId = components[outputComponent];
                        connection.OutputIndex = outputComponent.GetOutputIndex(outputConnector);
                        connection.InputComponentId = components[inputComponent];
                        connection.InputIndex = inputComponent.GetInputIndex(inputConnector);

                        ConnectionEventArgs args = new ConnectionEventArgs()
                        {
                            Connection = connection,
                            OutputComponent = outputComponent,
                            InputComponent = inputComponent,
                            Output = outputConnector,
                            Input = inputConnector,
                        };
                        createConnection?.Invoke(args);

                        description.Connections.Add(args.Connection);
                    }
                }
            }

            return description;
        }
    }

    public class ComponentEventArgs
    {
        public Component Component { get; set; }
        public LogicComponent LogicComponent { get; set; }
    }

    public class ConnectionEventArgs
    {
        public Connection Connection { get; set; }
        public LogicComponent OutputComponent { get; set; }
        public LogicComponent InputComponent { get; set; }
        public ConnectorOutput Output { get; set; }
        public ConnectorInput Input { get; set; }
    }
}
