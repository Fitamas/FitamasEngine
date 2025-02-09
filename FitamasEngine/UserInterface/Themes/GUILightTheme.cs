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

            dictionary[CommonResourceKeys.TextBlockDefaultColor] = Color.Black;

            dictionary[CommonResourceKeys.CheckBoxDefaultColor] = Color.White;
            dictionary[CommonResourceKeys.CheckBoxHoverColor] = Color.DeepSkyBlue;
            dictionary[CommonResourceKeys.CheckBoxPressedColor] = Color.LightSkyBlue;
            dictionary[CommonResourceKeys.CheckBoxDisableColor] = new Color(0.75f, 0.75f, 0.75f);
            dictionary[CommonResourceKeys.CheckBoxMarkColor] = Color.SkyBlue;
        }
    }
}
