using System.Collections.Generic;
using Fitamas.UserInterface.Components;

namespace Fitamas.UserInterface.Themes
{
    public class GUIStyle
    {
        public List<Setter> Setters { get; }
        public List<TriggerBase> Trigges { get; }
        public List<TriggerEvent> TriggerEvents { get; }
        public GUIStyle BaseOn { get; set; }
        public ResourceDictionary Resources { get; set; }

        public GUIStyle(ResourceDictionary resources, GUIStyle baseOn = null)
        {
            Setters = new List<Setter>();
            Trigges = new List<TriggerBase>();
            TriggerEvents = new List<TriggerEvent>();
            Resources = resources;
            BaseOn = baseOn;
        }

        public void ApplyStyle(GUIComponent component)
        {
            if (component == null)
            {
                return;
            }

            ApplySetters(component);

            if (BaseOn != null && this != BaseOn)
            {
                BaseOn.ApplyStyle(component);
            }

            GUIStyleHelpers.UpdateTriggers(component, Trigges);

            GUIStyleHelpers.UpdateTriggerEvents(component, TriggerEvents);
        }

        public void ProcessTriggers<T>(GUIComponent component, DependencyProperty<T> property)
        {
            GUIStyleHelpers.ProcessTriggers(component, property, Trigges);
        }

        internal void ApplySetters(GUIComponent component)
        {
            foreach (var setter in Setters)
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
                    component1.SetExpression(setter.Property, setter.Expression);
                }
            }
        }
    }
}