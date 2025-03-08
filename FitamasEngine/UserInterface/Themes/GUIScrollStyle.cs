using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class GUIScrollStyle
    {
        public static readonly string SlidingArea = nameof(SlidingArea);

        public static readonly string Thumb = nameof(Thumb);

        public static GUIStyle CreateTrackBar(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.TrackBarDefaultColor), SlidingArea));

            return style;
        }

        public static GUIStyle CreateThumb(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle(dictionary);

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, CommonResourceKeys.TrackBarThumbDefaultColor), Thumb));

            TriggerBase trigger;

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIComponent.InteractebleProperty, false,
                CommonResourceKeys.TrackBarThumbDisableColor, targetName: Thumb);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, GUIThumb.IsDragProperty, true,
                CommonResourceKeys.TrackBarThumbPressedColor, targetName: Thumb);
            style.Trigges.Add(trigger);

            trigger = CommonHelpers.CreateTriggerForButton(dictionary, new List<TriggerCondition>() {
                new TriggerCondition(GUIComponent.IsMouseOverProperty, true),
                new TriggerCondition(GUIComponent.InteractebleProperty, true)},
                CommonResourceKeys.TrackBarThumbHoverColor, targetName: Thumb);
            style.Trigges.Add(trigger);

            return style;
        }
    }
}
