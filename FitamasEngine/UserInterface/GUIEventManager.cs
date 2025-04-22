using Fitamas.Events;
using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fitamas.UserInterface
{
    public static class GUIEventManager
    {
        private static Dictionary<RoutedEvent, GUIEventBase> eventMap = new Dictionary<RoutedEvent, GUIEventBase>();

        public static void Register(RoutedEvent routedEvent, GUIEventBase handler)
        {
            if (!eventMap.ContainsKey(routedEvent))
            {
                eventMap.Add(routedEvent, handler);
            }
        }

        public static GUIEventBase GetEvent(RoutedEvent routedEvent)
        {
            if (eventMap.TryGetValue(routedEvent, out GUIEventBase eventBase))
            {
                return eventBase;
            }

            return null;
        }
    }
}
