using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Interactions
{
    public class SlowTapInteraction : IInputInteraction
    {
        public float TapTime;

        public SlowTapInteraction(float tapTime = 1)
        {
            TapTime = tapTime;
        }

        public void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState)
        {
            if (inputState.IsStarted)
            {
                outputState.Started(gameTime);
            }
            else if (inputState.IsCanceled)
            {
                if ((gameTime.TotalGameTime - inputState.StartTime).TotalSeconds > TapTime)
                {
                    outputState.Performed(gameTime);
                }

                outputState.Canceled();
            }
        }

        public void Reset()
        {

        }
    }
}
