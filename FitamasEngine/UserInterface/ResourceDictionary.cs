using Fitamas.UserInterface.Themes;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public class ResourceDictionary : Dictionary<string, object>
    {
        public static readonly ResourceDictionary DefaultResources;

        static ResourceDictionary()
        {
            DefaultResources = new ResourceDictionary();
            GUILightTheme.CreateColors(DefaultResources);
            CommonHelpers.CreateStyles(DefaultResources);
        }
    }
}