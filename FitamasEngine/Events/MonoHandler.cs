using System;

namespace Fitamas.Events
{
    public class MonoHandler<T> : MonoEventBase
    {
        public virtual void Invoke(object sender, T args)
        {
            InvokeEvent(sender, args);
        }

        public void AddListener(MonoAction<object, T> action)
        {
            AddDelegate(action);
        }

        public void RemoveListener(MonoAction<object, T> action)
        {
            RemoveDelegate(action);
        }
    }
}
