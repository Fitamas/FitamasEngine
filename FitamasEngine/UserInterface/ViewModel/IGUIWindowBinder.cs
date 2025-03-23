using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public interface IGUIWindowBinder
    {
        GUIWindow Window { get; }
        void Bind(GUISystem system, GUIWindowViewModel viewModel);
        void Close();
    }
}
