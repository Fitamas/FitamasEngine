using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIButtonStyle
    {
        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle();

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ButtonDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ButtonTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() { 
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true), 
                new TriggerCondition(GUIComponent.InterectebleProperty, true)}, 
                CommonResourceKeys.ButtonHoverColor, CommonResourceKeys.ButtonTextHoverColor);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true, 
                CommonResourceKeys.ButtonPressedColor, CommonResourceKeys.ButtonTextPressedColor);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InterectebleProperty, false, 
                CommonResourceKeys.ButtonDisableColor, CommonResourceKeys.ButtonTextDisableColor);
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
