using Fitamas.Container;
using Fitamas.Core;
using Fitamas.MVVM;
using Fitamas.UserInterface.ViewModel;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public class GUIGameblayManager : GUIManager
    {
        private GUIRootViewModel viewModel;

        public GUIGameblayManager(DIContainer container) : base(container)
        {
            viewModel = Container.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
        }

        public GameplayScreenViewModel OpenGameplayScreen()
        {
            GameplayScreenViewModel screen = new GameplayScreenViewModel(this);
            viewModel.OpenScreen(screen);

            return screen;
        }

        public LogicComponentsWindowViewModel OpenComponents()
        {
            GameplayScreenViewModel gameplay = Container.Resolve<GameplayScreenViewModel>();
            LogicComponentsWindowViewModel logicComponents = new LogicComponentsWindowViewModel(gameplay);
            viewModel.OpenWindow(logicComponents);

            return logicComponents;
        }

        public LogicSimulationWindowViewModel OpenSimulation(LogicSimulationViewModel simulationViewModel)
        {
            LogicSimulationWindowViewModel simulation = new LogicSimulationWindowViewModel(simulationViewModel);
            viewModel.OpenWindow(simulation);

            return simulation;
        }

        public LogicDescriptionWindowViewModel OpenCreateDescription(LogicComponentDescription description)
        {
            GameplayViewModel gameplay = Container.Resolve<GameplayViewModel>();
            LogicDescriptionWindowViewModel popup = new LogicDescriptionWindowViewModel(gameplay, description);
            viewModel.OpenWindow(popup);

            return popup;
        }
    }
}