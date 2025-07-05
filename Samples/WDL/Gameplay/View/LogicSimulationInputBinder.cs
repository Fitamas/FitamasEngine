using Fitamas;
using Fitamas.MVVM;
using System;
using WDL.Gameplay.Settings;

namespace WDL.Gameplay.View
{
    public class LogicSimulationInputBinder : Binder<LogicSimulationWindowViewModel>
    {
        private LogicSimulationWindowViewModel viewModel;
        private ActionMap map;

        public bool IsActive => viewModel != null && viewModel.IsFocus.Value;

        public LogicSimulationInputBinder(ActionMap map)
        {
            this.map = map;
            map.Delete.Started += context =>
            {
                if (IsActive)
                {
                    viewModel.DestroySelectComponents();
                }
            };
        }

        protected override IDisposable OnBind(LogicSimulationWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
            return null;
        }
    }
}
