using System;
using Fitamas.UserInterface.Components;

namespace Fitamas.UserInterface
{
    public class Trigger : TriggerBase
    {
        private object value;

        public DependencyProperty Property { get; set; }

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public Trigger(DependencyProperty property, object value)
        {
            Property = property;
            Value = value;
        }

        internal override bool HasPropertyCondition(DependencyProperty property)
        {
            return Property == property;
        }

        internal override bool IsActive(GUIComponent component)
        {
            if (Equals(component.GetValue(Property), Value))
            {
                return true;
            }

            return false;
        }

        internal override void CheckState(GUIComponent component)
        {
            if (IsActive(component))
            {
                component.ActiveTriggers.Push(this); //AddToFront(this);
                ProcessSettersCollection(component);
            }
        }
    }
}
