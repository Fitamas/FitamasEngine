using Fitamas.UserInterface.ViewModel;
using System;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class LogicDescriptionWindowViewModel : GUIWindowViewModel
    {
        private GameplayViewModel gameplay;

        public LogicComponentDescription Description { get; }

        public override GUIWindowType Type => GUIWindowTypes.CreateDescriptionPopup;

        public LogicDescriptionWindowViewModel(GameplayViewModel gameplay)
        {
            this.gameplay = gameplay;
            Description = gameplay.Simulation.CurrentValue.Description;
        }

        public bool TrySaveComponent()
        {
            return gameplay.TrySaveComponent();
        }

        public void SaveProject()
        {
            gameplay.SaveProject();
        }
    }
}
