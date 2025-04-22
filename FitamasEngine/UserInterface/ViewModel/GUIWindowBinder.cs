using Fitamas.MVVM;
using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowBinder<T> : GUIWindow, IGUIWindowBinder where T : GUIWindowViewModel
    {
        private IDisposable disposable;

        protected T ViewModel;

        public GUIWindowBinder()
        {

        }

        public void Bind(GUIWindowViewModel viewModel)
        {
            ViewModel = (T)viewModel;
            disposable = OnBind(ViewModel);
        }

        protected virtual IDisposable OnBind(T viewModel)
        {
            return null;
        }

        public override void OnClose()
        {
            ViewModel?.RequestClose();
            disposable?.Dispose();
        }
    }
}
