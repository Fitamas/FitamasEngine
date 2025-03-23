using ObservableCollections;
using System;

namespace WDL.DigitalLogic.ViewModel
{
    public class GameplayViewModel
    {
        public IObservableCollection<LogicComponentDescription> ComponentDescriptions { get; }

        public GameplayViewModel(LogicSystem logicSystem)
        {
            ComponentDescriptions = logicSystem.Manager.Components;
        }
    }
}
