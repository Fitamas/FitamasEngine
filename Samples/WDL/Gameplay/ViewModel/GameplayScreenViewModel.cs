using Fitamas.Core;
using Fitamas.MVVM;
using Fitamas.UserInterface.ViewModel;
using System;
using System.ComponentModel;
using WDL.DigitalLogic;
using WDL.Gameplay.ViewModel;

public class GameplayScreenViewModel : GUIWindowViewModel
{
    private GUIGameblayManager manager;

    public GameplayViewModel Gameplay { get; }

    public override GUIWindowType Type => GUIWindowTypes.GameplayScreen;

    public GameplayScreenViewModel(GUIGameblayManager manager)
    {
        this.manager = manager;
        Gameplay = manager.Container.Resolve<GameplayViewModel>();
    }

    public void OpenComponents()
    {
        manager.OpenComponents();
    }

    public void OpenSimulation()
    {
        OpenSimulation(new LogicComponentDescription());
    }

    public void OpenSimulation(LogicComponentDescription description)
    {
        manager.OpenSimulation(description);
    }

    public void OpenCreateDescriptionPopup()
    {
        manager.OpenCreateDescriptionPopup();
    }
}
