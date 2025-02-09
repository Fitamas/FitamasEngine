using Fitamas;
using Fitamas.Main;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using R3;
using SharpFont.Bdf;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public class GUIStyle
    {
        public List<Setter> Setters { get; }
        public List<TriggerBase> Trigges { get; }
        public List<TriggerEvent> TriggerEvents { get; }
        public GUIStyle BaseOn { get; set; }

        public GUIStyle(GUIStyle baseOn = null)
        {
            Setters = new List<Setter>();
            Trigges = new List<TriggerBase>();
            TriggerEvents = new List<TriggerEvent>();
            BaseOn = baseOn;
        }

        public void ApplyStyle(GUIComponent component)
        {
            if (component == null)
            {
                return;
            }

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
    }
}