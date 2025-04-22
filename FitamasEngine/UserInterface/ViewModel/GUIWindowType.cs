using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowType
    {
        private Func<IGUIWindowBinder> constructor;

        public GUIWindowType(Func<IGUIWindowBinder> constructor)
        {
            this.constructor = constructor;
        }

        public IGUIWindowBinder Create()
        {
            IGUIWindowBinder binder = constructor.Invoke();
            return binder;
        }
    }
}
