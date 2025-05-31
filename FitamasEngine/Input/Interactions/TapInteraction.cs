using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Interactions
{
    public class TapInteraction : IInputInteraction
    {
        public void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState)
        {
            if (inputState.IsStarted)
            {
                outputState.Started(gameTime);
            }
            else if (inputState.IsCanceled)
            {
                outputState.Performed(gameTime);
                outputState.Canceled();
            }
        }

        public void Reset()
        {

        }
    }
}
