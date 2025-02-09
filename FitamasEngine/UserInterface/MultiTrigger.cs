using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public class MultiTrigger : TriggerBase
    {
        public List<TriggerCondition> Conditions { get; }

        public MultiTrigger(List<TriggerCondition> conditions)
        {
            Conditions = conditions;
        }

        internal override bool HasPropertyCondition(DependencyProperty property)
        {
            foreach (TriggerCondition condition in Conditions)
            {
                if (condition.Property == property)
                {
                    return true;
                }
            }

            return false;
        }

        internal override bool IsActive(GUIComponent element)
        {
            bool flag = true;
            foreach (TriggerCondition condition in Conditions)
            {
                if (!Equals(element.GetValue(condition.Property), condition.Value))
                {
                    flag = false;
                }
            }

            return flag;
        }

        internal override void CheckState(GUIComponent element)
        {
            if (IsActive(element))
            {
                ProcessSettersCollection(element);
            }
        }
    }
}
