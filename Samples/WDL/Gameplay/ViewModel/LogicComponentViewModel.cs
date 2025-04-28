using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using R3;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;
using WDL.DigitalLogic.Components;

namespace WDL.Gameplay.ViewModel
{
    public class LogicComponentViewModel : IViewModel
    {
        private LogicComponent component;

        public List<LogicConnectorViewModel> Connectors { get; }

        public string Name => component.Description.TypeId;
        public int ThemeId => component.Description.ThemeId;
        public int Id => component.Id;
        public LogicComponentDescription Description => component.Description;
        public ReactiveProperty<Point> Position => component.Position;

        public LogicComponentViewModel(LogicComponent component)
        {
            this.component = component;
            Connectors = new List<LogicConnectorViewModel>();
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
