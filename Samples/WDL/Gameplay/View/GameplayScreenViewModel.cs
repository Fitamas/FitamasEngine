using Fitamas.Events;
using Fitamas.UserInterface.ViewModel;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;
using WDL.Gameplay.View;

public class GameplayScreenViewModel : GUIWindowViewModel
{
    private GUIGameblayManager manager;
    private GameplayViewModel gameplay;
    private ReactiveProperty<LogicSimulationWindowViewModel> simulation;

    public MonoAction<LogicComponentDescription> OnSelectComponent;

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

    public void OpenDescription()
    {
        OpenDescription(Simulation.CurrentValue.Description);
    }

    public void OpenDescription(LogicComponentDescription description)
    {
        if (description.IsBase)
        {
            return;
        }

        manager.OpenCreateDescription(description);
    }

    public LogicComponentViewModel CreateComponent(LogicComponentDescription description)
    {
        return gameplay.CreateComponent(description);
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

    public void SelectComponent(LogicComponentDescription description)
    {
        OnSelectComponent.Invoke(description);
    }

    public void Import(IEnumerable<string> paths)
    {
        gameplay.Import(paths);
    }
}
