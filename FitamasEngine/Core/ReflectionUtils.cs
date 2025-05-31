using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fitamas.Core
{
    public static class ReflectionUtils
    {
        public static Type[] GetTypesAssignableFrom<T>() where T : class
        {
            Type[] types = Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract).ToArray();

            return types;
        }

        public static Type[] GetTypes<T>() where T : Attribute
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(t => t.IsDefined(typeof(T)))).ToArray();

            return types;
        }

        public static FieldInfo[] GetSerializedFields(this Type type) //TODO fix loop
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                                 BindingFlags.Instance | BindingFlags.DeclaredOnly;

            FieldInfo[] fields = type.GetFields(flags);

            if (type.IsArray ||
                typeof(IEnumerable).IsAssignableFrom(type) ||
                type == typeof(Vector2) ||
                type == typeof(Vector3) ||
                type == typeof(Vector4) ||
                type == typeof(Color))
            {
                return fields;
            }

            List<FieldInfo> result = new List<FieldInfo>();

            if (type.BaseType != null)
            {
                result.AddRange(type.BaseType.GetSerializedFields());
            }
            
            foreach (FieldInfo field in fields)
            {
                if ((field.IsPublic && field.GetCustomAttribute(typeof(NonSerializedAttribute)) == null)
                || (!field.IsPublic && field.GetCustomAttribute(typeof(SerializeFieldAttribute)) != null))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }
    }
}
