using Fitamas;
using Fitamas.Input;
using Fitamas.Input.Actions;
using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using WDL.Gameplay.Settings;
using System;

namespace WDL.Gameplay.View
{
    public class GameplayInputBinder : Binder<GameplayScreenViewModel>
    {
        private ActionMap map;

        public GameplayInputBinder(ActionMap map)
        {
            this.map = map;
        }

        protected override IDisposable OnBind(GameplayScreenViewModel viewModel)
        {
            map.SaveProject.Started += (context) =>
            {
                if (!viewModel.IsSavedCurrentComponent())
                {
                    viewModel.OpenDescription();
                }

                viewModel.SaveProject();
            };

            return null;
        }
    }
}
