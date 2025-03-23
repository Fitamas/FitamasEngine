using Fitamas.UserInterface.Components;
using System;

namespace Fitamas.UserInterface.ViewModel
{
    public class GUIWindowType
    {
        private Func<GUIWindowBinder> constructor;

        public GUIWindowType(Func<GUIWindowBinder> constructor)
        {
            this.constructor = constructor;
        }

        public GUIWindowBinder Create()
        {
            GUIWindowBinder binder = constructor.Invoke();
            return binder;
        }
    }
}
