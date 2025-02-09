using R3;
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
        public Binding Parent { get; }

        public override object Value => Parent.Value;

        public BindingExpression(DependencyProperty property, Binding binding) : base(property)
        {
            Parent = binding;
        }
    }

    public class ValueExpression<T> : Expression
    {
        public new DependencyProperty<T> Property { get; }
        public ReactiveProperty<T> ReactiveProperty { get; }

        public override object Value => ReactiveProperty.Value;

        public ValueExpression(DependencyProperty<T> property, T value) : base(property) 
        {
            Property = property;
            ReactiveProperty = new ReactiveProperty<T>(value);
        }
    }

    public class Binding
    {
        public object Value { get; set; }
    }
}
