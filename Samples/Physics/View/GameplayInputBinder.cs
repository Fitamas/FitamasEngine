using Fitamas;
using Fitamas.Input;
using Fitamas.Input.Actions;
using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using Physics.Settings;
using System;

namespace Physics.View
{
    public class GameplayInputBinder : Binder<GameplayViewModel>
    {
        private ActionMap map;

        public GameplayInputBinder(ActionMap map)
        {
            this.map = map;
        }

        protected override IDisposable OnBind(GameplayViewModel viewModel)
        {
            map.MoveCamera.Performed += (context) =>
            {
                viewModel.MoveCamera(context.GetValue<Vector2>());
            };

            map.UseTool.Started += (context) =>
            {
                viewModel.BeginUseTool(map.UseToolPosition.GetValue<Point>());
            };

            map.UseTool.Canceled += (context) =>
            {
                viewModel.EndUseTool(map.UseToolPosition.GetValue<Point>());
            };

            map.UseToolPosition.Performed += (context) =>
            {
                if (map.UseTool.GetValue<bool>())
                {
                    viewModel.UseTool(context.GetValue<Point>());
                }
            };

            return null;
        }
    }
}
