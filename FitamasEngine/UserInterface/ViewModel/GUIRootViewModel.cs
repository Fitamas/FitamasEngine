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
        private ObservableList<GUIWindowViewModel> openedWindows;
        private ReactiveProperty<GUIWindowViewModel> openedScreen;
        private Dictionary<GUIWindowViewModel, IDisposable> windowSubscriptions;

        public IObservableCollection<GUIWindowViewModel> OpenedWindows => openedWindows;
        public ReadOnlyReactiveProperty<GUIWindowViewModel> OpenedScreen => openedScreen;

        public GUIRootViewModel()
        {
            openedWindows = new ObservableList<GUIWindowViewModel>();
            openedScreen = new ReactiveProperty<GUIWindowViewModel>();

            windowSubscriptions = new Dictionary<GUIWindowViewModel, IDisposable>();
        }

        public void OpenScreen(GUIWindowViewModel screenViewModel)
        {
            openedScreen.Value?.Dispose();
            openedScreen.Value = screenViewModel;
            windowSubscriptions[screenViewModel] = screenViewModel.CloseRequested.Subscribe(Close);
        }

        public void OpenWindow(GUIWindowViewModel windowViewModel)
        {
            if (openedWindows.Contains(windowViewModel))
            {
                return;
            }

            var subscription = windowViewModel.CloseRequested.Subscribe(Close);
            windowSubscriptions.Add(windowViewModel, subscription);
            openedWindows.Add(windowViewModel);
        }

        public void Close(GUIWindowViewModel windowViewModel)
        {
            if (openedScreen.Value == windowViewModel)
            {
                windowViewModel.Dispose();
            }

            if (openedWindows.Remove(windowViewModel))
            {
                windowViewModel.Dispose();
            }

            if (windowSubscriptions.Remove(windowViewModel, out var subscription))
            {
                subscription.Dispose();
            }
        }

        public bool ContainWindow<T>() where T : GUIWindowViewModel
        {
            return openedWindows.FirstOrDefault(p => p.GetType() == typeof(T)) != null;
        }

        public void CloseWindow<T>() where T : GUIWindowViewModel
        {
            var openedPopupViewModel = openedWindows.FirstOrDefault(p => p.GetType() == typeof(T));
            Close(openedPopupViewModel);
        }

        public void CloseAllWindows()
        {
            foreach (var openedPopup in openedWindows.ToArray())
            {
                Close(openedPopup);
            }
        }

        public void CloseLastWindow()
        {
            Close(openedWindows.Last());
        }

        public void Dispose()
        {
            CloseAllWindows();
            openedScreen.Value?.Dispose();
        }
    }
}