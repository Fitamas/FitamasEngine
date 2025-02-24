using Fitamas.Entities;
using Fitamas.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Scene
{
    public class Prefab : MonoObject
    {
        public string Name;
        public List<Component> Components;

        public Prefab(ICollection<Component> components, string name = "Entity")
        {
            Components = components.ToList();
            Name = name;
        }
    }
}
