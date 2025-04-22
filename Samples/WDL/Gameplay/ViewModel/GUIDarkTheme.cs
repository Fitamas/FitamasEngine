using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System;

namespace WDL.Gameplay.ViewModel
{
    public static class GUIDarkTheme
    {
        public static void CreateColors(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 0] = new Color(0.4f, 0, 0);
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 1] = new Color(0, 0.4f, 0);
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor + 2] = new Color(0, 0, 0.4f);

            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 0] = new Color(1f, 0, 0);
            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 1] = new Color(0, 1f, 0);
            dictionary[GUIStyleHelpers.NodeEditorWirePoweredColor + 2] = new Color(0, 0, 1f);

            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 0] = new Color(1f, 0.4f, 0.4f);
            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 1] = new Color(0.4f, 1f, 0.4f);
            dictionary[CommonResourceKeys.NodeEditorWireHoverColor + 2] = new Color(0.4f, 0.4f, 1f);

            dictionary[CommonResourceKeys.NodeEditorNodeHeadDefaultColor + 0] = new Color(0.07f, 0.43f, 0.7f);

            dictionary[CommonResourceKeys.NodeEditorNodeBodyDefaultColor + 0] = new Color(0.1f, 0.6f, 0.8f);
        }
    }
}
