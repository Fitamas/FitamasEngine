using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Input;
using System;

namespace Fitamas.UserInterface
{
    public static class GUIContextMenuManager
    {
        public static readonly DependencyProperty<GUIContextMenu> ContextMenuProperty = new DependencyProperty<GUIContextMenu>(null, false);

        public static readonly RoutedEvent ContextMenuOpeningEvent = new RoutedEvent();

        public static readonly RoutedEvent ContextMenuClosingEvent = new RoutedEvent();

        static GUIContextMenuManager()
        {
            GUIEventManager.Register(ContextMenuOpeningEvent, new GUIEvent<GUIComponent, GUIContextMenuEventArgs>(OnOpenContextMenu));
            GUIEventManager.Register(ContextMenuClosingEvent, new GUIEvent<GUIComponent, GUIContextMenuEventArgs>(OnCloseContextMenu));

            GUIEventManager.GetEvent(GUIMouse.MouseUpEvent).AddDelegate(OpenContextMenu);
        }

        public static GUIContextMenu GetContextMenu(GUIComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            GUIContextMenu contextMenu = component.GetValue(ContextMenuProperty);

            return contextMenu;
        }

        public static void SetContextMenu(GUIComponent component, GUIContextMenu contextMenu)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            component.SetValue(ContextMenuProperty, contextMenu);
        }

        public static void AddContextMenuOpeningHandler(GUIComponent component, Action<GUIComponent, GUIContextMenuEventArgs> handler)
        {
            component.AddRoutedEventHandler(ContextMenuOpeningEvent, handler);
        }

        public static void RemoveContextMenuOpeningHandler(GUIComponent component, Action<GUIComponent, GUIContextMenuEventArgs> handler)
        {
            component.RemoveRoutedEventHandler(ContextMenuOpeningEvent, handler);
        }

        public static void AddContextMenuClosingHandler(GUIComponent component, Action<GUIComponent, GUIContextMenuEventArgs> handler)
        {
            component.AddRoutedEventHandler(ContextMenuClosingEvent, handler);
        }

        public static void RemoveContextMenuClosingHandler(GUIComponent component, Action<GUIComponent, GUIContextMenuEventArgs> handler)
        {
            component.RemoveRoutedEventHandler(ContextMenuClosingEvent, handler);
        }

        private static void OpenContextMenu(GUIComponent component, GUIMouseEventArgs args)
        {
            if (args.Button != Fitamas.Input.MouseButton.Right || !component.IsMouseOver)
            {
                return;
            }

            GUIContextMenu contextMenu = GetContextMenu(component);

            if (contextMenu != null)
            {
                contextMenu.IsOpen = true;
                contextMenu.LocalPosition = contextMenu.Parent.ToLocal(args.Position);
                contextMenu.Target = component;
                component.RaiseEvent(new GUIContextMenuEventArgs(component, args.Position, ContextMenuOpeningEvent));
            }
        }

        private static void OnOpenContextMenu(GUIComponent sender, GUIContextMenuEventArgs args)
        {
            if (sender is IContextMenuHandler handler)
            {
                handler.OnOpen(args);
            }
        }

        private static void OnCloseContextMenu(GUIComponent sender, GUIContextMenuEventArgs args)
        {
            if (sender is IContextMenuHandler handler)
            {
                handler.OnClose(args);
            }
        }
    }
}
