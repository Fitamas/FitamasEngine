using System;

namespace Fitamas.MVVM
{
    public interface IBinder<T> : IDisposable where T : IViewModel
    {
        void Bind(T viewModel);
    }
}
