using System;

namespace WDL.DigitalLogic.Components
{
    public class LogicAnd : LogicComponent
    {
        public LogicAnd(LogicComponentDescription description, LogicComponentData data)
            : base(description, data, 2, 1)
        {

        }

        public override void Update()
        {
            bool result = input[0].Signal.IsHigh && input[1].Signal.IsHigh;
            output[0].ReceiveInput(new LogicSignal(result));
        }
    }
}
