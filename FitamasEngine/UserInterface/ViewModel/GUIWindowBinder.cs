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
            viewModel.IsFocus.Value = IsFocused;
        }

        protected virtual IDisposable OnBind(T viewModel)
        {
            return null;
        }

        protected override void OnFocus()
        {
            base.OnFocus();
            ViewModel.IsFocus.Value = true;
        }

        protected override void OnUnfocus()
        {
            base.OnUnfocus();
            ViewModel.IsFocus.Value = false;
        }

        protected override void OnCloseWindow()
        {
            ViewModel?.RequestClose();
            disposable?.Dispose();
        }
    }
}
