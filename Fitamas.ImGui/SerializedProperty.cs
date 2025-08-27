using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.Math;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fitamas.ImGuiNet
{
    public class SerializedProperty
    {
        private FieldInfo field;
        private object value;
        private Type type;
        private object property;
        private SerializedProperty[] childPropery;

        public object Value => value;
        public Type Type => type;
        public SerializedProperty[] ChildProperty => childPropery;
        public bool IsNull => value == null;

        public bool IsInt => type == typeof(int);
        public bool IsBool => type == typeof(bool);
        public bool IsFloat => type == typeof(float);
        public bool IsDouble => type == typeof(double);
        public bool IsString => type == typeof(string);
        public bool IsVector2 => type == typeof(Vector2);
        public bool IsVector3 => type == typeof(Vector3);
        public bool IsColor => type == typeof(Color);
        public bool IsRectangleF => type == typeof(RectangleF);
        public bool IsMonoObject => type.IsSubclassOf(typeof(MonoObject));
        public bool IsTexture2D => type == typeof(Texture2D);
        public bool IsEnum => type.IsEnum;
        public bool IsArray => type.IsArray;

        public string Name
        {
            get
            {
                if (field != null)
                {
                    return field.Name;
                }

                return type.Name;
            }
        }

        public SerializedProperty(FieldInfo field, object property)
        {
            this.field = field;
            this.type = field.FieldType;
            this.property = property;

            this.value = field.GetValue(property);

            if (!IsArray)
            {
                childPropery = GetChildProperty(value);
            }
            else
            {
                childPropery = GetArrayProperties(this);
            }
        }

        public SerializedProperty(Type type, object value)
        {
            this.type = type;
            this.value = value;

            if (!IsArray)
            {
                childPropery = GetChildProperty(value);
            }
            else
            {
                childPropery = GetArrayProperties(this);
            }
        }

        public void SetValue(object value)
        {
            this.value = value;
            if (field != null)
            {
                field.SetValue(property, value);
            }
        }

        public static SerializedProperty[] GetChildProperty(object component)
        {
            if (component == null)
            {
                return null;
            }

            FieldInfo[] fields = component.GetType().GetSerializedFields();
            List<SerializedProperty> properties = new List<SerializedProperty>();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].GetCustomAttribute<HideInInpectorAttribute>() == null)
                {
                    properties.Add(new SerializedProperty(fields[i], component));
                }

            }

            return properties.ToArray();
        }

        public static SerializedProperty[] GetArrayProperties(SerializedProperty property)
        {
            SerializedProperty[] properties = new SerializedProperty[property.ArraySize];

            if (property.IsArray && property.value != null)
            {
                int i = 0;
                IEnumerator enumerator = ((IEnumerable)property.value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    properties[i] = new SerializedProperty(property.ArrayElementType, enumerator.Current);
                    i++;
                }
            }

            return properties;
        }

        public Type ArrayElementType => type.GetElementType();

        public int ArraySize
        {
            get
            {
                if (value != null && value is ICollection collection)
                {
                    return collection.Count;
                }
                return 0;
            }
        }

        public SerializedProperty GetArrayElementAtIndex(int index)
        {
            if (IsArray)
            {
                return childPropery[index];
            }

            return null;
        }

        public void SetArrayElementIndex(int index, object ellementValue)
        {
            if (IsArray && value is IList list)
            {
                list[index] = ellementValue;
            }
        }

        public void AddNewElement()
        {
            if (IsArray && value is IList list)
            {
                object newObject;
                if (ArrayElementType.IsArray)
                {
                    newObject = Activator.CreateInstance(ArrayElementType, 0);
                }
                else
                {
                    newObject = Activator.CreateInstance(ArrayElementType);
                }


                if (list.IsFixedSize && value is Array array)
                {
                    Array result = Array.CreateInstance(ArrayElementType, list.Count + 1);
                    array.CopyTo(result, 0);
                    result.SetValue(newObject, result.Length - 1);

                    SetValue(result);
                }
                else
                {
                    list.Add(newObject);
                }

                childPropery = GetArrayProperties(this);
            }
        }

        public void DeleteArrayElementAtIndex(int index)
        {
            if (IsArray && value is IList list && list.Count > 0)
            {
                if (list.IsFixedSize && value is Array array)
                {
                    Array result = Array.CreateInstance(ArrayElementType, list.Count - 1);

                    for (int i = 0; i < result.Length; i++)
                    {
                        result.SetValue(array.GetValue(i), i);
                    }

                    SetValue(result);
                }
                else
                {
                    list.RemoveAt(index);
                }

                childPropery = GetArrayProperties(this);
            }
        }
    }
}
