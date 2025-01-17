using Fitamas.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.UserInterface.Scripting
{
    public class InvokableCall
    {
        public object Target { get; }
        public MethodInfo Method { get; }

        public InvokableCall(object target, MethodInfo method)
        {
            Target = target;
            Method = method;
        }

        public bool Find(object target, MethodInfo method) //TODO fix
        {
            return (Target == target || Target.GetType() == target.GetType()) && Method == method;
        }
    }

    public abstract class GUIEventBase
    {
        private List<InvokableCall> calls = new List<InvokableCall>();

        protected void AddListener(object target, MethodInfo method)
        {
            InvokableCall call = new InvokableCall(target, method);
            calls.Add(call);
        }

        protected void RemoveListener(object target, MethodInfo method)
        {
            List<InvokableCall> removeCalls = new List<InvokableCall>();
            foreach (InvokableCall call in calls)
            {
                if (call.Find(target, method))
                {
                    removeCalls.Add(call);
                }
            }

            foreach (InvokableCall call in removeCalls)
            {
                calls.Remove(call);
            }
        }

        protected void InvokeEvent(params object[] args)
        {
            foreach (InvokableCall call in calls)
            {
                call.Method.Invoke(call.Target, args);
            }
        }

        public void Clear()
        {
            calls.Clear();
        }
    }

    public class GUIEvent : GUIEventBase
    {
        public void Invoke()
        {
            InvokeEvent();      
        }

        public void AddListener(MonoAction action)
        {
            if (action != null)
            {
                AddListener(action.Target, action.Method);
            }
        }

        public void RemoveListener(MonoAction action)
        {
            RemoveListener(action.Target, action.Method);
        }
    }

    public class GUIEvent<T0> : GUIEventBase
    {
        public void Invoke(T0 arg0)
        {
            InvokeEvent(arg0);
        }

        public void AddListener(MonoAction<T0> action)
        {
            if (action != null)
            {
                AddListener(action.Target, action.Method);
            }
        }

        public void RemoveListener(MonoAction<T0> action)
        {
            RemoveListener(action.Target, action.Method);
        }
    }

    public class GUIEvent<T0, T1> : GUIEventBase
    {
        public void Invoke(T0 arg0, T1 arg1)
        {
            InvokeEvent(arg0, arg1);
        }

        public void AddListener(MonoAction<T0, T1> action)
        {
            if (action != null)
            {
                AddListener(action.Target, action.Method);
            }
        }

        public void RemoveListener(MonoAction<T0, T1> action)
        {
            RemoveListener(action.Target, action.Method);
        }
    }
}
