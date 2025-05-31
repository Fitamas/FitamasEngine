using Fitamas.Input;
using Fitamas.Input.Actions;
using Fitamas.Input.Bindings;
using Microsoft.Xna.Framework;
using System;

namespace Physics.Settings
{
    public class ActionMap
    {
        public InputActionMap InputActionMap { get; }
        public InputAction UseToolPosition { get; }
        public InputAction UseTool { get; }

        public ActionMap()
        {
            InputActionMap = new InputActionMap();

            UseToolPosition = new InputAction(nameof(UseToolPosition));
            UseToolPosition.AddBinding(new InputBinding<Point>("Mouse\\Position"));

            UseTool = new InputAction(nameof(UseTool));
            UseTool.AddBinding(new InputBinding<bool>("Mouse\\Left"));

            InputActionMap.AddAction(UseToolPosition)
                          .AddAction(UseTool);
        }
    }
}
