using Fitamas.Events;
using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface
{
    public class TriggerAction<T> : TriggerBase
    {
        public DependencyProperty<T> Property { get; set; }

        public MonoAction<GUIComponent, DependencyProperty<T>> Action { get; set; }

        public TriggerAction(DependencyProperty<T> property, MonoAction<GUIComponent, DependencyProperty<T>> action) 
        { 
            Property = property;
            Action = action;
        }

        internal override bool HasPropertyCondition(DependencyProperty property)
        {
            return Property == property;
        }

        internal override void CheckState(GUIComponent component)
        {
            if (IsActive(component))
            {
                ProcessSettersCollection(component);
                Action?.Invoke(component, Property);
            }
        }
    }
}
