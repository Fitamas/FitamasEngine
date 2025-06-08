using Fitamas.Container;
using Fitamas.Core;
using Fitamas.Input.Actions;
using Fitamas.MVVM;
using Fitamas.UserInterface.ViewModel;
using Physics.Settings;
using System;

namespace Physics.View
{
    public class GameplayScreenViewModel : GUIWindowViewModel
    {
        private GameplayViewModel gameplay;
        private ActionMap map;

        public override GUIWindowType Type { get; } = new GUIWindowType(() => new GameplayScreenBinder());

        public bool CanUse
        {
            get
            {
                return map.InputActionMap.Enable;
            }
            set
            {
                map.InputActionMap.Enable = value;
            }
        }

        public GameplayScreenViewModel(GameplayViewModel gameplay, ActionMap map)
        {
            this.gameplay = gameplay;
            this.map = map;
        }

        public void SelectTool(Tool tool)
        {
            gameplay.SelectTool(tool);
        }
    }
}
