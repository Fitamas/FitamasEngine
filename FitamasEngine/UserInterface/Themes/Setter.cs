using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public class Setter
    {
        public string TargetName { get; set; }
        public Expression Expression { get; set; }

        public DependencyProperty Property => Expression.Property;

        public Setter(Expression expression, string targetName = "")
        {
            Expression = expression;
            TargetName = targetName;
        }
    }
}
