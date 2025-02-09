using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public class TriggerEvent
    {
        public RoutedEvent RoutedEvent { get; }
        public Delegate Handler { get; }

        public TriggerEvent(RoutedEvent routedEvent, Delegate handler)
        {
            RoutedEvent = routedEvent;
            Handler = handler;
        }
    }

    public sealed class RoutedEvent
    {
        private static List<RoutedEvent> registeredEvents = new List<RoutedEvent>();
        private static int count = 0;

        public static int RegisteredEventsCount => registeredEvents.Count;

        public int Id { get; }

        internal RoutedEvent()
        {
            Id = count++;
            registeredEvents.Add(this);
        }
    }

    public class EventHandlersStore
    {
        private Dictionary<int, GUIEventBase> delegates = new Dictionary<int, GUIEventBase>();

        public int Count => delegates.Count;

        public GUIEvent Create(RoutedEvent routedEvent)
        {
            return AddEvent(routedEvent, new GUIEvent());
        }

        public GUIEvent<T0> Create<T0>(RoutedEvent routedEvent)
        {
            return AddEvent(routedEvent, new GUIEvent<T0>());
        }


        public GUIEvent<T0, T1> Create<T0, T1>(RoutedEvent routedEvent)
        {
            return AddEvent(routedEvent, new GUIEvent<T0, T1>());
        }

        private T AddEvent<T>(RoutedEvent routedEvent, T eventBase) where T : GUIEventBase
        {
            delegates[routedEvent.Id] = eventBase;
            return eventBase;
        }


        public void AddRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            Add(routedEvent.Id, handler);
        }

        private void Add(int id, Delegate handler)
        {
            if (delegates.ContainsKey(id))
            {
                delegates[id].AddDelegate(handler);
            }
        }

        public bool Contains(RoutedEvent key)
        {
            return delegates.ContainsKey(key.Id);
        }

        public void RemoveRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            Remove(routedEvent.Id, handler);
        }

        private void Remove(int id, Delegate handler)
        {
            if (delegates.ContainsKey(id))
            {
                delegates[id].RemoveDelegate(handler);
            }
        }
    }
}
