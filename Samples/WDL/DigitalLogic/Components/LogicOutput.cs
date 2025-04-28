using R3;
using System;

namespace WDL.DigitalLogic.Components
{
    public class LogicOutput : LogicComponent
    {
        private ReactiveProperty<bool> signal;

        public ReadOnlyReactiveProperty<bool> Signal => signal;

        public LogicOutput(LogicComponentDescription description, LogicComponentData data)
            : base(description, data, 1, 0)
        {
            signal = new ReactiveProperty<bool>();
        }

        public override void Update()
        {
            signal.Value = input[0].Signal.IsHigh;
        }
    }
}
