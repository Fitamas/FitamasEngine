using Fitamas.Input;
using Fitamas.Input.Actions;
using Fitamas.Input.Bindings;
using Fitamas.Input.Processors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace WDL.Gameplay.Settings
{
    public class ActionMap
    {
        public InputActionMap InputActionMap { get; }
        public InputAction SaveProject { get; }
        public InputAction Delete { get; }

        public ActionMap()
        {
            InputActionMap = new InputActionMap();

            InputActionMap.AddAction(SaveProject = new InputAction(nameof(SaveProject))
                                                    .AddBinding(new InputCompositeBinding()
                                                    .Bind("1", "Keyboard\\S")
                                                    .Bind("2", "Keyboard\\LeftControl")));

            InputActionMap.AddAction(Delete = new InputAction(nameof(SaveProject))
                                                    .AddBinding(new InputBinding<bool>("Keyboard\\Back"))
                                                    .AddBinding(new InputBinding<bool>("Keyboard\\Delete")));
        }
    }
}
