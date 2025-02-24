/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Fitamas.Serialization.Json.Converters
{
    public static class ColorHelper
    {
        //http://stackoverflow.com/questions/2353211/hsl-to-rgb-color-conversion
        public static Color FromHsl(float hue, float saturation, float lightness)
        {
            var hsl = new Vector4(hue, saturation, lightness, 1);
            var color = new Vector4(0, 0, 0, hsl.W);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (hsl.Y == 0.0f)
                color.X = color.Y = color.Z = hsl.Z;
            else
            {
                var q = hsl.Z < 0.5f ? hsl.Z * (1.0f + hsl.Y) : hsl.Z + hsl.Y - hsl.Z * hsl.Y;
                var p = 2.0f * hsl.Z - q;

                color.X = HueToRgb(p, q, hsl.X + 1.0f / 3.0f);
                color.Y = HueToRgb(p, q, hsl.X);
                color.Z = HueToRgb(p, q, hsl.X - 1.0f / 3.0f);
            }

            return new Color(color);
        }

        private static float HueToRgb(float p, float q, float t)
        {
            if (t < 0.0f) t += 1.0f;
            if (t > 1.0f) t -= 1.0f;
            if (t < 1.0f / 6.0f) return p + (q - p) * 6.0f * t;
            if (t < 1.0f / 2.0f) return q;
            if (t < 2.0f / 3.0f) return p + (q - p) * (2.0f / 3.0f - t) * 6.0f;
            return p;
        }

        public static Color FromHex(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Color.Transparent;
            var startIndex = 0;
            if (value.StartsWith("#"))
                startIndex++;
            var r = int.Parse(value.Substring(startIndex, 2), NumberStyles.HexNumber);
            var g = int.Parse(value.Substring(startIndex + 2, 2), NumberStyles.HexNumber);
            var b = int.Parse(value.Substring(startIndex + 4, 2), NumberStyles.HexNumber);
            var a = value.Length > 6 + startIndex ? int.Parse(value.Substring(startIndex + 6, 2), NumberStyles.HexNumber) : 255;

            return new Color(r, g, b, a);
        }

        public static string ToHex(Color color)
        {
            var rx = $"{color.R:x2}";
            var gx = $"{color.G:x2}";
            var bx = $"{color.B:x2}";
            var ax = $"{color.A:x2}";
            return $"#{rx}{gx}{bx}{ax}";
        }

        private static readonly Dictionary<string, Color> _colorsByName = typeof(Color)
            .GetRuntimeProperties()
            .Where(p => p.PropertyType == typeof(Color))
            .ToDictionary(p => p.Name, p => (Color)p.GetValue(null), StringComparer.OrdinalIgnoreCase);

        public static Color FromName(string name)
        {
            Color color;

            if (_colorsByName.TryGetValue(name, out color))
                return color;

            throw new InvalidOperationException($"{name} is not a valid color");
        }
    }
}