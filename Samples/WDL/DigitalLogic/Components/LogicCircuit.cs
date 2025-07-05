using System;
using System.Collections.Generic;

namespace WDL.DigitalLogic.Components
{
    public class LogicCircuit : LogicComponent
    {
        private LogicComponent[] components;
        private LogicInput[] pinInput;
        private LogicOutput[] pinOutput;

        public LogicCircuit(LogicComponentManager manager, LogicComponentDescription description, LogicComponentData data)
            : base(description, data, description.InputConnectors.Count, description.OutputConnectors.Count)
        {
            components = new LogicComponent[description.Components.Count];
            pinInput = new LogicInput[InputCount];
            pinOutput = new LogicOutput[OutputCount];

            Dictionary<int, LogicComponent> componentsMap = new Dictionary<int, LogicComponent>();
            for (int i = 0; i < description.Components.Count; i++)
            {
                LogicComponentData component = description.Components[i];
                LogicComponentDescription description1 = manager.GetComponent(component.TypeId);
                if (description1 == null)
                {
                    continue;
                }

                LogicComponent logicComponent = description1.CreateComponent(manager, component);
                componentsMap[component.Id] = logicComponent;
                components[i] = logicComponent;
            }

            foreach (var connectionData in description.Connections)
            {
                if (!(componentsMap.ContainsKey(connectionData.OutputComponentId) && componentsMap.ContainsKey(connectionData.InputComponentId)))
                {
                    continue;
                }

                LogicComponent inputComponent = componentsMap[connectionData.InputComponentId];
                LogicComponent outputComponent = componentsMap[connectionData.OutputComponentId];

                LogicConnectorInput input = inputComponent.GetInputFromId(connectionData.InputId);
                LogicConnectorOutput output = outputComponent.GetOutputFromId(connectionData.OutputId);

                LogicConnection connection = new LogicConnection(connectionData, input, output);
                output.Connect(connection);
                input.Connect(connection);
            }

            for (int i = 0; i < InputCount;  i++)
            {
                int id = description.InputConnectors[i].ComponentID;
                if (componentsMap.ContainsKey(id) && componentsMap[id] is LogicInput input)
                {
                    pinInput[i] = input;
                }
            }

            for (int i = 0; i < OutputCount; i++)
            {
                int id = description.OutputConnectors[i].ComponentID;
                if (componentsMap.ContainsKey(id) && componentsMap[id] is LogicOutput output)
                {
                    pinOutput[i] = output;
                }
            }
        }

        public override void Update()
        {
            for (var i = 0; i < InputCount; i++)
            {
                LogicSignal signal = input[i].Signal;
                pinInput[i].Signal.Value = signal.IsHigh;
            }

            foreach (var component in components)
            {
                component.Update();
            }

            for (var i = 0; i < OutputCount; i++)
            {
                LogicSignal signal = new LogicSignal(pinOutput[i].Signal.CurrentValue);
                output[i].ReceiveInput(signal);
            }
        }
    }
}
