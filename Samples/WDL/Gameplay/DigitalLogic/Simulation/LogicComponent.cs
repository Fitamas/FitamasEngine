using System;
using System.Linq;

namespace WDL.Gameplay.DigitalLogic
{
    public abstract class LogicComponent
    {
        protected ConnectorInput[] input;
        protected ConnectorOutput[] output;

        public int InputCount => input.Length;
        public int OutputCount => output.Length;

        public LogicComponentDescription Description { get; set; }
        public int Id { get; set; }

        public LogicComponent(int inputCount = 0, int outputCount = 0)
        {
            this.input = new ConnectorInput[inputCount];
            this.output = new ConnectorOutput[outputCount];
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

    public class LogicAND : LogicComponent
    {
        public LogicAND() : base(2, 1)
        {

        }

        public override void Update()
        {
            bool result = input[0].Signal.IsHigh && input[1].Signal.IsHigh;
            output[0].ReceiveInput(new Signal(result));
        }
    }

    public class LogicNOT : LogicComponent
    {
        public LogicNOT() : base(1, 1)
        {

        }

        public override void Update()
        {
            bool result = !input[0].Signal.IsHigh;
            output[0].ReceiveInput(new Signal(result));
        }
    }

    public class LogicINPUT : LogicComponent
    {
        private bool value;

        public bool Value => value;

        public LogicINPUT() : base(0, 1)
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

    public class LogicOUTPUT : LogicComponent, IOutputLogicComponent
    {
        private bool value;

        public bool Value => value;

        public ConnectorOutput[] Output => output;

        public LogicOUTPUT() : base(1, 0)
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

        private LogicINPUT[] pinInput;
        private LogicOUTPUT[] pinOutput;

        public LogicCircuit(LogicComponent[] components) : base(
            components.Count(s => s is LogicINPUT), 
            components.Count(s => s is LogicOUTPUT))
        {
            this.components = components;
            pinInput = new LogicINPUT[InputCount];
            pinOutput = new LogicOUTPUT[OutputCount];

            int inputCount = 0;
            int outputCount = 0;

            foreach (var component in components)
            {
                if (component is LogicINPUT input)
                {
                    pinInput[inputCount] = input;
                    inputCount++;
                }
                if (component is LogicOUTPUT output)
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
