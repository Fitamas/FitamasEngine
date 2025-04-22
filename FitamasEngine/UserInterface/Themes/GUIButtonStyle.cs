using Fitamas.UserInterface.Components;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIButtonStyle
    {
        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ButtonDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ButtonTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.ButtonDisableColor, CommonResourceKeys.ButtonTextDisableColor);
            style.Trigges.Add(trigger);

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true,
                CommonResourceKeys.ButtonPressedColor, CommonResourceKeys.ButtonTextPressedColor);
            style.Trigges.Add(trigger);

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() { 
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true), 
                new TriggerCondition(GUIComponent.InteractebleProperty, true)}, 
                CommonResourceKeys.ButtonHoverColor, CommonResourceKeys.ButtonTextHoverColor);
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
