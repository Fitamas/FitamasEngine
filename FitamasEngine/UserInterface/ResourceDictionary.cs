﻿using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
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
            GUICommonHelpers.CreateStyles(DefaultResources);
            GUICommonHelpers.CreateVars(DefaultResources);
        }

        public Point WindowPadding => (Point)this[CommonResourceKeys.WindowPadding];

        public Point FramePadding => (Point)this[CommonResourceKeys.FramePadding];

        public int ScrollbarSize => (int)this[CommonResourceKeys.ScrollbarSize];
    }
}