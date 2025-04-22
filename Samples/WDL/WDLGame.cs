using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework.Graphics;
using WDL.DigitalLogic;
using WDL.Gameplay.ViewModel;

namespace WDL
{
    public class WDLGame : GameEngine
    {
        protected override WorldBuilder CreateWorldBuilder()
        {
            LogicSystem logicSystem = new LogicSystem();
            GameplayViewModel viewModel = new GameplayViewModel(logicSystem);
            Container.RegisterInstance(viewModel);

            return base.CreateWorldBuilder()
                    .AddSystem(logicSystem);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary.DefaultResources[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
            GUIUtils.CreateStyles(ResourceDictionary.DefaultResources);
            GUIDarkTheme.CreateColors(ResourceDictionary.DefaultResources);

            GUIGameblayManager manager = new GUIGameblayManager(Container);
            Container.RegisterInstance(manager);
            GameplayScreenViewModel viewModel = manager.OpenGameplayViewModel();
            Container.RegisterInstance(viewModel);


            viewModel.OpenSimulation(); //TODO
            viewModel.OpenComponents();
        }
    }
}
