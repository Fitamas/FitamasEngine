using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Themes
{
    public static class CommonHelpers
    {
        public static void CreateStyles(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.ButtonStyle] = GUIButtonStyle.Create(dictionary);
            dictionary[CommonResourceKeys.CheckBoxStyle] = GUICheckBoxStyle.Create(dictionary);
            dictionary[CommonResourceKeys.ComboBoxStyle] = GUIComboBoxStyle.Create(dictionary);
            dictionary[CommonResourceKeys.ContextItemStyle] = GUIContextStyle.CreateItem(dictionary);
            dictionary[CommonResourceKeys.ContextMenuStyle] = GUIContextStyle.CreateMenu(dictionary);
            dictionary[CommonResourceKeys.TextBlockStyle] = GUITextBlockStyle.Create(dictionary);
            dictionary[CommonResourceKeys.TextInputStyle] = GUITextInputStyle.Create(dictionary);
            dictionary[CommonResourceKeys.TrackBarStyle] = GUITrackBarStyle.CreateTrackBar(dictionary);
            dictionary[CommonResourceKeys.TrackBarThumbStyle] = GUITrackBarStyle.CreateThumb(dictionary);
            dictionary[CommonResourceKeys.TreeViewStyle] = GUITreeStyle.CreateTreeViewStyle(dictionary);
            dictionary[CommonResourceKeys.TreeNodeStyle] = GUITreeStyle.CreateTreeNodeStyle(dictionary);
        }

        public static void CreateVars(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.WindowPadding] = new Point(3, 3);
            dictionary[CommonResourceKeys.FramePadding] = new Point(10, 10);
            dictionary[CommonResourceKeys.ScrollbarSize] = 10;
            dictionary[CommonResourceKeys.TreeViewIndent] = 20;
        }

        public static Trigger CreateTriggerForButton<T>(ResourceDictionary dictionary, DependencyProperty<T> property, T value, string imageColor = null, string textColor = null, string targetName = null)
        {
            Trigger trigger = new Trigger(property, value);

            if (!string.IsNullOrEmpty(imageColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, imageColor), targetName));
            }

            if (!string.IsNullOrEmpty(textColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, textColor), targetName));
            }

            return trigger;
        }

        public static MultiTrigger CreateTriggerForButton(ResourceDictionary dictionary, List<TriggerCondition> conditions, string imageColor = null, string textColor = null, string targetName = null)
        {
            MultiTrigger trigger = new MultiTrigger(conditions);

            if (!string.IsNullOrEmpty(imageColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUIImage.ColorProperty, dictionary, imageColor), targetName));
            }

            if (!string.IsNullOrEmpty(textColor))
            {
                trigger.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, textColor), targetName));
            }                

            return trigger;
        }
    }
}
