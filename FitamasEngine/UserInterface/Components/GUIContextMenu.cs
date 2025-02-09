using Fitamas.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public class GUIContextMenu : GUIPopup
    {
        public static readonly RoutedEvent routedEvent = new RoutedEvent();

        private GUIVerticalGroup group;

        public GUIEvent<GUIContextItem> OnClick { get; }

        public GUIContextMenu()
        {
            OnClick = eventHandlersStore.Create<GUIContextItem>(routedEvent);

            group = new GUIVerticalGroup();
            group.Pivot = new Vector2(0, 0);
            group.SetAlignment(GUIAlignment.LeftTop);
            AddChild(group);

            RaycastTarget = true;
        }

        public void AddItems(IEnumerable<string> items)
        {
            foreach (var item in items)
            {
                AddItem(item);
            }
        }

        public GUIContextItem AddItem(string name)
        {
            GUIContextItem item = GUI.CreateContextItem(name);
            item.Menu = this;
            group?.AddChild(item);
            return item;
        }

        public void Activate(GUIContextItem item)
        {
            if (ChildrensComponent.Contains(item))
            {
                OnClick.Invoke(item);
            }
        }
    }
}
