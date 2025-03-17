using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUIItemsControl : GUIComponent
    {
        public List<GUIComponent> Items { get; }
        public GUIComponent Container { get; set; }

        public GUIItemsControl()
        {
            Items = new List<GUIComponent>();
        }

        public void AddItem(GUIComponent component)
        {
            if (IsItemItsOwnContainerOverride(component))
            {
                Items.Add(component);
                Container.AddChild(component);
                OnAddItem(component);
            }
        }

        public void RemoveItem(GUIComponent component)
        {
            if (Items.Remove(component))
            {
                component.Parent = null;
                OnRemoveItem(component);
            }
        }

        protected virtual bool IsItemItsOwnContainerOverride(GUIComponent component)
        {
            return true;
        }

        protected virtual void OnAddItem(GUIComponent component)
        {

        }

        protected virtual void OnRemoveItem(GUIComponent component)
        {

        }
    }
}
