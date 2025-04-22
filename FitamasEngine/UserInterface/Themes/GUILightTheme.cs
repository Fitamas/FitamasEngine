using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Themes
{
    public static class GUILightTheme
    {
        public static void CreateColors(ResourceDictionary dictionary)
        {
            dictionary[CommonResourceKeys.ButtonDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.ButtonHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.ButtonPressedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.ButtonDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.ButtonTextDefaultColor] = Color.Black;
            dictionary[CommonResourceKeys.ButtonTextHoverColor] = Color.White;
            dictionary[CommonResourceKeys.ButtonTextPressedColor] = Color.White;
            dictionary[CommonResourceKeys.ButtonTextDisableColor] = Color.Black;

            dictionary[CommonResourceKeys.CheckBoxDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.CheckBoxHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.CheckBoxPressedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.CheckBoxDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.CheckBoxMarkColor] = Color.SkyBlue;

            dictionary[CommonResourceKeys.ComboBoxDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.ComboBoxHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.ComboBoxDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.ComboBoxTextDefaultColor] = Color.Black;
            dictionary[CommonResourceKeys.ComboBoxTextHoverColor] = Color.White;
            dictionary[CommonResourceKeys.ComboBoxTextDisableColor] = Color.Black;

            dictionary[CommonResourceKeys.ItemDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.ItemHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.ItemPressedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.ItemDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.ItemTextDefaultColor] = Color.Black;
            dictionary[CommonResourceKeys.ItemTextHoverColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextPressedColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextDisableColor] = Color.Black;

            dictionary[CommonResourceKeys.ContextMenuDefaultColor] = Color.LightGray;

            dictionary[CommonResourceKeys.NodeEditorSelectColor] = new Color(0.1f, 0.5f, 0.8f, 0.4f);
            dictionary[CommonResourceKeys.NodeEditorNodeHeadDefaultColor] = new Color(0.07f, 0.43f, 0.7f);
            dictionary[CommonResourceKeys.NodeEditorNodeBodyDefaultColor] = new Color(0.1f, 0.6f, 0.8f);
            dictionary[CommonResourceKeys.NodeEditorNodeSelectColor] = new Color(0.8f, 0.8f, 0.8f, 0.6f);
            dictionary[CommonResourceKeys.NodeEditorPinConnectedDefaultColor] = Color.Gray;
            dictionary[CommonResourceKeys.NodeEditorWireDefaultColor] = Color.White;

            dictionary[CommonResourceKeys.TextBlockDefaultColor] = Color.Black;

            dictionary[CommonResourceKeys.TextInputDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.TextInputFocusedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.TextInputDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.TextInputTextDefaultColor] = Color.Black;

            dictionary[CommonResourceKeys.TrackBarDefaultColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.TrackBarThumbDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.TrackBarThumbDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.TrackBarThumbHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.TrackBarThumbPressedColor] = Color.LightSkyBlue;

            dictionary[CommonResourceKeys.WindowBackgroundColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.WindowHeaderColor] = Color.White;
            dictionary[CommonResourceKeys.WindowTextColor] = Color.Black;
        }
    }
}
