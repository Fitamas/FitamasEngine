using Fitamas;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Input;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using System;

namespace Physics.View
{
    public class GameplayScreenBinder : GUIWindowBinder<GameplayScreenViewModel>
    {
        protected override IDisposable OnBind(GameplayScreenViewModel viewModel)
        {
            SetAlignment(GUIAlignment.Stretch);
            RaycastTarget = false;

            ResourceDictionary dictionary = ResourceDictionary.DefaultResources;

            GUIEventManager.Register(GUIMouse.MouseEnterGUIEvent, new GUIEvent<GUIComponent, GUIEventArgs>((c, a) =>
            {
                viewModel.CanUse = false;
            }));
            GUIEventManager.Register(GUIMouse.MouseExitGUIEvent, new GUIEvent<GUIComponent, GUIEventArgs>((c, a) =>
            {
                viewModel.CanUse = true;
            }));

            GUIGrid grid = new GUIGrid();
            grid.Margin = new Thickness(dictionary.FramePadding.X, dictionary.FramePadding.Y, dictionary.FramePadding.X, 100);
            grid.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            grid.VerticalAlignment = GUIVerticalAlignment.Top;
            grid.Pivot = new Vector2(0, 0);
            grid.CellSize = new Point(200, 50);
            grid.Spacing = dictionary.FramePadding;
            AddChild(grid);

            GUIButton button1 = GUI.CreateButton(Point.Zero, "None");
            button1.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.None);
            });
            grid.AddChild(button1);

            GUIButton button2 = GUI.CreateButton(Point.Zero, "Pumpkin");
            button2.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.Pumpkin);
            });
            grid.AddChild(button2);

            GUIButton button3 = GUI.CreateButton(Point.Zero, "Log");
            button3.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.Log);
            });
            grid.AddChild(button3);

            GUIButton button4 = GUI.CreateButton(Point.Zero, "TestBox");
            button4.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.TestBox);
            });
            grid.AddChild(button4);

            GUIButton button5 = GUI.CreateButton(Point.Zero, "Wheel");
            button5.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.Wheel);
            });
            grid.AddChild(button5);

            GUIButton button6 = GUI.CreateButton(Point.Zero, "Destroy");
            button6.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.Destroy);
            });
            grid.AddChild(button6);

            GUIButton button7 = GUI.CreateButton(Point.Zero, "RopeJoint");
            button7.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.RopeJoint);
            });
            grid.AddChild(button7);

            GUIButton button8 = GUI.CreateButton(Point.Zero, "RevoltJoint");
            button8.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.RevoltJoint);
            });
            grid.AddChild(button8);

            GUIButton button9 = GUI.CreateButton(Point.Zero, "WheelJoint");
            button9.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.WheelJoint);
            });
            grid.AddChild(button9);

            return base.OnBind(viewModel);
        }
    }
}
