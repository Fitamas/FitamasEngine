using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDL.DigitalLogic.Components
{
    public class LogicCircuit : LogicComponent
    {
        private LogicComponent[] components;
        private LogicInput[] pinInput;
        private LogicOutput[] pinOutput;

        public LogicCircuit(LogicComponentManager manager, LogicComponentDescription description, LogicComponentData data)
            : base(description, data,
                   description.Components.Count(s => s.TypeId == LogicComponents.Input.FullName),
                   description.Components.Count(s => s.TypeId == LogicComponents.Output.FullName))
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

            int inputCount = 0;
            int outputCount = 0;
            foreach (var component in components)
            {
                if (component is LogicInput input)
                {
                    pinInput[inputCount] = input;
                    inputCount++;
                }
                if (component is LogicOutput output)
                {
                    pinOutput[outputCount] = output;
                    outputCount++;
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
