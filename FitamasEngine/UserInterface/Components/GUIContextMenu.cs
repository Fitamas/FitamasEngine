using Fitamas.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public class GUIContextMenuEventArgs : GUIEventArgs
    {
        public GUIComponent Target { get; set; }
        public Point Position { get; set; }

        public GUIContextMenuEventArgs(GUIComponent target, Point position, RoutedEvent routedEvent) : base( routedEvent, target )
        {
            Target = target;
            Position = position;
        }
    }

    public interface IContextMenuHandler
    {
        void OnOpen(GUIContextMenuEventArgs args);
        void OnClose(GUIContextMenuEventArgs args);
    }

    public class GUISelectContextItemEventArgs : GUIEventArgs
    {
        public string Name { get; }
        public int Index { get; }

        public GUISelectContextItemEventArgs(RoutedEvent routedEvent, object source, string name, int index) : base(routedEvent, source)
        {
            Name = name;
            Index = index;
        }
    }

    public class GUIContextMenu : GUIWindow
    {
        public static readonly RoutedEvent OnSelectItemEvent = new RoutedEvent();

        private List<GUIContextItem> items;

        public MonoEvent<GUIContextMenu, GUISelectContextItemEventArgs> OnSelectItem { get; }

        public GUIGroup Group { get; set; }
        public GUIComponent Target { get; set; }

        public GUIContextMenu()
        {
            OnSelectItem = new MonoEvent<GUIContextMenu, GUISelectContextItemEventArgs>();
            SizeToContent = GUISizeToContent.WidthAndHeight;
            items = new List<GUIContextItem>();
        }

        public void SetItems(IEnumerable<string> items)
        {
            foreach (var item in this.items)
            {
                item.Destroy();
            }
            this.items.Clear();

            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public GUIContextItem AddItem(string name)
        {
            GUIContextItem item = GUI.CreateContextItem(name);
            AddItem(item);
            return item;
        }

        public void AddItem(GUIContextItem item)
        {
            item.Menu = this;
            Group?.AddChild(item);
            items.Add(item);
        }

        internal void Activate(GUIContextItem item)
        {
            if (Group.ChildrensComponent.Contains(item))
            {
                GUISelectContextItemEventArgs args = new GUISelectContextItemEventArgs(OnSelectItemEvent, this, item.Name, items.IndexOf(item));
                OnSelectItem.Invoke(this, args);
                RaiseEvent(args);
            }

            if (Parent is GUIPopup popup)
            {
                popup.IsOpen = false;
            }
        }

        public void SetFixedWidth(int width)
        {
            Thickness thickness = Padding;
            Group.ControlSizeWidth = false;
            Group.LocalSize = new Point(width - thickness.Left - thickness.Right, 0);
        }

        public void SetFixedHeight(int height)
        {
            Thickness thickness = Padding;
            Group.ControlSizeHeight = false;
            Group.LocalSize = new Point(height - thickness.Top - thickness.Bottom, 0);
        }
    }
}
