using System;
using System.Collections.Generic;

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
            foreach (TriggerBase item in triggers)
            {
                if (item is Trigger trigger)
                {
                    ProcessTrigger(component, trigger.Property, trigger);
                }

                if (item is MultiTrigger multiTrigger)
                {
                    foreach (var condition in multiTrigger.Conditions)
                    {
                        ProcessTrigger(component, condition.Property, multiTrigger);
                    }
                }
            }
        }

        internal static void ProcessTriggers(GUIComponent component, DependencyProperty property, List<TriggerBase> triggers)
        {
            List<TriggerBase> activeTriggers = new List<TriggerBase>();
            List<TriggerBase> activeTriggers1 = new List<TriggerBase>();

            foreach (TriggerBase trigger in triggers)
            {
                if (trigger.IsActive(component))
                {
                    if (trigger.HasPropertyCondition(property))
                    {
                        activeTriggers.Add(trigger);
                    }
                    else
                    {
                        activeTriggers1.Add(trigger);
                    }
                }

                trigger.RestorePropertyValues(component);
            }

            foreach (TriggerBase trigger in activeTriggers1)
            {
                trigger.CheckState(component);
            }

            foreach (TriggerBase trigger in activeTriggers)
            {
                trigger.CheckState(component);
            }
        }

        internal static void ProcessTrigger(GUIComponent component, DependencyProperty property, TriggerBase trigger)
        {
            if (trigger.HasPropertyCondition(property))
            {
                if (trigger.IsActive(component))
                {
                    trigger.CheckState(component);
                }
                else
                {
                    trigger.RestorePropertyValues(component);
                }
            }
        }
    }
}
