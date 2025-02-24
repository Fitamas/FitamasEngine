using Fitamas.UserInterface.Themes;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public abstract class TriggerBase
    {
        private class Store
        {
            public GUIComponent Component { get; }
            public DependencyProperty Property { get; }
            public Expression Expression { get; }

            public Store(GUIComponent component, DependencyProperty property, Expression expression)
            {
                Component = component;
                Property = property;
                Expression = expression;
            }
        }

        private Dictionary<DependencyObject, List<Store>> storeProperty;

        public List<Setter> Setters { get; }

        public TriggerBase()
        {
            storeProperty = new Dictionary<DependencyObject, List<Store>>();
            Setters = new List<Setter>();
        }

        internal virtual bool HasPropertyCondition(DependencyProperty property)
        {
            return false;
        }

        internal virtual bool IsActive(GUIComponent component)
        {
            return false;
        }

        internal virtual void CheckState(GUIComponent component)
        {
        }

        internal void ProcessSettersCollection(GUIComponent component)
        {
            if (storeProperty.ContainsKey(component))
            {
                return; 
            }

            List<Store> properies = new List<Store>();
            storeProperty.Add(component, properies);

            foreach (Setter setter in Setters)
            {
                GUIComponent component1;

                if (!string.IsNullOrEmpty(setter.TargetName))
                {
                    component1 = component.GetComponentFromName(setter.TargetName, false);
                }
                else
                {
                    component1 = component;
                }

                if (component1 != null)
                {
                    Store store = new Store(component1, setter.Property, component1.GetExpression(setter.Property));
                    properies.Add(store);
                    component1.SetExpression(setter.Property, setter.Expression);
                }
            }
        }

        internal void RestorePropertyValues(GUIComponent component)
        {
            if (!storeProperty.ContainsKey(component))
            {
                return;
            }

            foreach (var item in storeProperty[component])
            {
                item.Component.SetExpression(item.Property, item.Expression);
            }

            storeProperty.Remove(component);
        }
    }
}
