using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WDL.DigitalLogic
{
    public class LogicComponentDescription
    {
        public string TypeId;
        public string Namespace;
        public int ThemeId;

        public List<string> PinInputName = new List<string>();
        public List<string> PinOutputName = new List<string>();

        public List<LogicComponentData> Components = new List<LogicComponentData>();
        public List<LogicConnectionData> Connections = new List<LogicConnectionData>();

        public Func<LogicComponentManager, LogicComponentDescription, LogicComponentData, LogicComponent> CreateFunc { get; set; }

        public int ComponentsCount => Components.Count;
        public int ConnectionsCount => Connections.Count;

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(Namespace))
                {
                    return Path.Combine(Namespace, TypeId);
                }
                
                return TypeId;
            }
        }

        public LogicComponentDescription()
        {
            CreateFunc = (manager, description, data) =>
            {
                return new LogicCircuit(manager, description, data);
            };
        }

        public LogicComponentDescription(Func<LogicComponentManager, LogicComponentDescription, LogicComponentData, LogicComponent> createFunc)
        {
            CreateFunc = createFunc;
        }

        public LogicComponent CreateComponent(LogicComponentManager manager, LogicComponentData data)
        {
            return CreateFunc?.Invoke(manager, this, data);
        }

        //public LogicComponentData GetFromIndex(int index)
        //{
        //    return Components[index];
        //}

        //public LogicComponentData GetFromId(int id)
        //{
        //    foreach (var component in Components)
        //    {
        //        if (component.Id == id)
        //        {
        //            return component;
        //        }
        //    }

        //    return default;
        //}

        //public void RemoveFromIndex(int index)
        //{
        //    Components.RemoveAt(index);
        //}

        //public void RemoveFromId(int id)
        //{
        //    for (int i = 0; i < Components.Count; i++)
        //    {
        //        if (Components[i].Id == id)
        //        {
        //            Components.RemoveAt(i);
        //        }
        //    }
        //}

        public void Remove(LogicComponentDescription description)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].TypeId == description.TypeId)
                {
                    Components.RemoveAt(i);
                }
            }
        }

        //public void Add(LogicComponentData component)
        //{
        //    if (component.NotNull)
        //    {
        //        Components.Add(component);
        //    }
        //}

        //public bool Contains(LogicComponentDescription description)
        //{
        //    if (description != null)
        //    {
        //        foreach (var component in Components)
        //        {
        //            if (component.NotNull)
        //            {
        //                if (component.Description == description || component.Description.Contains(description))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public LogicComponent CreateComponent()
        //{
        //    if (ComponentType != null)
        //    {
        //        LogicComponent result;

        //        if (!IsBase)
        //        {
        //            Dictionary<int, LogicComponent> components = new Dictionary<int, LogicComponent>();

        //            foreach (var component in Components)
        //            {
        //                LogicComponent logicComponent = component.Description.CreateComponent();
        //                components[component.Id] = logicComponent;

        //                for (int j = 0; j < logicComponent.InputCount; j++)
        //                {
        //                    var connector = new ConnectorInput(j);
        //                    logicComponent.SetInput(connector, j);
        //                }

        //                for (int j = 0; j < logicComponent.OutputCount; j++)
        //                {
        //                    var connector = new ConnectorOutput(j);
        //                    logicComponent.SetOutput(connector, j);
        //                }
        //            }

        //            foreach (var connection in Connections)
        //            {
        //                if (!(components.ContainsKey(connection.OutputComponentId) && components.ContainsKey(connection.InputComponentId)))
        //                {
        //                    continue;
        //                }

        //                LogicComponent inputComponent = components[connection.InputComponentId];
        //                LogicComponent outputComponent = components[connection.OutputComponentId];

        //                ConnectorInput input = inputComponent.GetInput(connection.InputIndex);
        //                ConnectorOutput output = outputComponent.GetOutput(connection.OutputIndex);

        //                output.Connectors.Add(input);
        //            }

        //            //result = new LogicCircuit(components.Values.ToArray());
        //        }
        //        else
        //        {
        //            //result = (LogicComponent)Activator.CreateInstance(ComponentType);
        //        }

        //        //result.Description = this;
        //        //return result;
        //    }

        //    return null;
        //}

        private static bool CompareBaseObject(LogicComponentDescription a, LogicComponentDescription b)
        {
            bool flag1 = (object)a == null;
            bool flag2 = (object)b == null;

            if (flag1 && flag2)
            {
                return true;
            }
            else if (flag1 && !flag2)
            {
                return false;
            }
            else if (!flag1 && flag2)
            {
                return false;
            }

            return a.FullName == b.FullName;
        }

        public static bool operator ==(LogicComponentDescription a, LogicComponentDescription b)
        {
            return CompareBaseObject(a, b);
        }

        public static bool operator !=(LogicComponentDescription a, LogicComponentDescription b)
        {
            return !CompareBaseObject(a, b);
        }

        public override bool Equals(object other)
        {
            if (other != null && other is LogicComponentDescription obj)
            {
                return CompareBaseObject(this, obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}
