using Fitamas.Events;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Components
{
    public class GUIContextMenu : GUIPopup
    {
        public static readonly RoutedEvent routedEvent = new RoutedEvent();

        public GUIEvent<GUIContextMenu, GUIContextItem> OnSelectItem { get; }

        public GUIGroup Group { get; set; }

        public GUIContextMenu()
        {
            OnSelectItem = eventHandlersStore.Create<GUIContextMenu, GUIContextItem>(routedEvent);
            SizeToContent = SizeToContent.WidthAndHeight;
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
            AddItem(item);
            return item;
        }

        public void AddItem(GUIContextItem item)
        {
            item.Menu = this;
            Group?.AddChild(item);
        }

        public void Activate(GUIContextItem item)
        {
            if (Group.ChildrensComponent.Contains(item))
            {
                OnSelectItem.Invoke(this, item);
                Close();
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
            
        }
    }
}
