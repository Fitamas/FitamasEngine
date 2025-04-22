using Fitamas.UserInterface.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIContextStyle
    {
        public static GUIStyle CreateMenu(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ContextMenuDefaultColor)));

            return style;
        }

        public static GUIStyle CreateItem(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ItemDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ItemTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.ItemDisableColor, CommonResourceKeys.ItemTextDisableColor);
            style.Trigges.Add(trigger);

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true,
                CommonResourceKeys.ItemPressedColor, CommonResourceKeys.ItemTextPressedColor);
            style.Trigges.Add(trigger);

            trigger = GUICommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)},
                CommonResourceKeys.ItemHoverColor, CommonResourceKeys.ItemTextHoverColor);
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
