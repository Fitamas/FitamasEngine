using Microsoft.Xna.Framework;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDL.DigitalLogic
{
    public abstract class LogicComponent
    {
        protected LogicConnectorInput[] input;
        protected LogicConnectorOutput[] output;

        public LogicComponentDescription Description { get; }
        public LogicComponentData Data { get; }
        public ReactiveProperty<Point> Position { get; }

        public int Id => Data.Id;
        public int InputCount => input.Length;
        public int OutputCount => output.Length;

        public LogicComponent(LogicComponentDescription description, LogicComponentData data, int inputCount = 0, int outputCount = 0)
        {
            Description = description;
            Data = data;
            Position = new ReactiveProperty<Point>();
            Position.Subscribe(position => Data.Position = position);

            int index = 0;
            input = new LogicConnectorInput[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                input[i] = new LogicConnectorInput(index, this);
                index++;
            }
            output = new LogicConnectorOutput[outputCount];
            for (int i = 0;i < outputCount; i++)
            {
                output[i] = new LogicConnectorOutput(index, this);
                index++;
            }
        }

        public LogicConnectorInput GetInputFromIndex(int index)
        {
            return input[index];
        }

        public LogicConnectorInput GetInputFromId(int id)
        {
            foreach (var input in input)
            {
                if (input.Id == id)
                {
                    return input;
                }
            }

            return null;
        }

        public int GetInputIndex(LogicConnectorInput connector)
        {
            return Array.FindIndex(input, input => input == connector);
        }

        public LogicConnectorOutput GetOutputFromIndex(int index)
        {
            return output[index];
        }

        public LogicConnectorOutput GetOutputFromId(int id)
        {
            foreach (var output in output)
            {
                if (output.Id == id)
                {
                    return output;
                }
            }

            return null;
        }

        public int GetOutputIndex(LogicConnectorOutput connector)
        {
            return Array.FindIndex(output, output => output == connector);
        }

        public bool Contain(LogicConnector connector)
        {
            return input.Contains(connector) || output.Contains(connector);
        }

        public abstract void Update();
    }

    public class LogicAnd : LogicComponent
    {
        public LogicAnd(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 2, 1)
        { 

        }

        public override void Update()
        {
            bool result = input[0].Signal.IsHigh && input[1].Signal.IsHigh;
            output[0].ReceiveInput(new LogicSignal(result));
        }
    }

    public class LogicNot : LogicComponent
    {
        public LogicNot(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 1, 1)
        {

        }

        public override void Update()
        {
            bool result = !input[0].Signal.IsHigh;
            output[0].ReceiveInput(new LogicSignal(result));
        }
    }

    public class LogicInput : LogicComponent
    {
        public ReactiveProperty<bool> Signal { get; }

        public LogicInput(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 0, 1)
        {
            Signal = new ReactiveProperty<bool>();
        }

        public override void Update()
        {
            output[0].ReceiveInput(new LogicSignal(Signal.Value));
        }
    }

    public class LogicOutput : LogicComponent
    {
        private ReactiveProperty<bool> signal;

        public ReadOnlyReactiveProperty<bool> Signal => signal;

        public LogicOutput(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 1, 0)
        {
            signal = new ReactiveProperty<bool>();
        }

        public override void Update()
        {
            signal.Value = input[0].Signal.IsHigh;
        }
    }

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
