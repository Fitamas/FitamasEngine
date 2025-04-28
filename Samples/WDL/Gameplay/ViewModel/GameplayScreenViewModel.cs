using Fitamas.UserInterface.ViewModel;
using System;
using System.ComponentModel;
using WDL.DigitalLogic;
using WDL.Gameplay.ViewModel;
using R3;
using ObservableCollections;

public class GameplayScreenViewModel : GUIWindowViewModel
{
    private GUIGameblayManager manager;
    private GameplayViewModel gameplay;
    private ReactiveProperty<LogicSimulationWindowViewModel> simulation;

    public override GUIWindowType Type => GUIWindowTypes.GameplayScreen;
    public IObservableCollection<LogicComponentDescription> ComponentDescriptions => gameplay.ComponentDescriptions;
    public ReadOnlyReactiveProperty<LogicSimulationWindowViewModel> Simulation => simulation;

    public GameplayScreenViewModel(GUIGameblayManager manager)
    {
        this.manager = manager;
        gameplay = manager.Container.Resolve<GameplayViewModel>();
        simulation = new ReactiveProperty<LogicSimulationWindowViewModel>();
        if (gameplay.Simulation.CurrentValue != null)
        {
            simulation.Value = manager.OpenSimulation(gameplay.Simulation.CurrentValue);
        }
        gameplay.Simulation.Subscribe(s =>
        {
            simulation.Value?.RequestClose();

            if (s != null)
            {
                simulation.Value = manager.OpenSimulation(gameplay.Simulation.CurrentValue);
            }
            else
            {
                simulation.Value = null;
            }
        });
    }

    public void OpenComponents()
    {
        manager.OpenComponents();
    }

    public void OpenSimulation(LogicSimulationViewModel viewModel)
    {
        manager.OpenSimulation(viewModel);
    }

    public void OpenDescription(LogicComponentDescription description)
    {
        if (description.IsBase)
        {
            return;
        }

        manager.OpenCreateDescription(description);
    }

    public void CreateSimulation()
    {
        gameplay.CreateSimulation(new LogicComponentDescription());
    }

    public void CreateSimulation(LogicComponentDescription description)
    {
        gameplay.CreateSimulation(description);
    }

    public void Remove(LogicComponentDescription description)
    {
        gameplay.Remove(description);
    }

    public bool IsSavedCurrentComponent()
    {
        return gameplay.IsSavedCurrentComponent();
    }

    public void SaveProject()
    {
        gameplay.SaveProject();
    }
}
