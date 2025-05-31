using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Fitamas.Input.Actions
{
    public class InputActionMap : IDisposable
    {
        [SerializeField] private string name;
        [SerializeField] private List<InputAction> actions;

        public bool Enable;

        internal InputManager InputManager { get; set; }

        public string Name => name;

        public InputActionMap(string name = nameof(InputActionMap))
        {
            this.name = name;
            actions = new List<InputAction>();
            Enable = true;
        }

        public void Update(GameTime gameTime)
        {
            if (Enable)
            {
                foreach (var action in actions)
                {
                    action.ActionMap = this;
                    action.Update(gameTime);
                }
            }
        }

        public InputActionMap AddAction(InputAction action)
        {
            if (!actions.Contains(action))
            {
                actions.Add(action);
            }
            return this;
        }

        public void Dispose()
        {

        }
    }
}