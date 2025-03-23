using ObservableCollections;
using R3;
using System;
using Fitamas.MVVM;
using Fitamas.Core;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIRootBinder : Binder<GUIRootViewModel>
    {
        private GUIWindowsContainer windowsContainer;
        private CompositeDisposable subscriptions;

        public GUIRootBinder(GUISystem system)
        {
            windowsContainer = new GUIWindowsContainer(system);
            subscriptions = new CompositeDisposable();
        }

        protected sealed override IDisposable OnBind(GUIRootViewModel viewModel)
        {
            subscriptions.Add(viewModel.OpenedScreen.Subscribe(newScreenViewModel =>
            {
                windowsContainer.OpenScreen(newScreenViewModel);
            }));

            foreach (var openedPopup in viewModel.OpenedPopups)
            {
                windowsContainer.OpenWindow(openedPopup);
            }

            subscriptions.Add(viewModel.OpenedPopups.ObserveAdd().Subscribe(e =>
            {
                windowsContainer.OpenWindow(e.Value);
            }));

            subscriptions.Add(viewModel.OpenedPopups.ObserveRemove().Subscribe(e =>
            {
                windowsContainer.CloseWindow(e.Value);
            }));

            return subscriptions;
        }
    }
}