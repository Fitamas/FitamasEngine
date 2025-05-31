using Fitamas.Input.Bindings;
using Fitamas.Input.InputListeners;
using Fitamas.Input.Interactions;
using Fitamas.Input.Processors;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Fitamas.Input.Actions
{
    public class InputAction
    {
        [SerializeField] private string name;
        [SerializeField] private List<InputAbstractBinding> bindings;

        private InputListener listener;
        private InputAbstractBinding binding;
        private InputActionState state;
        private InputActionState postState;

        internal InputActionMap ActionMap;

        public IInputInteraction Interaction;
        public InputProcessor Processor;

        public event Action<CallbackContext> Started;
        public event Action<CallbackContext> Performed;
        public event Action<CallbackContext> Canceled;

        public string Name => name;

        public InputAction(string name = nameof(InputAction))
        {
            this.name = name;
            bindings = new List<InputAbstractBinding>();
        }

        public void AddBinding(InputAbstractBinding binding)
        {
            if (!bindings.Contains(binding))
            {
                bindings.Add(binding);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (ActionMap.InputManager == null)
            {
                return;
            }

            if (!state.IsActive && !postState.IsActive)
            {
                foreach (var binding in bindings)
                {
                    bool flag = false;

                    foreach (var listener in ActionMap.InputManager.Listeners)
                    {
                        if (binding.Process(gameTime, listener))
                        {
                            this.listener = listener;
                            this.binding = binding;
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        break;
                    }
                }
            }
            else
            {
                binding.Process(gameTime, listener);
            }

            if (listener != null)
            {
                binding.PostProcess(gameTime, ref state);

                if (Interaction != null)
                {
                    Interaction.Process(gameTime, state, ref postState);
                    if (!postState.IsActive)
                    {
                        Interaction.Reset();
                    }
                    postState.Flush();
                }
                else
                {
                    postState = state;
                }

                Invoke(new CallbackContext(this, postState));

                state.Flush();
            }
        }

        public T GetValue<T>()
        {
            if (TryGetObjectValue(out object value))
            {
                return (T)value;
            }

            return default;
        }

        public bool TryGetObjectValue(out object objectValue)
        {
            if (binding == null)
            {
                objectValue = default;
                return false;
            }

            if (Processor != null)
            {
                objectValue = Processor.ProcessObject(binding.GetValue());
            }
            else
            {
                objectValue = binding.GetValue();
            }

            return true;
        }

        private void Invoke(CallbackContext callbackContext)
        {
            if (callbackContext.Type.HasFlag(InputActionType.Started))
            {
                Started?.Invoke(callbackContext);
            }
            if (callbackContext.Type.HasFlag(InputActionType.Performed))
            {
                Performed?.Invoke(callbackContext);
            }
            if (callbackContext.Type.HasFlag(InputActionType.Canceled))
            {
                Canceled?.Invoke(callbackContext);
            }
        }
    }

    public struct CallbackContext
    {
        public InputAction Action { get; }
        public InputActionState State { get; }
        public InputActionType Type => State.Type;

        public CallbackContext(InputAction action, InputActionState state)
        {
            Action = action;
            State = state;
        }

        public T GetValue<T>()
        {
            return Action.GetValue<T>();
        }

        public bool TryGetObjectValue(out object objectValue)
        {
            return Action.TryGetObjectValue(out objectValue);
        }
    }
}
