using Fitamas.Core;
using Fitamas;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework.Graphics;
using Physics.View;
using Fitamas.UserInterface.ViewModel;
using Physics.Gameplay;
using Microsoft.Xna.Framework;
using Fitamas.Input;
using Physics.Settings;

namespace Physics
{
    public class PhysicsGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            GameplayViewModel viewModel = new GameplayViewModel(this);
            MainContainer.RegisterInstance(viewModel);

            GameplayScreenViewModel screen = new GameplayScreenViewModel(viewModel);
            MainContainer.RegisterInstance(screen);
            GUIRootViewModel root = MainContainer.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
            root.OpenScreen(screen);

            InputManager manager = MainContainer.Resolve<InputManager>();
            ActionMap map = new ActionMap();
            manager.AddActionMap(map.InputActionMap);
            GameplayInputBinder binder = new GameplayInputBinder(map);
            binder.Bind(viewModel);

            EntityHelper.CreatePumpkin(World, Vector2.Zero);
            EntityHelper.CreateRock(World, new Vector2(0, -15));

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            GUIDebug.Active = true;

            FontManager.DefaultFont = Content.Load<SpriteFont>("Font\\Pixel_20");
            ResourceDictionary dictionary = ResourceDictionary.DefaultResources;
            dictionary[CommonResourceKeys.DefaultFont] = FontManager.DefaultFont;
        }
    }
}
