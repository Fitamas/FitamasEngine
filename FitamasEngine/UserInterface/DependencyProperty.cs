using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public abstract class DependencyProperty
    {
        protected static List<DependencyProperty> properties = new List<DependencyProperty>();
        protected static int count = 0;

        public int Id { get; }

        public abstract Type ValueType { get; }

        public ExpressionChangedCallback ExpressionChangedCallback { get; set; }
        public bool IsInherited { get; set; }
        public object DefaultValue { get; set; }

        public static int RegisteredPropertiesCount => properties.Count;

        protected DependencyProperty(ExpressionChangedCallback expressionChangedCallback, object defaultValue, bool isInherited)
        {
            DefaultValue = defaultValue;
            IsInherited = isInherited;
            ExpressionChangedCallback = expressionChangedCallback;

            Id = count;
            count++;
            properties.Add(this);
        }
    }

    public class DependencyProperty<T> : DependencyProperty
    {
        public new T DefaultValue => (T)base.DefaultValue;

        public PropertyChangedCallback<T> PropertyChangedCallback { get; set; }

        public override Type ValueType => typeof(T);

        public DependencyProperty(T defaultValue = default, bool isInherited = true) 
            : base(null, defaultValue, isInherited)
        {

        }

        public DependencyProperty(ExpressionChangedCallback expressionChangedCallback, T defaultValue = default, bool isInherited = true) 
            : base(expressionChangedCallback, defaultValue, isInherited)
        {

        }

        public DependencyProperty(PropertyChangedCallback<T> propertyChangedCallback, T defaultValue = default, bool isInherited = true) 
            : base(null, defaultValue, isInherited)
        {
            PropertyChangedCallback = propertyChangedCallback;
        }

        public DependencyProperty(PropertyChangedCallback<T> propertyChangedCallback, ExpressionChangedCallback expressionChangedCallback, T defaultValue = default, bool isInherited = true) 
            : base(expressionChangedCallback, defaultValue, isInherited)
        {
            PropertyChangedCallback = propertyChangedCallback;
        }
    }
}
