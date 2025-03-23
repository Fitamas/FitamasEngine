using System;
using System.Collections.Generic;
using System.Linq;

namespace WDL.DigitalLogic
{
    public abstract class LogicComponent
    {
        protected ConnectorInput[] input;
        protected ConnectorOutput[] output;

        public int InputCount => input.Length;
        public int OutputCount => output.Length;

        public LogicComponentDescription Description { get;}
        public LogicComponentData Data { get; }
        public int Id => Data.Id;

        public LogicComponent(LogicComponentDescription description, LogicComponentData data, int inputCount = 0, int outputCount = 0)
        {
            Description = description;
            Data = data;
            input = new ConnectorInput[inputCount];
            output = new ConnectorOutput[outputCount];
        }

        public void SetInput(ConnectorInput connector, int index)
        {
            input[index] = connector;
        }

        public ConnectorInput GetInput(int index)
        {
            return input[index];
        }

        public int GetInputIndex(ConnectorInput connector)
        {
            return Array.FindIndex(input, input => input == connector);
        }

        public void SetOutput(ConnectorOutput connector, int index)
        {
            output[index] = connector;
        }

        public ConnectorOutput GetOutput(int index)
        {
            return output[index];
        }

        public int GetOutputIndex(ConnectorOutput connector)
        {
            return Array.FindIndex(output, output => output == connector);
        }

        public bool Contain(Connector connector)
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
            output[0].ReceiveInput(new Signal(result));
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
            output[0].ReceiveInput(new Signal(result));
        }
    }

    public class LogicInput : LogicComponent
    {
        private bool value;

        public bool Value => value;

        public LogicInput(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 0, 1)
        {

        }

        public override void Update()
        {
            output[0].ReceiveInput(new Signal(value));
        }

        public void Press()
        {
            Press(!value);
        }

        public void Press(bool value)
        {
            this.value = value;
        }
    }

    public interface IOutputLogicComponent
    {
        bool Value { get; }
        ConnectorOutput[] Output { get; }
    }

    public class LogicOutput : LogicComponent, IOutputLogicComponent
    {
        private bool value;

        public bool Value => value;

        public ConnectorOutput[] Output => output;

        public LogicOutput(LogicComponentDescription description, LogicComponentData data) 
            : base(description, data, 1, 0)
        {

        }

        public override void Update()
        {
            value = input[0].Signal.IsHigh;
        }
    }

    public class LogicCircuit : LogicComponent
    {
        private LogicComponent[] components;
        private LogicInput[] pinInput;
        private LogicOutput[] pinOutput;

        public LogicCircuit(LogicComponentManager manager, LogicComponentDescription description, LogicComponentData data) 
            : base(description, data,
                   description.Components.Count(s => s.TypeId == "Input"),
                   description.Components.Count(s => s.TypeId == "Output"))
        {
            components = new LogicComponent[description.Components.Count];
            pinInput = new LogicInput[InputCount];
            pinOutput = new LogicOutput[OutputCount];

            Dictionary<int, LogicComponent> componentsMap = new Dictionary<int, LogicComponent>();

            foreach (var component in description.Components)
            {
                LogicComponentDescription description1 = manager.GetComponent(component.TypeId);
                LogicComponent logicComponent = description1.CreateComponent(manager, component);
                componentsMap[component.Id] = logicComponent;

                for (int j = 0; j < logicComponent.InputCount; j++)
                {
                    var connector = new ConnectorInput(j);
                    logicComponent.SetInput(connector, j);
                }

                for (int j = 0; j < logicComponent.OutputCount; j++)
                {
                    var connector = new ConnectorOutput(j);
                    logicComponent.SetOutput(connector, j);
                }
            }

            foreach (var connection in description.Connections)
            {
                if (!(componentsMap.ContainsKey(connection.OutputComponentId) && componentsMap.ContainsKey(connection.InputComponentId)))
                {
                    continue;
                }

                LogicComponent inputComponent = componentsMap[connection.InputComponentId];
                LogicComponent outputComponent = componentsMap[connection.OutputComponentId];

                ConnectorInput input = inputComponent.GetInput(connection.InputIndex);
                ConnectorOutput output = outputComponent.GetOutput(connection.OutputIndex);

                output.Connectors.Add(input);
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
                Signal signal = input[i].Signal;
                pinInput[i].Press(signal.IsHigh);
            }

            foreach (var component in components)
            {
                component.Update();
            }

            for (var i = 0;i < OutputCount; i++)
            {
                Signal signal = new Signal(pinOutput[i].Value);
                output[i].ReceiveInput(signal);
            }
        }
    }
}
