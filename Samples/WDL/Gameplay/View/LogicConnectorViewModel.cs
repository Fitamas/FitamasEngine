using Fitamas.MVVM;
using System;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public class LogicConnectorViewModel : IViewModel
    {
        private LogicConnector connector;

        public LogicComponentViewModel Component { get; }

        public int Id => connector.Id;
        public string Name => connector.Name;
        public bool IsOutput => connector is LogicConnectorOutput;
        public bool IsInput => connector is LogicConnectorInput;

        public LogicConnectorViewModel(LogicComponentViewModel component, LogicConnector connector)
        {
            Component = component;
            this.connector = connector;
        }
    }
}
