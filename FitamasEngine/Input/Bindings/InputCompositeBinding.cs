using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.Input.Bindings
{
    public abstract class InputCompositeBinding<T> : InputAbstractBinding<T>
    {
        protected Dictionary<string, InputBinding<bool>> bindingMap;

        public InputCompositeBinding()
        {
            bindingMap = new Dictionary<string, InputBinding<bool>>();
        }

        internal override bool CanProcess(InputListener listener)
        {
            bool flag = false;
            foreach (var binding in bindingMap.Values)
            {
                if (binding.CanProcess(listener))
                {
                    flag = true;
                }
            }

            return flag;
        }

        public override bool Process(GameTime gameTime, InputListener listener)
        {
            foreach (var binding in bindingMap.Values)
            {
                binding.Process(gameTime, listener);
            }

            return base.Process(gameTime, listener);
        }

        public InputCompositeBinding<T> Bind(string name, string path)
        {
            bindingMap[name] = new InputBinding<bool>(path);
            return this;
        }
    }
}
