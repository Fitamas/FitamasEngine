using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Fitamas.UserInterface;
using System;
using System.Collections.Generic;
using Fitamas.UserInterface.Components.NodeEditor;

namespace WDL.Gameplay.ViewModel
{
    public static class GUIStyleHelpers
    {
        public static readonly string CheckBoxStyle1 = nameof(CheckBoxStyle1);

        public static GUIStyle CreateCheckBox(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.CheckBoxDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.CheckBoxMarkColor), GUICheckBoxStyle.CheckMark));

            TriggerBase trigger;

            trigger = new Trigger(GUICheckBox.ValueProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false, true), GUICheckBoxStyle.CheckMark));
            style.Trigges.Add(trigger);

            return style;
        }

        public static readonly DependencyProperty<bool> IsPoweredProperty = new DependencyProperty<bool>(false, false);

        public static readonly string NodeEditorWirePoweredColor = nameof(NodeEditorWirePoweredColor);

        public static GUIStyle CreateWire(ResourceDictionary dictionary, int index)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUILineRenderer.ColorProperty, dictionary, CommonResourceKeys.NodeEditorWireDefaultColor + index)));

            style.Setters.Add(new Setter(new ValueExpression<int>(GUILineRenderer.ThicknessProperty, 10)));

            TriggerBase trigger;

            trigger = new Trigger(IsPoweredProperty, true);
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUILineRenderer.ColorProperty, dictionary, NodeEditorWirePoweredColor + index)));
            style.Trigges.Add(trigger);

            trigger = new MultiTrigger(new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)});
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUILineRenderer.ColorProperty, dictionary, CommonResourceKeys.NodeEditorWireHoverColor + index)));
            style.Trigges.Add(trigger);

            return style;
        }

        public static GUIStyle CreateNode(ResourceDictionary dictionary, int index)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeHeadDefaultColor + index), GUINodeEditorStyle.NodeHead));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeBodyDefaultColor + index), GUINodeEditorStyle.NodeBody));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeSelectColor), GUINodeEditorStyle.NodeSelect));

            TriggerBase trigger;

            trigger = new Trigger(GUINode.IsSelectProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false), GUINodeEditorStyle.NodeSelect));
            style.Trigges.Add(trigger);

            return style;
        }

    }
}
