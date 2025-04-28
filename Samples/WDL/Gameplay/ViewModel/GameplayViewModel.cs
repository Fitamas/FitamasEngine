using Fitamas;
using Fitamas.MVVM;
using ObservableCollections;
using R3;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class GameplayViewModel : IViewModel
    {
        private LogicSystem logicSystem;
        private ReactiveProperty<LogicSimulationViewModel> simulation;

        public IObservableCollection<LogicComponentDescription> ComponentDescriptions => logicSystem.Manager.Components;
        public ReadOnlyReactiveProperty<LogicSimulationViewModel> Simulation => simulation;

        public GameplayViewModel(LogicSystem logicSystem)
        {
            this.logicSystem = logicSystem;
            simulation = new ReactiveProperty<LogicSimulationViewModel>();
            if (logicSystem.Simulation.Value != null)
            {
                simulation.Value = new LogicSimulationViewModel(logicSystem.Simulation.Value);
            }
            logicSystem.Simulation.Subscribe(s =>
            {
                if (s != null)
                {
                    simulation.Value = new LogicSimulationViewModel(s);
                }
                else
                {
                    simulation.Value = null;
                }
            });
        }

        public void CreateSimulation(LogicComponentDescription description)
        {
            logicSystem.CreateSimulation(description);
        }

        public void SaveProject()
        {
            logicSystem.Manager.SaveComponents();
        }

        public bool IsSavedCurrentComponent()
        {
            if (simulation.Value == null)
            {
                return false;
            }

            LogicComponentDescription description = simulation.Value.Description;
            return logicSystem.Manager.ContainComponent(description);
        }

        public bool Contain(string fullname)
        {
            return logicSystem.Manager.ContainComponent(fullname);
        }

        public void SaveComponent(LogicComponentDescription description)
        {
            logicSystem.Manager.AddComponent(description);
        }

        public void Remove(LogicComponentDescription description)
        {
            logicSystem.Manager.Remove(description);
        }
    }
}
