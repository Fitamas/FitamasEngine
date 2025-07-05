using Microsoft.Xna.Framework;
using R3;
using System;
using System.Linq;

namespace WDL.DigitalLogic.Components
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
            Position = new ReactiveProperty<Point>(Data.Position);
            Position.Subscribe(position => Data.Position = position);

            input = new LogicConnectorInput[inputCount];
            for (int i = 0; i < inputCount; i++)
            {
                input[i] = new LogicConnectorInput(description.InputConnectors[i], this);
            }
            output = new LogicConnectorOutput[outputCount];
            for (int i = 0; i < outputCount; i++)
            {
                output[i] = new LogicConnectorOutput(description.OutputConnectors[i], this);
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
}
