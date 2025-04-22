using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public class GUIControlTemplate
    {
        private Dictionary<string, GUIComponent> componentsMap;

        public GUIControlTemplate()
        {
            componentsMap = new Dictionary<string, GUIComponent>();
        }


        public GUIComponent this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Add(value);
            }
        }

        public GUIComponent Get(string name)
        {
            if (componentsMap.TryGetValue(name, out GUIComponent component))
            {
                return component;
            }

            return null;
        }

        public void Add(GUIComponent component)
        {
            Add(component, component.Name);
        }

        public void Add(GUIComponent component, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                componentsMap[name] = component;
            }
        }

        public void Remove(GUIComponent component)
        {
            Remove(component.Name);
        }

        public void Remove(string name)
        {
            componentsMap.Remove(name);
        }
    }
}
