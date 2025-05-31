using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Interactions
{
    public class PressInteraction : IInputInteraction
    {
        public float PressTime;

        private bool waitingForRelease;

        public PressInteraction(float pressTime = 1)
        {
            PressTime = pressTime;
        }

        public void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState)
        {
            if (inputState.IsStarted)
            {
                outputState.Started(gameTime);
            }
            else if ((gameTime.TotalGameTime - inputState.StartTime).TotalSeconds > PressTime && !waitingForRelease && inputState.IsActive)
            {
                outputState.Performed(gameTime);
                waitingForRelease = true;
            }
            else if (inputState.IsCanceled)
            {
                outputState.Canceled();
                waitingForRelease = false;
            }
        }

        public void Reset()
        {
            waitingForRelease = false;
        }
    }
}
