using Fitamas.Core;
using System;
using System.Reflection;

namespace Fitamas.Serialization
{
    public abstract class MonoObject
    {
        public Guid Guid { get; set; }

        public MonoObject()
        {
            Guid = Guid.NewGuid();
        }

        public static MonoObject Instantiate(MonoObject original)
        {
            Type type = original.GetType();
            MonoObject instance = (MonoObject)Activator.CreateInstance(type);

            FieldInfo[] fields = type.GetSerializedFields();

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(original);
                field.SetValue(instance, value);
            }

            return instance;
        }

        private static bool CompareBaseObject(MonoObject a, MonoObject b)
        {
            bool flag1 = (object)a == null;
            bool flag2 = (object)b == null;

            if (flag1 && flag2)
            {
                return true;
            }
            else if (flag1 && !flag2)
            {
                return false;
            }
            else if (!flag1 && flag2)
            {
                return false;
            }

            return a.Guid == b.Guid;
        }

        public static bool operator ==(MonoObject a, MonoObject b)
        {
            return CompareBaseObject(a, b);
        }

        public static bool operator !=(MonoObject a, MonoObject b)
        {
            return !CompareBaseObject(a, b);
        }

        public override bool Equals(object other)
        {
            if (other != null && other is MonoObject obj)
            {
                return CompareBaseObject(this, obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
