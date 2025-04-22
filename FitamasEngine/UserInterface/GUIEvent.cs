using Fitamas.Events;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public abstract class GUIEventBase
    {
        private List<Delegate> delegates = new List<Delegate>();

        internal void AddDelegate(Delegate del)
        {
            if (del != null)
            {
                delegates.Add(del);
            }
        }

        internal void RemoveDelegate(Delegate handler)
        {
            List<Delegate> removeCalls = new List<Delegate>();
            foreach (var item in delegates)
            {
                if (item == handler)
                {
                    removeCalls.Add(item);
                }
            }

            foreach (var call in removeCalls)
            {
                delegates.Remove(call);
            }
        }

        internal void InvokeEvent(params object[] args)
        {
            foreach (var call in delegates.ToArray())
            {
                call.DynamicInvoke(args);
            }
        }

        public void Clear()
        {
            delegates.Clear();
        }
    }

    public class GUIEvent : GUIEventBase
    {
        public GUIEvent(params MonoAction[] actions)
        {
            foreach (var action in actions)
            {
                AddListener(action);
            }
        }

        public void Invoke()
        {
            InvokeEvent();      
        }

        public void AddListener(MonoAction action)
        {
            AddDelegate(action);
        }

        public void RemoveListener(MonoAction action)
        {
            RemoveDelegate(action);
        }
    }

    public class GUIEvent<T0> : GUIEventBase
    {
        public GUIEvent(params MonoAction<T0>[] actions)
        {
            foreach (var action in actions)
            {
                AddListener(action);
            }
        }

        public void Invoke(T0 arg0)
        {
            InvokeEvent(arg0);
        }

        public void AddListener(MonoAction<T0> action)
        {
            AddDelegate(action);
        }

        public void RemoveListener(MonoAction<T0> action)
        {
            RemoveDelegate(action);
        }
    }

    public class GUIEvent<T0, T1> : GUIEventBase
    {
        public GUIEvent(params MonoAction<T0, T1>[] actions)
        {
            foreach (var action in actions)
            {
                AddListener(action);
            }
        }

        public void Invoke(T0 arg0, T1 arg1)
        {
            InvokeEvent(arg0, arg1);
        }

        public void AddListener(MonoAction<T0, T1> action)
        {
            AddDelegate(action);
        }

        public void RemoveListener(MonoAction<T0, T1> action)
        {
            RemoveDelegate(action);
        }
    }
}
