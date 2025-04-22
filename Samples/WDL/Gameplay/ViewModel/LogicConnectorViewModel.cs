using Fitamas.MVVM;
using System;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class LogicConnectorViewModel : IViewModel
    {
        private LogicConnector connector;

        public string Name { get; set; }
        public LogicComponentViewModel Component { get; }

        public int Id => connector.Id;
        public bool IsOutput => connector is LogicConnectorOutput;
        public bool IsInput => connector is LogicConnectorInput;

        public LogicConnectorViewModel(LogicComponentViewModel component, LogicConnector connector, string name)
        {
            Component = component;
            this.connector = connector;
            Name = name;
        }
    }
}
