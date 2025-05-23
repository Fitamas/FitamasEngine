﻿using Fitamas;
using Fitamas.MVVM;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System;

namespace WDL.Gameplay.ViewModel
{
    public class GameplayScreenBinder : GUIWindowBinder<GameplayScreenViewModel>
    {
        protected override IDisposable OnBind(GameplayScreenViewModel viewModel)
        {
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
                            viewModel.OpenDescription(viewModel.Simulation.CurrentValue.Description);
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
                            viewModel.OpenDescription(viewModel.Simulation.CurrentValue.Description);
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

            GUIButton button3 = GUI.CreateButton(new Point(0, 100), "TOOL3", new Point(100, 100));
            button3.OnClicked.AddListener(b => 
            { 
                Debug.Log(b); 
            });
            stack.AddChild(button3);

            return null;
        }
    }
}
