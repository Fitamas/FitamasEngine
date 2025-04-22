using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUINodeEditorStyle
    {
        public static readonly string SelectRegion = nameof(SelectRegion);

        public static readonly string NodeHead = nameof(NodeHead);

        public static readonly string NodeBody = nameof(NodeBody);

        public static readonly string NodeSelect = nameof(NodeSelect);

        public static GUIStyle CreateNodeEditor(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorSelectColor), SelectRegion));

            return style;
        }

        public static GUIStyle CreateNode(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeHeadDefaultColor), NodeHead));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeBodyDefaultColor), NodeBody));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeSelectColor), NodeSelect));

            TriggerBase trigger;

            trigger = new Trigger(GUINode.IsSelectProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false), NodeSelect));
            style.Trigges.Add(trigger);

            return style;
        }

        public static GUIStyle CreatePin(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorPinDefaultColor)));

            TriggerBase trigger;

            trigger = new Trigger(GUIPin.IsConnectedProperty, true);
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorPinConnectedDefaultColor)));
            style.Trigges.Add(trigger);

            return style;
        }

        public static GUIStyle CreateWire(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUILineRenderer.ColorProperty, dictionary, CommonResourceKeys.NodeEditorWireDefaultColor)));

            style.Setters.Add(new Setter(new ValueExpression<int>(GUILineRenderer.ThicknessProperty, 10)));

            TriggerBase trigger;

            trigger = new MultiTrigger(new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)});
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUILineRenderer.ColorProperty, dictionary, CommonResourceKeys.NodeEditorWireHoverColor)));
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
