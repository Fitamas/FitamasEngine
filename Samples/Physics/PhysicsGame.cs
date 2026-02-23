using Fitamas;
using Fitamas.Audio;
using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Fonts;
using Fitamas.Graphics;
using Fitamas.ImGuiNet;
using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.Serialization;
using Fitamas.ImGuiNet.Windows;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.ViewModel;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Physics.Settings;
using Physics.View;
using System;

namespace Physics
{
    public class PhysicsGame : GameEngine
    {
        public PhysicsGame()
        {
            AssetSystem.InitializeProject();

            Content = new FitamasContentManager(Services, "Content");

            ImGuiManager manager = new ImGuiManager(this);
            manager.ShowSeperateGameWindow = true;
            RenderManager.FinalRender = manager;
            MainContainer.RegisterInstance(manager, true);

            ImGuiIOPtr io = ImGui.GetIO();
            io.FontGlobalScale = 2f;

            ImGuiThemes.DarkTheme();

            GUIDebug.Active = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Entity entity = GameWorld.CreateEntity();
            entity.Attach(new Transform());
            entity.Attach(new SpriteRendererComponent());


            //ActionMap map = new ActionMap();
            //InputManager.AddActionMap(map.InputActionMap);

            //GameplayViewModel viewModel = new GameplayViewModel(this);
            //MainContainer.RegisterInstance(viewModel);

            //GameplayScreenViewModel screen = new GameplayScreenViewModel(viewModel, map);
            //MainContainer.RegisterInstance(screen);
            //GUIRootViewModel root = MainContainer.Resolve<GUIRootViewModel>(ApplicationKey.GUIRootViewModel);
            //root.OpenScreen(screen);

            //GameplayInputBinder binder = new GameplayInputBinder(map);
            //binder.Bind(viewModel);

            //AudioClip clip = AudioClip.LoadWav("Piano_Ui (5).wav");

            //Entity entity = EntityHelper.CreatePumpkin(GameWorld, Vector2.Zero);
            //entity.Attach(new AudioSource() 
            //{ 
            //    Clip = clip, PlayOnAwake = true, Looping = true, Is3d = true, MaxDistance = 4, 
            //    AttenuationModel = AudioAttenuationModel.LinearDistance, AttenuationRolloffFactor = 2,
            //});
            //entity.Get<SpriteRendererComponent>().Material = new Material(Content.Load<Effect>("File"));

            //EntityHelper.CreateRock(GameWorld, new Vector2(0, -15));

            //Entity entity1 = GameWorld.CreateEntity();
            //entity1.Attach(new Transform() { Position = new Vector2(1, 0)});
            //entity1.Attach(new AudioReverbZone() { MaxDistance = 3 });

            //EntityHelper.CreateLight(GameWorld, Vector2.Zero);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            FramesPerSecondCounter.Instance.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            FramesPerSecondCounter.Instance.Draw(gameTime);
        }
    }
}
