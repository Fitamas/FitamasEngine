using Fitamas.Audio;
using Fitamas.Core;
using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public class TriggerEvent
    {
        public RoutedEvent RoutedEvent { get; }
        public List<IAction> Actions { get; }

        public TriggerEvent(RoutedEvent routedEvent)
        {
            RoutedEvent = routedEvent;
            Actions = new List<IAction>();
        }

        internal void Handler(GUIComponent sender, GUIEventArgs args)
        {
            foreach (var action in Actions)
            {
                action.Execute(sender, args);
            }
        }
    }

    public interface IAction
    {
        void Execute(GUIComponent sender, GUIEventArgs args);
    }

    public class HandlerAction : IAction
    {
        public Delegate Delegate { get; set; }

        public void Execute(GUIComponent sender, GUIEventArgs args)
        {
            Delegate?.DynamicInvoke(sender, args);
        }
    }

    public class AudioSourceAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty<AudioClip> ClipProperty = new DependencyProperty<AudioClip>();

        private AudioSourceBusInstance sourceInstance;

        public AudioSourceBusInstance SourceInstance => sourceInstance;

        public AudioClip Clip
        {
            get
            {
                return GetValue(ClipProperty);
            }
            set
            {
                SetValue(ClipProperty, value);
            }
        }

        public AudioSourceAction()
        {
            sourceInstance = new AudioSourceBusInstance(GameEngine.Instance.AudioManager);
            sourceInstance.Play();
        }

        public void Execute(GUIComponent sender, GUIEventArgs args)
        {
            if (Clip == null)
            {
                return;
            }

            AudioClipInstance clipInstance = new AudioClipInstance(GameEngine.Instance.AudioManager, Clip);
            clipInstance.Source = sourceInstance;
            clipInstance.Play();
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
        private Dictionary<int, List<Delegate>> delegates;

        public EventHandlersStore()
        {
            delegates = new Dictionary<int, List<Delegate>>();
        }

        public void Invoke(GUIComponent sender, RoutedEvent routedEvent, GUIEventArgs args)
        {
            if (delegates.ContainsKey(routedEvent.Id))
            {
                foreach (var handler in delegates[routedEvent.Id])
                {
                    handler.DynamicInvoke(sender, args);
                }
            }
        }

        public void AddRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            Add(routedEvent.Id, handler);
        }

        private void Add(int id, Delegate handler)
        {
            if (!delegates.ContainsKey(id))
            {
                delegates[id] = new List<Delegate>() { handler };
            }
            else
            {
                delegates[id].Add(handler);
            }
        }

        public void RemoveRoutedEventHandler(RoutedEvent routedEvent, Delegate handler)
        {
            Remove(routedEvent.Id, handler);
        }

        private void Remove(int id, Delegate handler)
        {
            if (delegates.ContainsKey(id))
            {
                delegates[id].Remove(handler);

                if (delegates[id].Count == 0)
                {
                    delegates.Remove(id);
                }
            }
        }
    }
}
