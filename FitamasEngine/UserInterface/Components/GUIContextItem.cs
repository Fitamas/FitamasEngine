using Fitamas.Input.InputListeners;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUIContextItem : GUIButton
    {
        public GUIContextMenu Menu { get; set; }

        protected override void OnClickedButton()
        {
            if (Menu != null)
            {
                Menu.Activate(this);
            }
        }
    }
}
