using Fitamas.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Fitamas.UserInterface.Components;

namespace Fitamas.UserInterface
{
    public static class GUIStyleHelpers
    {
        internal static void UpdateTriggerEvents(GUIComponent component, List<TriggerEvent> triggers)
        {
            foreach (TriggerEvent trigger in triggers)
            {
                component.AddRoutedEventHandler(trigger.RoutedEvent, trigger.Handler);
            }
        }

        internal static void ClearTriggerEvents(GUIComponent component, List<TriggerEvent> triggers)
        {
            foreach (TriggerEvent trigger in triggers)
            {
                component.RemoveRoutedEventHandler(trigger.RoutedEvent, trigger.Handler);
            }
        }

        internal static void UpdateTriggers(GUIComponent component, List<TriggerBase> triggers)
        {
            List<TriggerBase> list = new List<TriggerBase>(1);
            foreach (TriggerBase item in triggers)
            {
                list.Add(item);
                if (item is Trigger trigger)
                {
                    ProcessTriggers(component, trigger.Property, list);
                }

                if (item is MultiTrigger multiTrigger)
                {
                    foreach (var condition in multiTrigger.Conditions)
                    {
                        ProcessTriggers(component, condition.Property, list);
                    }
                }
                list.Clear();
            }
        }

        internal static void ProcessTriggers(GUIComponent component, DependencyProperty property, List<TriggerBase> triggers)
        {
            foreach (TriggerBase item in triggers)
            {
                if (item.HasPropertyCondition(property) && !component.ActiveTriggers.Contains(item))
                {
                    item.CheckState(component);
                }
            }

            if (component.ActiveTriggers.Count <= 0)
            {
                return;
            }

            if (component.ActiveTriggers.Peek().HasPropertyCondition(property))
            {
                while (component.ActiveTriggers.Count > 0 && !component.ActiveTriggers.Peek().IsActive(component))
                {
                    component.ActiveTriggers.Pop().RestorePropertyValues(component);
                }

                return;
            }

            TriggerBase triggerBase = null;
            foreach (TriggerBase activeTrigger in component.ActiveTriggers)
            {
                if (activeTrigger.HasPropertyCondition(property))
                {
                    triggerBase = activeTrigger;
                    break;
                }
            }

            if (triggerBase != null && !triggerBase.IsActive(component))
            {
                List<TriggerBase> list = new List<TriggerBase>();
                while (component.ActiveTriggers.Count > 0 && component.ActiveTriggers.Peek() != triggerBase)
                {
                    TriggerBase triggerBase2 = component.ActiveTriggers.Pop();
                    list.Add(triggerBase2);
                    triggerBase2.RestorePropertyValues(component);
                }

                triggerBase = component.ActiveTriggers.Pop();
                triggerBase.RestorePropertyValues(component);
                foreach (var item in list)
                {
                    item.CheckState(component);
                }
            }
        }
    }
}
