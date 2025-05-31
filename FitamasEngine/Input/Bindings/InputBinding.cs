using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.Input.Bindings
{
    public class InputBinding<T> : InputAbstractBinding<T>
    {
        public string Path;

        public InputBinding(string path = "")
        {
            Path = path;
        }

        internal override bool CanProcess(InputListener listener)
        {
            foreach (var ptr in listener.InputEvents)
            {
                if (ptr.Path == Path)
                {
                    return true;
                }
            }

            return false;
        }

        protected override T ReadValue(InputListener listener)
        {
            foreach (var ptr in listener.InputEvents)
            {
                if (ptr.Path == Path)
                {
                    return (T)ptr.Value;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
