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
using Fitamas.Audio;
using Fitamas.Entities;
using Fitamas.Audio.Filters;
using Fitamas.Graphics;

namespace Physics
{
    public class PhysicsGame : GameEngine
    {
        protected override void Initialize()
        {
            base.Initialize();

            ActionMap map = new ActionMap();
            InputManager.AddActionMap(map.InputActionMap);

            GameplayViewModel viewModel = new GameplayViewModel(this);
            MainContainer.RegisterInstance(viewModel);

            GameplayScreenViewModel screen = new GameplayScreenViewModel(viewModel, map);
            MainContainer.RegisterInstance(screen);
            GUIRootViewModel root = MainContainer.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
            root.OpenScreen(screen);

            GameplayInputBinder binder = new GameplayInputBinder(map);
            binder.Bind(viewModel);

            Entity entity = EntityHelper.CreatePumpkin(World, Vector2.Zero);
            EntityHelper.CreateRock(World, new Vector2(0, -15));

            AudioClip clip = new AudioClip("Content\\Piano_Ui (5).wav");

            entity.Attach(new AudioSource() 
            { 
                Clip = clip, PlayOnAwake = true, Looping = true, Is3d = true, MaxDistance = 4, 
                AttenuationModel = AudioAttenuationModel.LinearDistance, AttenuationRolloffFactor = 2,
            });

            entity = World.CreateEntity();
            entity.Attach(new Transform() { Position = new Vector2(1, 0)});
            entity.Attach(new AudioReverbZone() { MaxDistance = 3 });
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
