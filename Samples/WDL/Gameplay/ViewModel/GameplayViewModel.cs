using Fitamas;
using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class GameplayViewModel : IViewModel
    {
        private LogicSystem logicSystem;
        private ReactiveProperty<LogicSimulationWindowViewModel> simulation;

        public IObservableCollection<LogicComponentDescription> ComponentDescriptions => logicSystem.Manager.Components;
        public ReadOnlyReactiveProperty<LogicSimulationWindowViewModel> Simulation => simulation;

        public GameplayViewModel(LogicSystem logicSystem)
        {
            this.logicSystem = logicSystem;
            simulation = new ReactiveProperty<LogicSimulationWindowViewModel>();
            if (logicSystem.Simulation.Value != null)
            {
                simulation.Value = new LogicSimulationWindowViewModel(logicSystem.Simulation.Value);
            }
            logicSystem.Simulation.Subscribe(s => 
            { 
                if (s != null)
                {
                    simulation.Value = new LogicSimulationWindowViewModel(s);
                }
                else
                {
                    simulation.Value = null;
                }
            });
        }

        public LogicSimulationWindowViewModel CreateSimulation(LogicComponentDescription description)
        {
            logicSystem.CreateSimulation(description);
            return Simulation.CurrentValue;
        }

        public void SaveProject()
        {
            logicSystem.Manager.SaveComponents();
        }

        public bool IsSaveed()
        {
            LogicComponentDescription description = logicSystem.Simulation.Value.Description;
            return logicSystem.Manager.ContainComponent(description);
        }

        public bool TrySaveComponent()
        {
            LogicComponentDescription description = logicSystem.Simulation.Value.Description;
            if (!logicSystem.Manager.ContainComponent(description))
            {
                logicSystem.Manager.AddComponent(description);
                return true;
            }

            return false;
        }

        public void Remove(LogicComponentDescription description)
        {
            logicSystem.Manager.Remove(description);
        }
    }
}
