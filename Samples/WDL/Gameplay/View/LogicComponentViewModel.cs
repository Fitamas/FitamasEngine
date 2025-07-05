using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using WDL.DigitalLogic;
using WDL.DigitalLogic.Components;

namespace WDL.Gameplay.View
{
    public class LogicComponentViewModel : IViewModel
    {
        private LogicComponent component;
        private Dictionary<int, LogicConnectorViewModel> connectorMap;
        public List<LogicConnectorViewModel> connectors;

        public IReadOnlyList<LogicConnectorViewModel> Connectors => connectors;
        public bool IsSelect { get; set; }

        public string Name => component.Description.TypeId;
        public int ThemeId => component.Description.ThemeId;
        public int Id => component.Id;
        public LogicComponentDescription Description => component.Description;
        public ReactiveProperty<Point> Position => component.Position;

        public LogicComponentViewModel(LogicComponent component)
        {
            this.component = component;
            connectors = new List<LogicConnectorViewModel>();
            connectorMap = new Dictionary<int, LogicConnectorViewModel>();

            for (int i = 0; i < component.InputCount; i++)
            {
                LogicConnector connector = component.GetInputFromIndex(i);
                LogicConnectorViewModel connectorViewModel = new LogicConnectorViewModel(this, connector);
                connectors.Add(connectorViewModel);
                connectorMap.Add(connector.Id, connectorViewModel);
            }

            for (int i = 0; i < component.OutputCount; i++)
            {
                LogicConnector connector = component.GetOutputFromIndex(i);
                LogicConnectorViewModel connectorViewModel = new LogicConnectorViewModel(this, connector);
                connectors.Add(connectorViewModel);
                connectorMap.Add(connector.Id, connectorViewModel);
            }
        }

        public LogicConnectorViewModel GetConnector(int id)
        {
            return connectorMap[id];
        }

        public bool TrySetSignalValue(bool signal)
        {
            if (component is LogicInput input)
            {
                input.Signal.Value = signal;
                return true;
            }

            return false;
        }

        public bool TryGetSignalValue(out ReadOnlyReactiveProperty<bool> signal)
        {
            if (component is LogicInput logic)
            {
                signal = logic.Signal;
                return true;
            }
            else if (component is LogicOutput output)
            {
                signal = output.Signal;
                return true;
            }

            signal = null;
            return false;
        }
    }
}
