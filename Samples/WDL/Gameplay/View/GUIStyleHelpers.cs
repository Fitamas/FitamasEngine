using Fitamas;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.Input;
using Fitamas.UserInterface.Themes;
using System;
using System.Collections.Generic;

namespace WDL.Gameplay.View
{
    public static class GUIStyleHelpers
    {
        public static readonly string CheckBoxStyle1 = nameof(CheckBoxStyle1);

        public static readonly string PumpkinTexture = nameof(PumpkinTexture);

        public static readonly string PinOffTexture = nameof(PinOffTexture);

        public static readonly string PinOnTexture = nameof(PinOnTexture);

        public static readonly string OpenTreeNodeTexture = nameof(OpenTreeNodeTexture);

        public static readonly string CloseTreeNodeTexture = nameof(CloseTreeNodeTexture);

        public static readonly string EnableButtonTexture = nameof(EnableButtonTexture);

        public static readonly string DisableButtonTexture = nameof(DisableButtonTexture);

        public static readonly string FolderTexture = nameof(FolderTexture);

        public static readonly string CircuitTexture = nameof(CircuitTexture);

        public static readonly string ButtonPressedAudio = nameof(ButtonPressedAudio);

        public static readonly string ButtonHoverAudio = nameof(ButtonHoverAudio);

        public static GUIStyle CreateButton(ResourceDictionary dictionary)
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

            TriggerEvent triggerEvent;
            AudioSourceAction action;

            triggerEvent = new TriggerEvent(GUIMouse.MouseEnterGUIEvent);
            action = new AudioSourceAction();
            action.SetResourceReference(AudioSourceAction.ClipProperty, dictionary, ButtonHoverAudio);
            triggerEvent.Actions.Add(action);
            style.TriggerEvents.Add(triggerEvent);

            triggerEvent = new TriggerEvent(GUIButton.OnClickedEvent);
            action = new AudioSourceAction();
            action.SetResourceReference(AudioSourceAction.ClipProperty, dictionary, ButtonPressedAudio);
            triggerEvent.Actions.Add(action);
            style.TriggerEvents.Add(triggerEvent);

            return style;
        }

        public static GUIStyle CreateTreeNode(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.ItemDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.ItemTextDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.TreeNodeArrowColor), GUITreeStyle.ArrowIcon));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, CloseTreeNodeTexture), GUITreeStyle.ArrowIcon));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.TreeNodeIconColor), GUITreeStyle.Icon));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, FolderTexture), GUITreeStyle.Icon));

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

            trigger = new Trigger(GUITreeNode.IsOpenProperty, true);
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, OpenTreeNodeTexture), GUITreeStyle.ArrowIcon));
            style.Trigges.Add(trigger);

            trigger = new Trigger(GUITreeNode.IsLeafProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false, true), GUITreeStyle.ArrowIcon));
            style.Trigges.Add(trigger);

            return style;
        }

        public static GUIStyle CreateCheckBox(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, DisableButtonTexture)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, EnableButtonTexture), GUICheckBoxStyle.CheckMark));

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

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.NodeEditorTextColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeHeadDefaultColor + index), GUINodeEditorStyle.NodeHead));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeBodyDefaultColor + index), GUINodeEditorStyle.NodeBody));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.NodeEditorNodeSelectColor), GUINodeEditorStyle.NodeSelect));

            TriggerBase trigger;

            trigger = new Trigger(GUINode.IsSelectProperty, false);
            trigger.Setters.Add(new Setter(new ValueExpression<bool>(GUIComponent.EnableProperty, false), GUINodeEditorStyle.NodeSelect));
            style.Trigges.Add(trigger);

            return style;
        }

        public static GUIStyle CreatePin(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, PinOffTexture)));

            TriggerBase trigger;

            trigger = new Trigger(GUIPin.IsConnectedProperty, true);
            trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.SpriteProperty, dictionary, PinOnTexture)));
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
