using Fitamas.Input;
using Fitamas.Input.Actions;
using Fitamas.Input.Bindings;
using Fitamas.Input.Processors;
using Microsoft.Xna.Framework;
using System;

namespace Physics.Settings
{
    public class ActionMap
    {
        public InputActionMap InputActionMap { get; }
        public InputAction MoveCamera { get; }
        public InputAction UseToolPosition { get; }
        public InputAction UseTool { get; }

        public ActionMap()
        {
            InputActionMap = new InputActionMap();

            MoveCamera = new InputAction();
            MoveCamera.AddBinding(new InputVector2DBinding()
                .Bind(InputVector2DBinding.PositiveX, "Keyboard\\D")
                .Bind(InputVector2DBinding.NegativeX, "Keyboard\\A")
                .Bind(InputVector2DBinding.PositiveY, "Keyboard\\W")
                .Bind(InputVector2DBinding.NegativeY, "Keyboard\\S"));
            MoveCamera.Processor = new NormalizeVector2Processor();

            UseToolPosition = new InputAction(nameof(UseToolPosition));
            UseToolPosition.AddBinding(new InputBinding<Point>("Mouse\\Position"));

            UseTool = new InputAction(nameof(UseTool));
            UseTool.AddBinding(new InputBinding<bool>("Mouse\\Left"));

            InputActionMap.AddAction(MoveCamera)
                          .AddAction(UseToolPosition)
                          .AddAction(UseTool);
        }
    }
}
