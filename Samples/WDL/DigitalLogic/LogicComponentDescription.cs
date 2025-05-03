using Fitamas;
using R3;
using System;
using System.Collections.Generic;
using System.IO;
using WDL.DigitalLogic.Components;

namespace WDL.DigitalLogic
{
    public class LogicComponentDescription
    {
        public string TypeId;
        public string Namespace;
        public int ThemeId;
        public bool IsBase;
        public int ComponentCount;
        public int ConnectionCount;

        public List<LogicComponentData> Components = new List<LogicComponentData>();
        public List<LogicConnectionData> Connections = new List<LogicConnectionData>();

        public Dictionary<int, string> Connectors = new Dictionary<int, string>();

        public Func<LogicComponentManager, LogicComponentDescription, LogicComponentData, LogicComponent> CreateFunc { get; set; }

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

            TypeId = Guid.NewGuid().ToString();
        }

        public LogicComponentDescription(Func<LogicComponentManager, LogicComponentDescription, LogicComponentData, LogicComponent> createFunc)
        {
            CreateFunc = createFunc;
        }

        public LogicComponent CreateComponent(LogicComponentManager manager, LogicComponentData data)
        {
            return CreateFunc?.Invoke(manager, this, data);
        }

        public void RemoveComponents(LogicComponentDescription description)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i].TypeId == description.TypeId)
                {
                    Components.RemoveAt(i);
                }
            }
        }

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
