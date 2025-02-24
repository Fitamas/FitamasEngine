using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIComboBoxStyle
    {
        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ComboBoxDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ComboBoxTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.ComboBoxDisableColor, CommonResourceKeys.ComboBoxTextDisableColor);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)},
                CommonResourceKeys.ComboBoxHoverColor, CommonResourceKeys.ComboBoxTextHoverColor);
            style.Trigges.Add(trigger);

            style.TriggerEvents.Add(new TriggerEvent(GUIComboBox.OnSelectItemEvent, SetText));

            void SetText(GUIComboBox comboBox, string item)
            {
                comboBox.SetValue(GUITextBlock.TextProperty, item);
            }

            return style;
        }
    }
}
