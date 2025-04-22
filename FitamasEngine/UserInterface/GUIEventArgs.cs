using System;

namespace Fitamas.UserInterface
{
    public class GUIEventArgs
    {
        public RoutedEvent RoutedEvent { get; set; }
        public object Source { get; set; }

        public GUIEventArgs()
        {

        }

        public GUIEventArgs(RoutedEvent routedEvent, object source)
        {
            RoutedEvent = routedEvent;
            Source = source;
        }

        public GUIEventArgs(RoutedEvent routedEvent) : this(routedEvent, null)
        {

        }
    }
}
