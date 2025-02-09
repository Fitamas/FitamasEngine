using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public class TriggerCondition
    {
        public DependencyProperty Property { get; }

        public object Value { get; }

        public TriggerCondition(DependencyProperty property, object value)
        {
            Property = property;
            Value = value;
        }
    }
}
