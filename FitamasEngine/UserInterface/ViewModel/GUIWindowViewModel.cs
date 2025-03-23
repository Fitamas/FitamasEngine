using System;
using Fitamas.MVVM;
using R3;

namespace Fitamas.UserInterface.ViewModel
{
    public abstract class GUIWindowViewModel : IDisposable, IViewModel
    {
        private Subject<GUIWindowViewModel> closeRequested;

        public abstract GUIWindowType Type { get; }

        public Observable<GUIWindowViewModel> CloseRequested => closeRequested;

        public GUIWindowViewModel()
        {
            closeRequested = new Subject<GUIWindowViewModel>();
        }

        public void RequestClose()
        {
            closeRequested.OnNext(this);
        }

        public virtual void Dispose() { }
    }
}