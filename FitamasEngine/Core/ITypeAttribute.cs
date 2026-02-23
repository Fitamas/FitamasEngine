using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Core
{
    public interface ITypeAttribute
    {
        public Type TargetType { get; set; }
    }
}
