using Fitamas.MVVM;
using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public interface IGUIWindowBinder
    {
        void Bind(GUIWindowViewModel viewModel);
        void Close();
    }
}
