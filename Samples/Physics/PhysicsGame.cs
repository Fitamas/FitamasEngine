using Fitamas;
using Fitamas.Audio;
using Fitamas.Core;
using Fitamas.ECS;
using Fitamas.Graphics;
using Fitamas.ImGuiNet;
using Fitamas.ImGuiNet.Windows;
using Fitamas.Input;
using Fitamas.Physics;
using Fitamas.UserInterface;
using Fitamas.UserInterface.ViewModel;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Physics.Gameplay;
using Physics.Settings;
using Physics.View;
using System;

namespace Physics
{
    public class PhysicsGame : GameEngine
    {
        public PhysicsGame()
        {
            ImGuiManager manager = new ImGuiManager(this);
            RenderManager.FinalRender = manager;

            manager.OpenWindow(new ConsoleWindow());
            manager.OpenWindow(new ConsoleWindow());
            manager.OpenWindow(new HierarchyWindow());
            manager.OpenWindow(new GameWorldWindow());
            manager.OpenWindow(new InspectorWindow());
            manager.OpenWindow(new SceneEditorWindow());
            manager.OpenWindow(new GUIEditorWindow());

            manager.ShowSeperateGameWindow = true;

            ImGuiIOPtr io = ImGui.GetIO();
            io.FontGlobalScale = 2f;

            ImGuiThemes.DarkTheme();
        }

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

            Entity entity = EntityHelper.CreatePumpkin(GameWorld, Vector2.Zero);
            entity.Attach(new AudioSource() 
            { 
                Clip = clip, PlayOnAwake = true, Looping = true, Is3d = true, MaxDistance = 4, 
                AttenuationModel = AudioAttenuationModel.LinearDistance, AttenuationRolloffFactor = 2,
            });
            entity.Get<SpriteRendererComponent>().Material = new Material(Content.Load<Effect>("File"));

            EntityHelper.CreateRock(GameWorld, new Vector2(0, -15));

            Entity entity1 = GameWorld.CreateEntity();
            entity1.Attach(new Transform() { Position = new Vector2(1, 0)});
            entity1.Attach(new AudioReverbZone() { MaxDistance = 3 });

            EntityHelper.CreateLight(GameWorld, Vector2.Zero);
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
