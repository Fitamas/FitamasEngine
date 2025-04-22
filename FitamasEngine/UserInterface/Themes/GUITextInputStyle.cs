using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.Themes
{
    public static class GUITextInputStyle
    {
        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.TextInputDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.TextInputTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.TextInputDisableColor);
            style.Trigges.Add(trigger);

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.IsFocusedProperty, true,
                CommonResourceKeys.TextInputFocusedColor);
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
