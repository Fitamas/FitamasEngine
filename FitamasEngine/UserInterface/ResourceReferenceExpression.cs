using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public abstract class Expression : IDisposable
    {
        public DependencyProperty Property { get; }
        public abstract object Value { get; }
        
        public Expression(DependencyProperty property)
        {
            Property = property;
        }

        public virtual void Dispose()
        {
            
        }
    }

    public class ResourceReferenceExpression : Expression
    {
        public ResourceDictionary ResourceDictionary { get; }
        public string Key { get; }

        public override object Value
        {
            get
            {
                if (ResourceDictionary.TryGetValue(Key, out var value))
                {
                    return value;
                }

                return Property.DefaultValue;
            }
        }

        public ResourceReferenceExpression(DependencyProperty property, ResourceDictionary dictionary, string key) : base(property)
        {
            ResourceDictionary = dictionary;
            Key = key;
        }
    }

    public class BindingExpression : Expression
    {
        public DependencyObject DependencyObject { get; }

        public override object Value => DependencyObject.GetValue(Property);

        public BindingExpression(DependencyProperty property, DependencyObject dependencyObject) : base(property)
        {
            DependencyObject = dependencyObject;
        }
    }

    public class ValueExpression<T> : Expression
    {
        private T value;

        public new DependencyProperty<T> Property { get; }
        public T CurrentValue 
        { 
            get
            {
                return value;
            }
            set
            {
                if (!IsFrozen)
                {
                    this.value = value;
                }
            }
        }
        public bool IsFrozen { get; set; }

        public override object Value => CurrentValue;

        public ValueExpression(DependencyProperty<T> property, T value, bool isFrozen = false) : base(property) 
        {
            Property = property;
            CurrentValue = value;
            IsFrozen = isFrozen;
        }
    }

    //public class Binding
    //{
    //    public object Value { get; set; }
    //}
}
