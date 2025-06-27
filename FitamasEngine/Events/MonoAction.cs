using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Events
{
    public delegate void MonoAction();

    public delegate void MonoAction<T0>(T0 arg0);

    public delegate void MonoAction<T0, T1>(T0 arg0, T1 arg1);

    public delegate TResult MonoFunc<out TResult>();

    public delegate TResult MonoFunc<T0, out TResult>(T0 arg0);

    public delegate TResult MonoFunc<T0, T1, out TResult>(T0 arg0, T1 arg1);

}
