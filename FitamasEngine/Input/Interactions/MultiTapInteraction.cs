using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Interactions
{
    public class MultiTapInteraction : IInputInteraction
    {
        public int Count;
        public float TapTime;

        private int tapCount;

        public MultiTapInteraction(int count = 2, float tapTime = 0.1f)
        {
            Count = count;
            TapTime = tapTime;
        }

        public void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState)
        {
            if (Count <= 0)
            {
                return;
            }

            if(outputState.IsActive && (gameTime.TotalGameTime - inputState.StartTime).TotalSeconds > TapTime)
            {
                outputState.Canceled();
            }
            else if (inputState.IsStarted)
            {
                outputState.Started(gameTime);
                tapCount++;

                if (tapCount == Count)
                {
                    outputState.Performed(gameTime);
                    outputState.Canceled();
                }
            }
        }

        public void Reset()
        {
            tapCount = 0;
        }
    }
}
