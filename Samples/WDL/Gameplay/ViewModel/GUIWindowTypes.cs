using Fitamas.UserInterface.ViewModel;
using System;
using Fitamas.Core;
using Fitamas.UserInterface.Components;
using WDL.DigitalLogic;
using Fitamas.UserInterface;

namespace WDL.Gameplay.ViewModel
{
    public static class GUIWindowTypes
    {
        public static GUIWindowType GameplayScreen = CreateRegister(nameof(GameplayScreen), () => new GameplayScreenBinder());

        public static GUIWindowType SimulationWindow = CreateRegister(nameof(SimulationWindow), () => new LogicSimulationWindowBinder());

        public static GUIWindowType CreateDescriptionPopup = CreateRegister(nameof(CreateDescriptionPopup), GUI.CreateWindow<LogicDescriptionWindowBinder>);

        public static GUIWindowType LogicComponents = CreateRegister(nameof(LogicComponents), () => new LogicComponentsWindowBinder());

        private static GUIWindowType CreateRegister(string typeId, Func<IGUIWindowBinder> constructor)
        {
            return Registry.Register(BuiltInRegistries.Windows, typeId, new GUIWindowType(constructor));
        }
    }
}
