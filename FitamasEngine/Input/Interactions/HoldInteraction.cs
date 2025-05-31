using Fitamas.Input.Actions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Input.Interactions
{
    public class HoldInteraction : IInputInteraction
    {
        public float durationTime;
        public int HoldThreshold;

        public void Process(GameTime gameTime, InputActionState inputState, ref InputActionState outputState)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Reset()
        {
            throw new NotImplementedException(); //TODO
        }
    }
}
