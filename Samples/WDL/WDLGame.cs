using Fitamas;
using Fitamas.Core;
using Fitamas.Entities;
using Fitamas.MVVM;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WDL.DigitalLogic;
using WDL.DigitalLogic.ViewModel;
using R3;
using ObservableCollections;
using Fitamas.UserInterface.ViewModel;

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
            GUIDarkTheme.CreateColors(ResourceDictionary.DefaultResources);

            GUIGameblayManager manager = new GUIGameblayManager(Container);
            Container.RegisterInstance(manager);
            GameplayScreenViewModel viewModel = manager.CreateGameplayViewModel();
            Container.RegisterInstance(viewModel);

            GUISystem system = Container.Resolve<GUISystem>(ApplicationKey.GUISystem);

            //GUIButton button = GUI.CreateButton(new Point(0, 100), "Button from C# script");
            //button.SetAlignment(GUIAlignment.Center);
            //button.OnClicked.AddListener((b, a) =>
            //{
            //    Debug.Log(b);
            //});
            //system.AddComponent(button);
        }
    }
}
