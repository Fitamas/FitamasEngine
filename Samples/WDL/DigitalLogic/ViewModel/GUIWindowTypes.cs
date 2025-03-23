using Fitamas.UserInterface.ViewModel;
using System;
using Fitamas.Core;
using Fitamas.UserInterface.Components;

namespace WDL.DigitalLogic.ViewModel
{
    public static class GUIWindowTypes
    {
        public static GUIWindowType GameplayScreen = CreateRegister("GameplayScreen", () => new GameplayScreenBinder());

        private static GUIWindowType CreateRegister(string typeId, Func<GUIWindowBinder> constructor)
        {
            return Registry.Register(BuiltInRegistries.Windows, typeId, new GUIWindowType(constructor));
        }
    }
}
