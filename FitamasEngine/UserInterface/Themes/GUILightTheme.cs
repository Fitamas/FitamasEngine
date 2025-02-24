﻿using Microsoft.Xna.Framework;
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

            dictionary[CommonResourceKeys.ItemDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.ItemHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.ItemPressedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.ItemDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.ItemTextDefaultColor] = Color.Black;
            dictionary[CommonResourceKeys.ItemTextHoverColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextPressedColor] = Color.White;
            dictionary[CommonResourceKeys.ItemTextDisableColor] = Color.Black;

            dictionary[CommonResourceKeys.TextBlockDefaultColor] = Color.Black;

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

            dictionary[CommonResourceKeys.ContextMenuDefaultColor] = Color.LightGray;
        }
    }
}
