﻿using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;
using ObservableCollections;
using R3;
using Fitamas;
using Fitamas.UserInterface.Themes;

namespace WDL.Gameplay.View
{
    public class LogicComponentsWindowBinder : GUIWindowBinder<LogicComponentsWindowViewModel>
    {
        private Dictionary<LogicComponentDescription, GUITreeNode> descriptionToNode;
        private Dictionary<GUITreeNode, LogicComponentDescription> nodeToDescription;
        private GUITreeView treeView;
        private GUIContextMenu contextMenu;

        protected override IDisposable OnBind(LogicComponentsWindowViewModel viewModel)
        {
            descriptionToNode = new Dictionary<LogicComponentDescription, GUITreeNode>();
            nodeToDescription = new Dictionary<GUITreeNode, LogicComponentDescription>();

            VerticalAlignment = GUIVerticalAlignment.Stretch;
            Margin = new Thickness(0, 120, 400, 0);
            Pivot = Vector2.Zero;

            GUIImage image1 = new GUIImage();
            image1.SetAlignment(GUIAlignment.Stretch);
            image1.Color = new Color(0.3f, 0.3f, 0.3f);
            AddChild(image1);

            treeView = GUI.CreateTreeView(Point.Zero, Point.Zero);
            treeView.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            treeView.VerticalAlignment = GUIVerticalAlignment.Stretch;
            treeView.Margin = new Thickness(10, 10, 10, 10);
            treeView.OnSelectTreeNode.AddListener(args =>
            {
                if (nodeToDescription.TryGetValue(args.Node, out var description))
                {
                    viewModel.SelectComponent(description);
                }
            });
            image1.AddChild(treeView);

            GUIPopup popup = new GUIPopup();
            contextMenu = GUI.CreateContextMenu();
            contextMenu.AddItem("Remove description");
            contextMenu.AddItem("Edit");
            contextMenu.AddItem("Properties");
            contextMenu.OnSelectItem.AddListener((m, a) =>
            {
                if (m.Target is GUITreeNode node && nodeToDescription.TryGetValue(node, out LogicComponentDescription description))
                {
                    switch (a.Index)
                    {
                        case 0:
                            viewModel.Remove(description);
                            break;
                        case 1:
                            if (!viewModel.IsSavedCurrentComponent() && viewModel.Simulation.CurrentValue != null)
                            {
                                GUIMessageBox.Show("Do you want save this logic component?", string.Empty, GUIMessageBoxType.YesNo, res =>
                                {
                                    if (res == GUIMessageBoxResult.Yes)
                                    {
                                        viewModel.OpenDescription(viewModel.Simulation.CurrentValue.Description);
                                    }
                                    else if (res == GUIMessageBoxResult.No)
                                    {
                                        viewModel.CreateSimulation(description);
                                    }
                                });
                            }
                            else
                            {
                                viewModel.CreateSimulation(description);
                            }
                            break;
                        case 2:
                            viewModel.OpenDescription(description);
                            break;
                    }
                }
            });
            popup.Window = contextMenu;
            popup.AddChild(contextMenu);
            AddChild(popup);

            foreach (var description in viewModel.ComponentDescriptions)
            {
                Add(description);
            }

            viewModel.ComponentDescriptions.ObserveAdd().Subscribe(e =>
            {
                Add(e.Value);
            });

            viewModel.ComponentDescriptions.ObserveRemove().Subscribe(e =>
            {
                Remove(e.Value);
            });

            return null;
        }

        private void Add(LogicComponentDescription description)
        {
            GUITreeNode treeNode = treeView.CreateTreeNode(description.TypeId);
            GUIContextMenuManager.SetContextMenu(treeNode, contextMenu);
            treeNode.ControlTemplate[GUITreeStyle.Icon].SetResourceReference(
                GUIImage.SpriteProperty, ResourceDictionary.DefaultResources, GUIStyleHelpers.CircuitTexture);
            treeNode.ControlTemplate[GUITreeStyle.Icon].SetResourceReference(
                GUIImage.ColorProperty, ResourceDictionary.DefaultResources, CommonResourceKeys.NodeEditorNodeBodyDefaultColor + description.ThemeId);
            descriptionToNode[description] = treeNode;
            nodeToDescription[treeNode] = description;
        }

        private void Remove(LogicComponentDescription description)
        {
            if (descriptionToNode.Remove(description, out GUITreeNode node))
            {
                treeView.RemoveItem(node);
                nodeToDescription.Remove(node);
                if (node.ParentNode != null)
                {
                    node.ParentNode.RemoveItem(node);
                }
                else
                {
                    node.TreeView.RemoveItem(node);
                }
            }
        }
    }
}
