using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUICheckBoxStyle
    {
        public static readonly string CheckMark = nameof(CheckMark);

        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle();

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.CheckBoxDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.CheckBoxMarkColor), CheckMark));

            TriggerBase trigger;

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InterectebleProperty, true)},
                CommonResourceKeys.CheckBoxHoverColor, null);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true,
                CommonResourceKeys.CheckBoxPressedColor, null);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InterectebleProperty, false,
                CommonResourceKeys.CheckBoxDisableColor, null);
            style.Trigges.Add(trigger);

            trigger = new Trigger(GUICheckBox.ValueProperty, true);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, true), CheckMark));
            style.Trigges.Add(trigger);

            trigger = new Trigger(GUICheckBox.ValueProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false), CheckMark));
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
