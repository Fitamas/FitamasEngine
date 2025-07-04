﻿using Fitamas.ECS;
using Fitamas.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Scene
{
    public class EntityData : MonoObject
    {
        public string Name;
        public List<Component> Components = new List<Component>();

        public EntityData(ICollection<Component> components = null, string name = "Entity")
        {
            if (components != null)
            {
                Components = components.ToList();
            }
            
            Name = name;
        }

        public bool Contain(Component component)
        {
            return Components.Contains(component);
        }

        public bool Contains(Type type)
        {
            foreach (var item in Components)
            {
                if (item.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();

            return component != null;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (var component in Components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }

            return null;
        }
    }
}
