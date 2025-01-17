using System;

namespace Fitamas.MVVM
{
    public abstract class Binder<T> : IDisposable where T : IViewModel
    {
        private IDisposable disposable;

        public void Bind(T viewModel)
        {
            Dispose();

            disposable = OnBind(viewModel);
        }

        protected abstract IDisposable OnBind(T viewModel);

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}
