using Fitamas;
using Fitamas.Core;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using R3;
using System;
using System.IO;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public class GameplayScreenBinder : GUIWindowBinder<GameplayScreenViewModel>
    {
        private GUIPopup ghostPopup;
        private GUINode ghostComponent;
        private GameplayScreenViewModel viewModel;
        private LogicComponentDescription description;

        protected override IDisposable OnBind(GameplayScreenViewModel viewModel)
        {
            this.viewModel = viewModel;
            SetAlignment(GUIAlignment.Stretch);

            //TOOLBAR
            GUIImage image = new GUIImage();
            image.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            image.Margin = new Thickness(0, 0, 0, 120);
            image.Pivot = Vector2.Zero;
            image.Color = new Color(0.4f, 0.4f, 0.4f);
            AddChild(image);

            GUIStack stack = new GUIStack();
            stack.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            stack.VerticalAlignment = GUIVerticalAlignment.Stretch;
            stack.Margin = new Thickness(10, 10, 10, 10);
            stack.Orientation = GUIGroupOrientation.Horizontal;
            stack.Spacing = 10;
            image.AddChild(stack);

            GUIButton button0 = GUI.CreateButton(new Point(0, 100), "Save", new Point(100, 100));
            button0.OnClicked.AddListener(b => 
            {
                if (!viewModel.IsSavedCurrentComponent() && viewModel.Simulation.CurrentValue != null)
                {
                    GUIMessageBox.Show("Do you want save this logic component?", string.Empty, GUIMessageBoxType.YesNo, res =>
                    {
                        if (res == GUIMessageBoxResult.Yes)
                        {
                            viewModel.OpenDescription();
                        }
                    });
                }
                else
                {
                    viewModel.SaveProject();
                }
            });
            stack.AddChild(button0);

            GUIButton button1 = GUI.CreateButton(new Point(0, 100), "New", new Point(100, 100));
            button1.OnClicked.AddListener(b => 
            {
                if (!viewModel.IsSavedCurrentComponent() && viewModel.Simulation.CurrentValue != null)
                {
                    GUIMessageBox.Show("Do you want save this logic component?", string.Empty, GUIMessageBoxType.YesNo, res =>
                    {
                        if (res == GUIMessageBoxResult.Yes)
                        {
                            viewModel.OpenDescription();
                        }
                        else
                        {
                            viewModel.CreateSimulation();
                        }    
                    });
                }
                else
                {
                    viewModel.CreateSimulation();
                }
            });
            stack.AddChild(button1);

            GUIButton button3 = GUI.CreateButton(new Point(0, 100), "Import", new Point(100, 100));
            button3.OnClicked.AddListener(b => 
            { 
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultPath = LogicComponentManager.RootDirectory;
                dialog.Filter = LogicComponentManager.FileExtension;
                dialog.Multiselect = true;
                dialog.OnCompleted += d =>
                {
                    viewModel.Import(d.Paths);
                };
                dialog.Show();
            });
            stack.AddChild(button3);

            viewModel.OnSelectComponent += value =>
            {
                if (ghostPopup != null)
                {
                    ghostPopup.IsOpen = true;
                    description = value;

                    if (ghostComponent != null)
                    {
                        ghostComponent.Destroy();
                    }

                    ghostComponent = GUINodeUtils.CreateNode(new Point(), description.TypeId);
                    GUIComponentUtils.Process(ghostComponent, description);
                    ghostComponent.Interacteble = false;
                    ghostComponent.Alpha = 0.5f;
                    ghostPopup.Window.AddChild(ghostComponent);
                }
            };

            return null;
        }

        protected override void OnInit()
        {
            base.OnInit();

            ghostPopup = new GUIPopup();
            ghostPopup.PlacementMode = GUIPlacementMode.Mouse;
            ghostPopup.OnClose.AddListener(s =>
            {
                if (viewModel.Simulation.CurrentValue != null)
                {
                    viewModel.Simulation.CurrentValue.CreateComponent(description, ghostPopup.Window.LocalPosition);
                }
            });
            System.AddComponent(ghostPopup);

            GUIWindow window1 = new GUIWindow();
            //window1.LocalSize = new Point(200, 200);
            ghostPopup.Window = window1;
            ghostPopup.AddChild(window1);
        }
    }
}
