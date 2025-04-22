using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIWindowStyle
    {
        public static readonly string Header = nameof(Header);

        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.WindowBackgroundColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.WindowTextColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.WindowHeaderColor), Header));

            //TriggerBase trigger;

            //trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
            //    CommonResourceKeys.ItemDisableColor, CommonResourceKeys.ItemTextDisableColor);
            //style.Trigges.Add(trigger);

            //trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true,
            //    CommonResourceKeys.ItemPressedColor, CommonResourceKeys.ItemTextPressedColor);
            //style.Trigges.Add(trigger);

            //trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
            //    new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
            //    new TriggerCondition(GUIComponent.InteractebleProperty, true)},
            //    CommonResourceKeys.ItemHoverColor, CommonResourceKeys.ItemTextHoverColor);
            //style.Trigges.Add(trigger);

            return style;
        }
    }
}
