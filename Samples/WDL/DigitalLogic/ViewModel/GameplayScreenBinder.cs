using Fitamas;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;

namespace WDL.DigitalLogic.ViewModel
{
    public class GameplayScreenBinder : GUIWindowBinder
    {
        private Dictionary<LogicComponentDescription, GUITreeNode> componentDescriptionMap;

        protected override void BindOverride(object viewModel)
        {
            GameplayScreenViewModel screenViewModel = viewModel as GameplayScreenViewModel;
            GameplayViewModel gameplay = screenViewModel.Gameplay;
            Window = new GUIWindow();
            Window.SetAlignment(GUIAlignment.Stretch);

            componentDescriptionMap = new Dictionary<LogicComponentDescription, GUITreeNode>();

            //TOOLBAR
            GUIImage image = new GUIImage();
            image.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            image.Margin = new Thickness(0, 0, 0, 120);
            image.Pivot = Vector2.Zero;
            Window.AddChild(image);

            GUIStack stack = new GUIStack();
            stack.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            stack.VerticalAlignment = GUIVerticalAlignment.Stretch;
            stack.Margin = new Thickness(10, 10, 10, 10);
            stack.Orientation = GUIGroupOrientation.Horizontal;
            stack.Spacing = 10;
            image.AddChild(stack);

            GUIButton button0 = GUI.CreateButton(new Point(0, 100), "TOOL0", new Point(100, 100));
            button0.OnClicked.AddListener((b, a) => { Debug.Log(b); });
            stack.AddChild(button0);

            GUIButton button1 = GUI.CreateButton(new Point(0, 100), "TOOL2", new Point(100, 100));
            button1.OnClicked.AddListener((b, a) => { Debug.Log(b); });
            stack.AddChild(button1);

            GUIButton button3 = GUI.CreateButton(new Point(0, 100), "TOOL3", new Point(100, 100));
            button3.OnClicked.AddListener((b, a) => { Debug.Log(b); });
            stack.AddChild(button3);

            //COMPONENTS
            GUIImage image1 = new GUIImage();
            image1.VerticalAlignment = GUIVerticalAlignment.Stretch;
            image1.Margin = new Thickness(0, 120, 400, 0);
            image1.Pivot = Vector2.Zero;
            Window.AddChild(image1);

            GUITreeView treeView = GUI.CreateTreeView(Point.Zero, Point.Zero);
            treeView.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            treeView.VerticalAlignment = GUIVerticalAlignment.Stretch;
            treeView.Margin = new Thickness(10, 10, 10, 10);
            image1.AddChild(treeView);

            foreach (var description in gameplay.ComponentDescriptions)
            {
                componentDescriptionMap[description] = treeView.CreateTreeNode(description.TypeId);
            }

            gameplay.ComponentDescriptions.ObserveAdd().Subscribe(e => 
            {
                componentDescriptionMap[e.Value] = treeView.CreateTreeNode(e.Value.TypeId);
            });

            gameplay.ComponentDescriptions.ObserveRemove().Subscribe(e => 
            { 
                if (componentDescriptionMap.Remove(e.Value, out GUITreeNode node))
                {
                    treeView.RemoveItem(node);
                }
            });

            //EDITOR
            GUIImage image2 = new GUIImage();
            image2.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            image2.VerticalAlignment = GUIVerticalAlignment.Stretch;
            image2.Margin = new Thickness(400, 120, 0, 0);
            Window.AddChild(image2);

            GUINodeEditor editor = new GUINodeEditor();
            editor.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            editor.VerticalAlignment = GUIVerticalAlignment.Stretch;
            image2.AddChild(editor);



            //TEST
            GUINode node = editor.CreateNode("NODE");
            node.CreatePin("text");

            Debug.Log(node.LocalPosition);
            Debug.Log(node.LocalSize);
            //editor.CreateWire()
        }
    }
}
