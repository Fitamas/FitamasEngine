using Fitamas.Container;
using Fitamas.Core;
using Fitamas.MVVM;
using Fitamas.UserInterface.ViewModel;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class GUIGameblayManager : GUIManager
    {
        private GUIRootViewModel viewModel;

        public GUIGameblayManager(DIContainer container) : base(container)
        {
            viewModel = Container.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
        }

        public GameplayScreenViewModel OpenGameplayViewModel()
        {
            GameplayScreenViewModel screen = new GameplayScreenViewModel(this);
            viewModel.OpenScreen(screen);

            return screen;
        }

        public LogicComponentsWindowViewModel OpenComponents()
        {
            GameplayViewModel gameplay = Container.Resolve<GameplayViewModel>();
            LogicComponentsWindowViewModel logicComponents = new LogicComponentsWindowViewModel(gameplay);
            viewModel.OpenWindow(logicComponents);

            return logicComponents;
        }

        public LogicSimulationWindowViewModel OpenSimulation(LogicComponentDescription description)
        {
            GameplayViewModel gameplay = Container.Resolve<GameplayViewModel>();
            LogicSimulationWindowViewModel simulation = gameplay.CreateSimulation(description);
            viewModel.OpenWindow(simulation);

            return simulation;
        }

        public LogicDescriptionWindowViewModel OpenCreateDescriptionPopup()
        {
            GameplayViewModel gameplay = Container.Resolve<GameplayViewModel>();
            LogicDescriptionWindowViewModel popup = new LogicDescriptionWindowViewModel(gameplay);
            viewModel.OpenWindow(popup);

            return popup;
        }
    }
}