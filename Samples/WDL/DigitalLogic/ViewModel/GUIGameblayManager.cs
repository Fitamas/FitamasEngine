using Fitamas.Container;
using Fitamas.Core;
using Fitamas.UserInterface.ViewModel;

public class GUIGameblayManager : GUIManager
{
    private GUIRootViewModel viewModel;

    public GUIGameblayManager(DIContainer container) : base(container)
    {
        viewModel = container.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
    }

    public GameplayScreenViewModel CreateGameplayViewModel()
    {
        GameplayScreenViewModel screen = new GameplayScreenViewModel(this);

        viewModel.OpenScreen(screen);

        return screen;
    }
}