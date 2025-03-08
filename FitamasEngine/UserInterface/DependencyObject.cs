using Newtonsoft.Json.Linq;
using R3;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface
{
    public delegate void PropertyChangedCallback<T>(DependencyObject dependencyObject, DependencyProperty<T> property, T oldValue, T newValue);

    public delegate void ExpressionChangedCallback(DependencyObject dependencyObject, DependencyProperty property, Expression oldExpression, Expression newExpression);

    public class DependencyObject
    {
        private Dictionary<int, Expression> expressionMap;

        internal DependencyObject InheritanceParent { get; set; }

        public bool IsSealed { get; internal set; }

        public DependencyObject()
        {
            expressionMap = new Dictionary<int, Expression>();
        }

        public void ClearValue<T>(DependencyProperty<T> property)
        {
            if (expressionMap.Remove(property.Id, out Expression expression))
            {
                expression.Dispose();

                OnPropertyChanged(property);

                T value = GetValue(property);

                property.PropertyChangedCallback?.Invoke(this, property, value, property.DefaultValue);
            }
        }

        public T GetValue<T>(DependencyProperty<T> property)
        {
            return (T)GetValue(property as DependencyProperty);
        }

        public object GetValue(DependencyProperty property)
        {
            if (TryReadLocalValue(property, out object value))
            {
                return value;
            }

            if (property.IsInherited && InheritanceParent != null)
            {
                return InheritanceParent.GetValue(property);
            }

            return property.DefaultValue;
        }

        public bool TryReadLocalValue(DependencyProperty property, out object value)
        {
            value = default;

            if (expressionMap.TryGetValue(property.Id, out Expression expression))
            {
                value = expression.Value;
                return true;
            }

            return false;
        }

        public DependencyObject SetValue<T>(DependencyProperty<T> property, T value)
        {
            if (IsSealed)
            {
                throw new InvalidOperationException("You can't change sealed instance.");
            }

            SetValueInternal(property, value);

            return this;
        }

        internal void SetValueInternal<T>(DependencyProperty<T> property, T value)
        {
            if (expressionMap.TryGetValue(property.Id, out Expression expression))
            {
                if (expression is ValueExpression<T> valueExpression)
                {
                    if (!valueExpression.IsFrozen)
                    {
                        T oldValue = valueExpression.CurrentValue;
                        if (!oldValue.Equals(value))
                        {
                            valueExpression.CurrentValue = value;
                            property.PropertyChangedCallback?.Invoke(this, property, oldValue, value);
                            OnPropertyChanged(property);
                        }
                        return;
                    }
                }
            }

            ValueExpression<T> valueExpression1 = new ValueExpression<T>(property, value);
            SetExpression(property, valueExpression1);
            property.PropertyChangedCallback?.Invoke(this, property, property.DefaultValue, value);
            OnPropertyChanged(property);
        }

        public virtual void OnPropertyChanged<T>(DependencyProperty<T> property) { }

        public bool ContainsProperty(DependencyProperty property)
        {
            return expressionMap.ContainsKey(property.Id);
        }

        public Expression GetExpression(DependencyProperty property)
        {
            if (expressionMap.TryGetValue(property.Id, out Expression expression))
            {
                return expression;
            }

            return null;
        }

        public void SetExpression(DependencyProperty property, Expression expression)
        {
            expressionMap.TryGetValue(property.Id, out Expression oldValue);

            if (expression == null)
            {
                expressionMap.Remove(property.Id);
            }
            else
            {
                expressionMap[property.Id] = expression;
            }

            property.ExpressionChangedCallback?.Invoke(this, property, oldValue, expression);
        }

        public void SetResourceReference(DependencyProperty property, ResourceDictionary dictionary, string resourceKey)
        {
            SetExpression(property, new ResourceReferenceExpression(property, dictionary, resourceKey));
        }

        public void SetBinding(DependencyProperty property, Binding binding)
        {
            SetExpression(property, new BindingExpression(property, binding));
        }
    }
}
