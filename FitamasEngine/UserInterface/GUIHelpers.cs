using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface
{
    public static class GUIHelpers
    {
        public static GUIStack Sameline()
        {
            GUIStack stack = new GUIStack();
            stack.Orientation = GUIGroupOrientation.Horizontal;
            stack.ControlSizeWidth = true;
            stack.ControlSizeHeight = true;

            return stack;
        }
    }
}
