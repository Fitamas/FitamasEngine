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

            GUIButton button4 = GUI.CreateButton(Point.Zero, "TEST");
            button4.OnClicked.AddListener(s =>
            {
                viewModel.SelectTool(Tool.None);
            });
            grid.AddChild(button4);

            return base.OnBind(viewModel);
        }
    }
}
