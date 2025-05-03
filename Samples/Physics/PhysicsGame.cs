using Fitamas.Core;
using Fitamas;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework.Graphics;
using Physics.View;
using Fitamas.UserInterface.ViewModel;
using Physics.Gameplay;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class PhysicsGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            GameplayViewModel viewModel = new GameplayViewModel(World);
            Container.RegisterInstance(viewModel);

            GameplayScreenViewModel screen = new GameplayScreenViewModel(viewModel);
            Container.RegisterInstance(screen);
            GUIRootViewModel root = Container.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
            root.OpenScreen(screen);

            GameplayInputBinder binder = new GameplayInputBinder();
            binder.Bind(viewModel);

            EntityHelper.CreatePumpkin(World, Vector2.Zero);
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
