using System;
using System.Collections.Generic;
using Fitamas.UserInterface.Components;

namespace Fitamas.UserInterface
{
    public class MultiTrigger : TriggerBase
    {
        public List<TriggerCondition> Conditions { get; }

        public MultiTrigger(List<TriggerCondition> conditions)
        {
            Conditions = conditions;
        }

        internal override bool HasPropertyCondition(DependencyProperty property)
        {
            foreach (TriggerCondition condition in Conditions)
            {
                if (condition.Property == property)
                {
                    return true;
                }
            }

            return false;
        }

        internal override bool IsActive(GUIComponent component)
        {
            bool flag = true;
            foreach (TriggerCondition condition in Conditions)
            {
                if (!Equals(component.GetValue(condition.Property), condition.Value))
                {
                    flag = false;
                }
            }

            return flag;
        }

        internal override void CheckState(GUIComponent component)
        {
            if (IsActive(component))
            {
                component.ActiveTriggers.Push(this);// AddToFront(this);
                ProcessSettersCollection(component);
            }
        }
    }
}
