using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System;

namespace WDL.Gameplay.View
{
    public static class GUIDarkTheme
    {
        public static void CreateColors(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.NodeEditorTextColor] = Color.White;

            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 0] = new Color(0.4f, 0, 0);
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 1] = new Color(0, 0.4f, 0);
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 2] = new Color(0, 0, 0.4f);

            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 0] = new Color(1f, 0, 0);
            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 1] = new Color(0, 1f, 0);
            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 2] = new Color(0, 0, 1f);

            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 0] = new Color(1f, 0.4f, 0.4f);
            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 1] = new Color(0.4f, 1f, 0.4f);
            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 2] = new Color(0.4f, 0.4f, 1f);

            dictionary[CommonResourceKeys.NodeEditorNodeHeadDefaultColor + 0] = new Color(0.6f, 0.1f, 0.1f);
            dictionary[CommonResourceKeys.NodeEditorNodeHeadDefaultColor + 1] = new Color(0.1f, 0.6f, 0.3f);
            dictionary[CommonResourceKeys.NodeEditorNodeHeadDefaultColor + 2] = new Color(0.07f, 0.43f, 0.7f);

            dictionary[CommonResourceKeys.NodeEditorNodeBodyDefaultColor + 0] = new Color(0.8f, 0.1f, 0.1f);
            dictionary[CommonResourceKeys.NodeEditorNodeBodyDefaultColor + 1] = new Color(0.1f, 0.8f, 0.3f);
            dictionary[CommonResourceKeys.NodeEditorNodeBodyDefaultColor + 2] = new Color(0.1f, 0.6f, 0.8f);

            dictionary[CommonResourceKeys.ItemDefaultColor] = new Color();
            dictionary[CommonResourceKeys.ItemHoverColor] = new Color(1, 1, 1, 0.1f);
            dictionary[CommonResourceKeys.ItemPressedColor] = new Color(1, 1, 1, 0.3f);
            dictionary[CommonResourceKeys.ItemDisableColor] = new Color();
            dictionary[CommonResourceKeys.ItemTextDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextHoverColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextPressedColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextDisableColor] = new Color(0.75f, 0.75f, 0.75f);

            dictionary[CommonResourceKeys.ContextMenuDefaultColor] = new Color(0.4f, 0.4f, 0.4f);

            dictionary[CommonResourceKeys.WindowBackgroundColor] = new Color(0.4f, 0.4f, 0.4f);
            dictionary[CommonResourceKeys.WindowHeaderColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.WindowTextColor] = Color.Black;
        }
    }
}
