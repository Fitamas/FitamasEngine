using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework.Graphics;
using WDL.DigitalLogic;
using WDL.Gameplay.ViewModel;

namespace WDL
{
    public class WDLGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            GameplayViewModel gameplayViewModel = new GameplayViewModel(MainContainer.Resolve<LogicSystem>());
            MainContainer.RegisterInstance(gameplayViewModel);
            GUIGameblayManager manager = new GUIGameblayManager(MainContainer);
            MainContainer.RegisterInstance(manager);
            GameplayScreenViewModel viewModel = manager.OpenGameplayScreen();
            MainContainer.RegisterInstance(viewModel);

            viewModel.CreateSimulation();
            viewModel.OpenComponents();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            //GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary dictionary = ResourceDictionary.DefaultResources;
            dictionary[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
            GUIUtils.CreateStyles(ResourceDictionary.DefaultResources);
            GUIDarkTheme.CreateColors(ResourceDictionary.DefaultResources);
        }

        protected override WorldBuilder CreateWorldBuilder()
        {
            return base.CreateWorldBuilder()
                .AddSystemAndRegister(new LogicSystem(), MainContainer);
        }
    }
}
