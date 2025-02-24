using Fitamas.Container;
using Fitamas.Serialization;
using Fitamas.UserInterface.Scripting;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Serializeble
{
    public class SerializebleLayout : MonoObject
    {
        public GUIScripting Scripting { get; }

        public List<GUIComponent> Components { get; }

        public SerializebleLayout(GUIScripting scripting, List<GUIComponent> components)
        {
            Scripting = scripting;
            Components = components;
        }
    }
}
