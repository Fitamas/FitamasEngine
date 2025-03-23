using Fitamas;
using Fitamas.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WDL.DigitalLogic
{
    public class LogicSystem : IUpdateSystem
    {
        private Simulator simulator;

        public Simulator Simulator => simulator;
        public LogicComponentManager Manager { get; }

        public LogicSystem()
        {
            Manager = new LogicComponentManager();
        }

        public void Initialize(GameWorld world)
        {
            Manager.LoadAll();
        }

        public void Update(GameTime gameTime)
        {
            simulator?.Update();
        }

        public void CreateSimulator()
        {
            LogicComponentDescription description = new LogicComponentDescription();
            description.TypeId = Guid.NewGuid().ToString();
            CreateSimulator(description);
        }

        public void CreateSimulator(LogicComponentDescription description)
        {
            if (description == null)
            {
                Debug.LogError("LogicSystem: description is null");
                return;
            }

            simulator = new Simulator(Manager, description);
            Dictionary<int, LogicComponent> components = new Dictionary<int, LogicComponent>();

            //foreach (var component in description.Components)
            //{
            //    if (!component.NotNull)
            //    {
            //        continue;
            //    }

            //    LogicComponentDescription componentDescription = component.Description;
            //    LogicComponent logicComponent = componentDescription.CreateComponent();
            //    simulator.AddComponent(logicComponent);
            //    components[component.Id] = logicComponent;
            //}

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
            }
        }

        //public LogicComponentDescription CreateDescription()
        //{
        //    LogicComponentDescription description = new LogicComponentDescription();
        //    Dictionary<LogicComponent, int> components = new Dictionary<LogicComponent, int>();
        //    Random random = new Random();

        //    description.PinInputName.Clear();
        //    description.PinOutputName.Clear();
        //    description.Components.Clear();
        //    description.Connections.Clear();

            //foreach (var logicComponent in simulator.Components)
            //{
            //    int id = random.Next(100000, 999999);
            //    components[logicComponent] = id;
            //    Component component = new Component()
            //    {
            //        Id = id,
            //        Description = logicComponent.Description,
            //    };

            //    ComponentEventArgs args = new ComponentEventArgs()
            //    {
            //        Component = component,
            //        LogicComponent = logicComponent,
            //    };
            //    createComponent?.Invoke(args);

            //    description.Components.Add(args.Component);
            //}

        //    foreach (var outputComponent in components.Keys)
        //    {
        //        for (int i = 0; i < outputComponent.OutputCount; i++)
        //        {
        //            ConnectorOutput outputConnector = outputComponent.GetOutput(i);

        //            foreach (var inputConnector in outputConnector.Connectors)
        //            {
        //                LogicComponent inputComponent = simulator.GetComponent(inputConnector);

        //                if (inputComponent == null)
        //                {
        //                    Debug.LogError("Cannto create connection: " + outputConnector.Id + " and " + inputConnector.Id);
        //                    continue;
        //                }

        //                LogicConnectionData connection = new LogicConnectionData();
        //                connection.OutputComponentId = components[outputComponent];
        //                connection.OutputIndex = outputComponent.GetOutputIndex(outputConnector);
        //                connection.InputComponentId = components[inputComponent];
        //                connection.InputIndex = inputComponent.GetInputIndex(inputConnector);

        //                description.Connections.Add(connection);
        //            }
        //        }
        //    }

        //    return description;
        //}

        public void Dispose()
        {

        }
    }
}
