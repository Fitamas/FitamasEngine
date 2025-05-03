using Fitamas.Container;
using Fitamas.Core;
using Fitamas.MVVM;
using Fitamas.UserInterface.ViewModel;
using System;

namespace Physics.View
{
    public class GameplayScreenViewModel : GUIWindowViewModel
    {
        private GameplayViewModel gameplay;

        public override GUIWindowType Type { get; } = new GUIWindowType(() => new GameplayScreenBinder());

        public bool CanUse
        {
            get
            {
                return gameplay.CanUse;
            }
            set
            {
                gameplay.CanUse = value;
            }
        }

        public GameplayScreenViewModel(GameplayViewModel gameplay)
        {
            this.gameplay = gameplay;
        }

        public void SelectTool(Tool tool)
        {
            gameplay.SelectTool(tool);
        }
    }
}
