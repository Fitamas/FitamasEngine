using System.Collections.Generic;
using Fitamas.Core;
namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowsContainer
    {
        private Dictionary<GUIWindowViewModel, IGUIWindowBinder> openedWindowBinders;
        private IGUIWindowBinder openedScreenBinder;
        private IGUIWindowBinder openedPopupBinder;
        private GUISystem system;

        public GUIWindowsContainer(GUISystem system)
        {
            openedWindowBinders = new Dictionary<GUIWindowViewModel, IGUIWindowBinder>();
            this.system = system;
        }

        public void OpenWindow(GUIWindowViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            GUIWindowBinder binder = viewModel.Type.Create();
            binder.Bind(system, viewModel);
            openedWindowBinders.Add(viewModel, binder);
            system.Root.OpenWindow(binder.Window);
        }

        public void CloseWindow(GUIWindowViewModel popupViewModel)
        {
            if (openedWindowBinders.Remove(popupViewModel, out IGUIWindowBinder binder))
            {
                binder.Close();
            }
        }

        public void OpenScreen(GUIWindowViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            CloseScreen();

            GUIWindowBinder binder = viewModel.Type.Create();
            binder.Bind(system, viewModel);
            openedScreenBinder = binder;
            system.Root.MainWindow = binder.Window;
        }

        public void CloseScreen()
        {
            openedScreenBinder?.Close();
        }

        public void OpenPopup(GUIWindowViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

            ClosePopup();
            GUIWindowBinder binder = viewModel.Type.Create();
            binder.Bind(system, viewModel);
            openedPopupBinder = binder;
            system.Root.OpenPopup(binder.Window);
        }

        public void ClosePopup()
        {
            openedPopupBinder?.Close();
        }
    }
}