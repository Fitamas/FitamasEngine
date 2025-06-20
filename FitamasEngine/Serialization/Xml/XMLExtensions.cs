using Fitamas.Serialization.Json.Converters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Fitamas.Serialization.Xml
{
    public static class XMLExtensions
    {
        private static readonly Dictionary<Type, Func<string, object>> stringParsers = new Dictionary<Type, Func<string, object>>
        {
            {typeof(int), s => int.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
            {typeof(float), s => float.Parse(s, CultureInfo.InvariantCulture.NumberFormat)},
            {typeof(bool), s => bool.Parse(s)},
        };

        public static string GetAttributeString(this XElement element, string name, string defaultValue = "")
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return string.IsNullOrEmpty(attribute.Value) ? defaultValue : attribute.Value;
        }

        public static int GetAttributeInt(this XElement element, string name, int defaultValue = 0)
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Parse<int>();
        }

        public static float GetAttributeFloat(this XElement element, string name, float defaultValue = 0)
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Parse<float>();
        }

        public static T GetAttributeEnum<T>(this XElement element, string name, T defaultValue = default) where T : struct, Enum
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            if (Enum.TryParse(attribute.Value, true, out T result))
            {
                return result;
            }
            else if (int.TryParse(attribute.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out int resultInt))
            {
                return Unsafe.As<int, T>(ref resultInt);
            }

            return defaultValue;
        }

        public static bool GetAttributeBool(this XElement element, string name, bool defaultValue = false)
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.Parse<bool>();
        }

        public static Point GetAttributePoint(this XElement element, string name, Point defaultValue = new Point())
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.ParseToPoint();
        }

        public static Vector2 GetAttributeVector2(this XElement element, string name, Vector2 defaultValue = new Vector2())
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.ParseToVector2();
        }

        public static Color GetAttributeColor(this XElement element, string name, Color defaultValue = new Color())
        {
            XAttribute attribute = element?.Attribute(name);

            if (attribute == null)
            {
                return defaultValue;
            }

            return attribute.ParseToColor();
        }

        public static T GetAttributeMonoObject<T>(this XElement element, string name) where T : MonoContentObject
        {
            string path = element.GetAttributeString(name);
            if (!string.IsNullOrEmpty(path))
            {
                return Resources.Load<T>(path);
            }

            return null;
        }

        private static Point ParseToPoint(this XAttribute attribute)
        {
            Point point = new Point();

            int[] values = attribute.ReadAsDelimitedString<int>();

            if (values.Length > 0)
            {
                point.X = values[0];
            }
            if (values.Length > 1)
            {
                point.Y = values[1];
            }

            return point;
        }

        private static Vector2 ParseToVector2(this XAttribute attribute)
        {
            Vector2 point = new Vector2();

            float[] values = attribute.ReadAsDelimitedString<float>();

            if (values.Length > 0)
            {
                point.X = values[0];
            }
            if (values.Length > 1)
            {
                point.Y = values[1];
            }

            return point;
        }

        private static Color ParseToColor(this XAttribute attribute)
        {
            string value = attribute.Value;

            return value.StartsWith("#") ? ColorHelper.FromHex(value) : ColorHelper.FromName(value);
        }

        private static T[] ReadAsDelimitedString<T>(this XAttribute attribute)
        {
            string value = attribute.Value;
            var parser = stringParsers[typeof(T)];
            return value.Split(' ')
                .Select(i => parser(i))
                .Cast<T>()
                .ToArray();
        }

        private static T Parse<T>(this XAttribute attribute)
        {
            string value = attribute.Value;
            var parser = stringParsers[typeof(T)];

            return (T)parser(value);
        }
    }
}
