using Fitamas.Events;
using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.UserInterface
{
    public static class GUIEventManager
    {
        private static Dictionary<RoutedEvent, MonoEventBase> eventMap = new Dictionary<RoutedEvent, MonoEventBase>();

        public static void Register(RoutedEvent routedEvent, MonoEventBase handler)
        {
            if (!eventMap.ContainsKey(routedEvent))
            {
                eventMap.Add(routedEvent, handler);
            }
        }

        public static MonoEventBase GetEvent(RoutedEvent routedEvent)
        {
            if (eventMap.TryGetValue(routedEvent, out MonoEventBase eventBase))
            {
                return eventBase;
            }

            return null;
        }
    }
}
