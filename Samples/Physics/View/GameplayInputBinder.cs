using Fitamas.Input;
using Fitamas.MVVM;
using System;

namespace Physics.View
{
    public class GameplayInputBinder : Binder<GameplayViewModel>
    {
        protected override IDisposable OnBind(GameplayViewModel viewModel)
        {
            InputSystem.mouse.MouseDown += (s, e) =>
            {
                viewModel.BeginUseTool(e.Position);
            };

            InputSystem.mouse.MouseDragStart += (s, e) =>
            {
                viewModel.BeginUseTool(e.Position);
            };

            InputSystem.mouse.MouseDrag += (s, e) =>
            {
                viewModel.UseTool(e.Position);
            };

            InputSystem.mouse.MouseUp += (s, e) =>
            {
                viewModel.EndUseTool(e.Position);
            };

            return null;
        }
    }
}
