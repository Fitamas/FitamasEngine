using Fitamas;
using Fitamas.Audio;
using Fitamas.Audio.Filters;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.Scene;
using Fitamas.Serialization.Json;
using Fitamas.UserInterface;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Physics.Gameplay;
using Physics.Settings;
using Physics.View;

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

            AudioClip clip = AudioClip.LoadWav("Piano_Ui (5).wav");

            Entity entity = EntityHelper.CreatePumpkin(World, Vector2.Zero);
            entity.Attach(new AudioSource() 
            { 
                Clip = clip, PlayOnAwake = true, Looping = true, Is3d = true, MaxDistance = 4, 
                AttenuationModel = AudioAttenuationModel.LinearDistance, AttenuationRolloffFactor = 2,
            });

            EntityHelper.CreateRock(World, new Vector2(0, -15));

            Entity entity1 = World.CreateEntity();
            entity1.Attach(new Transform() { Position = new Vector2(1, 0)});
            entity1.Attach(new AudioReverbZone() { MaxDistance = 3 });
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
