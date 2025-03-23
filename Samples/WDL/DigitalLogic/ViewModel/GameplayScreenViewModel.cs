using Fitamas.UserInterface.ViewModel;
using System;
using WDL.DigitalLogic.ViewModel;

public class GameplayScreenViewModel : GUIWindowViewModel
{
    public GUIGameblayManager Manager { get; }
    public GameplayViewModel Gameplay { get; }

    public override GUIWindowType Type => GUIWindowTypes.GameplayScreen;//"Layouts\\MainMenu.xml";

    public GameplayScreenViewModel(GUIGameblayManager manager)
    {
        Manager = manager;
        Gameplay = manager.Container.Resolve<GameplayViewModel>();
    }

    public void OpenMenu()
    {

    }

    public void OpenDebugMenu()
    {

    }

    public void OpenMainMenu()
    {

    }
}
