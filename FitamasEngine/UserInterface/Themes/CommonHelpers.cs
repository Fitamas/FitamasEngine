using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public static class CommonHelpers
    {
        public static void CreateStyles(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.ButtonStyle] = GUIButtonStyle.Create(dictionary);
            dictionary[CommonResourceKeys.TextBlockStyle] = GUITextBlockStyle.Create(dictionary);
            dictionary[CommonResourceKeys.CheckBoxStyle] = GUICheckBoxStyle.Create(dictionary);
        }

        public static Trigger CreateTriggerForButton<T>(ResourceDictionary dictionary, DependencyProperty<T> property, T value, string imageColor, string textColor, string targetName = null)
        {
            Trigger trigger = new Trigger(property, value);

            if (!string.IsNullOrEmpty(imageColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, imageColor), targetName));
            }

            if (!string.IsNullOrEmpty(textColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, textColor), targetName));
            }

            return trigger;
        }

        public static MultiTrigger CreateTriggerForButton(ResourceDictionary dictionary, List<TriggerCondition> conditions, string imageColor, string textColor, string targetName = null)
        {
            MultiTrigger trigger = new MultiTrigger(conditions);

            if (!string.IsNullOrEmpty(imageColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, imageColor), targetName));
            }

            if (!string.IsNullOrEmpty(textColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, textColor), targetName));
            }                

            return trigger;
        }
    }
}
