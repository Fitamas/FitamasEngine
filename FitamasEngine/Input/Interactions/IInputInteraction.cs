using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Interactions
{
    public interface IInputInteraction
    {
        void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState);
        void Reset();
    }
}
