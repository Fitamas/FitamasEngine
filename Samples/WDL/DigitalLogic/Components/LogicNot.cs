using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDL.DigitalLogic.Components
{
    public class LogicNot : LogicComponent
    {
        public LogicNot(LogicComponentDescription description, LogicComponentData data)
            : base(description, data, 1, 1)
        {

        }

        public override void Update()
        {
            bool result = !input[0].Signal.IsHigh;
            output[0].ReceiveInput(new LogicSignal(result));
        }
    }
}
