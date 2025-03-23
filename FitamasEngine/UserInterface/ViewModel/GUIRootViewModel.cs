using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using R3;
using Fitamas.MVVM;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIRootViewModel : IDisposable, IViewModel
    {
        private ReactiveProperty<GUIWindowViewModel> openedScreen;
        private ObservableList<GUIWindowViewModel> openedPopups;
        private Dictionary<GUIWindowViewModel, IDisposable> popupSubscriptions;

        public ReadOnlyReactiveProperty<GUIWindowViewModel> OpenedScreen => openedScreen;
        public IObservableCollection<GUIWindowViewModel> OpenedPopups => openedPopups;

        public GUIRootViewModel()
        {
            openedScreen = new ReactiveProperty<GUIWindowViewModel>();
            openedPopups = new ObservableList<GUIWindowViewModel>();
            popupSubscriptions = new Dictionary<GUIWindowViewModel, IDisposable>();
        }

        public void OpenScreen(GUIWindowViewModel screenViewModel)
        {
            openedScreen.Value?.Dispose();
            openedScreen.Value = screenViewModel;
        }

        public void OpenPopup(GUIWindowViewModel popupViewModel)
        {
            if (openedPopups.Contains(popupViewModel))
            {
                return;
            }

            var subscription = popupViewModel.CloseRequested.Subscribe(ClosePopup);
            popupSubscriptions.Add(popupViewModel, subscription);
            openedPopups.Add(popupViewModel);
        }

        public void ClosePopup(GUIWindowViewModel popupViewModel)
        {
            if (openedPopups.Contains(popupViewModel))
            {
                popupViewModel.Dispose();
                openedPopups.Remove(popupViewModel);

                var popupSubscription = popupSubscriptions[popupViewModel];
                popupSubscription?.Dispose();
                popupSubscriptions.Remove(popupViewModel);
            }
        }

        public bool ContainPopup<T>() where T : GUIWindowViewModel
        {
            return openedPopups.FirstOrDefault(p => p.GetType() == typeof(T)) != null;
        }

        public void ClosePopup<T>() where T : GUIWindowViewModel
        {
            var openedPopupViewModel = openedPopups.FirstOrDefault(p => p.GetType() == typeof(T));
            ClosePopup(openedPopupViewModel);
        }

        public void CloseAllPopups()
        {
            foreach (var openedPopup in openedPopups.ToArray())
            {
                ClosePopup(openedPopup);
            }
        }

        public void CloseLastPopup()
        {
            ClosePopup(openedPopups.Last());
        }

        public void Dispose()
        {
            CloseAllPopups();
            openedScreen.Value?.Dispose();
        }
    }
}