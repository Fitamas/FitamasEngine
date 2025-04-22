using System.Collections.Generic;
using Fitamas.UserInterface.Components;
namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowsContainer
    {
        private Dictionary<GUIWindowViewModel, IGUIWindowBinder> openedWindowBinders;
        private IGUIWindowBinder openedScreenBinder;
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

            IGUIWindowBinder binder = viewModel.Type.Create();
            binder.Bind(viewModel);
            openedWindowBinders.Add(viewModel, binder);
            system.Root.OpenWindow(binder as GUIWindow);
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

            IGUIWindowBinder binder = viewModel.Type.Create();
            binder.Bind(viewModel);
            openedScreenBinder = binder;
            system.Root.Screen = binder as GUIWindow;
        }

        public void CloseScreen()
        {
            openedScreenBinder?.Close();
        }
    }
}