using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUITreeStyle
    {
        public static readonly string Header = nameof(Header);

        public static readonly string Container = nameof(Container);

        public static readonly string ArrowIcon = nameof(ArrowIcon);

        public static readonly string Icon = nameof(Icon);

        public static GUIStyle CreateTreeViewStyle(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITreeView.IndentProperty, dictionary, CommonResourceKeys.TreeViewIndent)));

            return style;
        }

        public static GUIStyle CreateTreeNodeStyle(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ItemDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ItemTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            TriggerBase trigger;

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.ItemDisableColor, CommonResourceKeys.ItemTextDisableColor);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIButton.IsPressedProperty, true,
                CommonResourceKeys.ItemPressedColor, CommonResourceKeys.ItemTextPressedColor);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)},
                CommonResourceKeys.ItemHoverColor, CommonResourceKeys.ItemTextHoverColor);
            style.Trigges.Add(trigger);

            trigger = new Trigger(GUITreeNode.IsOpenProperty, true);
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ButtonDisableColor), ArrowIcon));
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
