﻿using Fitamas.Graphics;
using Fitamas.Scene;
using Fitamas.UserInterface.Serializeble;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.Serializeble
{
    public static class ExtensionUtils
    {
        public static Dictionary<string, Type> dictionary = new Dictionary<string, Type>()
        {
            { ".scene", typeof(SerializebleScene) },
            { ".spr", typeof(Sprite) },
            { ".mat", typeof(Matireal) },
            { ".prefab", typeof(Prefab) },
            { ".xml", typeof(SerializebleLayout) },
        };

        public static string[] GetAllExtensions()
        {
            return dictionary.Keys.ToArray();
        }

        public static Type GetType(string extension)
        {
            if (dictionary.TryGetValue(extension, out Type type))
            {
                return type;
            }

            return null;
        }
    }
}
