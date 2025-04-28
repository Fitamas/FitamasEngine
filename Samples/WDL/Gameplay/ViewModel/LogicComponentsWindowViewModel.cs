using Fitamas.UserInterface.ViewModel;
using ObservableCollections;
using R3;
using System;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class LogicComponentsWindowViewModel : GUIWindowViewModel
    {
        private GameplayScreenViewModel gameplay;

        public override GUIWindowType Type => GUIWindowTypes.LogicComponents;
        public IObservableCollection<LogicComponentDescription> ComponentDescriptions => gameplay.ComponentDescriptions;
        public ReadOnlyReactiveProperty<LogicSimulationWindowViewModel> Simulation => gameplay.Simulation;

        public LogicComponentsWindowViewModel(GameplayScreenViewModel gameplay)
        {
            this.gameplay = gameplay;
        }

        public bool IsSavedCurrentComponent()
        {
            return gameplay.IsSavedCurrentComponent();
        }

        public void Remove(LogicComponentDescription description)
        {
            gameplay.Remove(description);
        }

        public void CreateSimulation(LogicComponentDescription description)
        {
            gameplay.CreateSimulation(description);
        }

        public void OpenDescription(LogicComponentDescription description)
        {
            gameplay.OpenDescription(description);
        }
    }
}
