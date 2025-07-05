using Fitamas.UserInterface.ViewModel;
using System;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public class LogicDescriptionWindowViewModel : GUIWindowViewModel
    {
        private GameplayViewModel gameplay;

        private LogicComponentDescription description;

        public string Name { get; set; }
        public int ThemeId { get; set; }
        public override GUIWindowType Type => GUIWindowTypes.CreateDescriptionPopup;

        public LogicDescriptionWindowViewModel(GameplayViewModel gameplay, LogicComponentDescription description)
        {
            this.gameplay = gameplay;
            this.description = description;
            Name = description.TypeId;
            ThemeId = description.ThemeId;
        }

        public bool IsSavedCurrentComponent()
        {
            return gameplay.IsSavedCurrentComponent();
        }

        public bool Contain(string fullname)
        {
            return gameplay.Contain(fullname);
        }

        public void SaveComponent()
        {
            description.TypeId = Name;
            description.ThemeId = ThemeId;
            gameplay.SaveComponent(description);
            gameplay.SaveProject();
        }
    }
}
