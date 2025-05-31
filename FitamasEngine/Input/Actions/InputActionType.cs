using System;

namespace Fitamas.Input.Actions
{
    [Flags]
    public enum InputActionType
    {
        None = 0,
        Started = 1,
        Performed = 2,
        Canceled = 4,
    }
}
