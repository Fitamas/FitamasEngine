using Fitamas.MVVM;
using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowBinder : Binder<GUIWindowViewModel>, IGUIWindowBinder
    {
        public GUIWindow Window { get; set; }

        public GUIWindowBinder()
        {

        }

        public void Bind(GUISystem system, GUIWindowViewModel viewModel)
        {
            Bind(viewModel);
            BindOverride(viewModel);
        }

        protected sealed override IDisposable OnBind(GUIWindowViewModel viewModel)
        {
            return viewModel;
        }

        public void Close()
        {
            Window?.Close();
            Dispose();
        }

        protected virtual void BindOverride(object viewModel) { }
    }
}
