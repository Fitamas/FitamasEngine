using R3;
using System;

namespace WDL.DigitalLogic.Components
{
    public class LogicInput : LogicComponent
    {
        public ReactiveProperty<bool> Signal { get; }

        public LogicInput(LogicComponentDescription description, LogicComponentData data)
            : base(description, data, 0, 1)
        {
            Signal = new ReactiveProperty<bool>();
        }

        public override void Update()
        {
            output[0].ReceiveInput(new LogicSignal(Signal.Value));
        }
    }
}
