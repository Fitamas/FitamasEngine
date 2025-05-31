using Fitamas.Input.Actions;
using Fitamas.Input.InputListeners;
using Fitamas.Input.Interactions;
using Fitamas.Input.Processors;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Input.Bindings
{
    public abstract class InputAbstractBinding
    {
        public IInputInteraction Interaction;
        public InputProcessor Processor;

        protected InputActionState state;

        public InputAbstractBinding()
        {

        }

        public abstract bool Process(GameTime gameTime, InputListener listener);

        public void PostProcess(GameTime gameTime, ref InputActionState state)
        {
            if (Interaction != null)
            {
                Interaction.Process(gameTime, this.state, ref state);

                if (!state.IsActive)
                {
                    Interaction.Reset();
                }
            }
            else
            {
                state = this.state;
            }

            this.state.Flush();
        }

        public abstract object GetValue();
    }

    public abstract class InputAbstractBinding<T> : InputAbstractBinding
    {
        protected T value;

        public T Value => value;

        protected InputAbstractBinding()
        {

        }

        public override bool Process(GameTime gameTime, InputListener listener)
        {
            if (!CanProcess(listener))
            {
                return false;
            }

            T value = ReadValue(listener);

            if (!Equals(value, default(T)))
            {
                if (!state.IsActive || !Equals(value, Value))
                {
                    state.Performed(gameTime);
                }
            }
            else
            {
                if (state.IsActive)
                {
                    state.Performed(gameTime);
                    state.Canceled();
                }
            }

            this.value = value;

            return state.InProgress;
        }

        internal abstract bool CanProcess(InputListener listener);

        protected abstract T ReadValue(InputListener listener);

        public override object GetValue()
        {
            if (Processor != null)
            {
                return Processor.ProcessObject(value);
            }

            return value;
        }
    }
}
