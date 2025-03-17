using ClipperLib;
using Fitamas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDL.Gameplay.DigitalLogic
{
    public class Simulator
    {
        private List<LogicComponent> components = new List<LogicComponent>();
        private List<Connector> connectors = new List<Connector>();
        private int componentCount = 0;
        private int connectCount = 0;

        public LogicComponent[] Components => components.ToArray();

        public IOutputLogicComponent[] OutputComponents
        {
            get
            {
                List<IOutputLogicComponent> result = new List<IOutputLogicComponent>();
                foreach (LogicComponent component in components)
                {
                    if (component is IOutputLogicComponent output)
                    {
                        result.Add(output);
                    }
                }

                return result.ToArray();
            }
        }

        public Simulator()
        {

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
            Debug.Log("Add to simulator: " + component.Description.FullName);

            if (!components.Contains(component))
            {
                component.Id = componentCount;
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
