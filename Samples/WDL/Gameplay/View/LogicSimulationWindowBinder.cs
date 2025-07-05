using Fitamas;
using Fitamas.MVVM;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public class LogicComponentBinder
    {
        public LogicComponentViewModel Component { get; }
        public GUINode Node { get; }
        public Dictionary<GUIPin, LogicConnectorViewModel> ConnectorMap { get; }
        public Dictionary<LogicConnectorViewModel, GUIPin> PinMap { get; }

        public LogicComponentBinder(LogicComponentViewModel component, GUINode node)
        {
            Component = component;
            Node = node;
            ConnectorMap = new Dictionary<GUIPin, LogicConnectorViewModel>();
            PinMap = new Dictionary<LogicConnectorViewModel, GUIPin>();
        }
    }

    public class LogicSimulationWindowBinder : GUIWindowBinder<LogicSimulationWindowViewModel>
    {
        private GUINodeEditor editor;

        private Dictionary<LogicComponentViewModel, LogicComponentBinder> componentToNode;
        private Dictionary<GUINode, LogicComponentBinder> nodeToComponent;

        private Dictionary<LogicConnectionViewModel, GUIWire> connectionToWire;
        private Dictionary<GUIWire, LogicConnectionViewModel> wireToConnection;

        private GUIContextMenu editorContextMenu;
        private GUIContextMenu nodeContextMenu;
        private GUIContextMenu wireContextMenu;

        public LogicSimulationWindowBinder()
        {
            componentToNode = new Dictionary<LogicComponentViewModel, LogicComponentBinder>();
            nodeToComponent = new Dictionary<GUINode, LogicComponentBinder>();
            connectionToWire = new Dictionary<LogicConnectionViewModel, GUIWire>();
            wireToConnection = new Dictionary<GUIWire, LogicConnectionViewModel>();
        }

        protected override IDisposable OnBind(LogicSimulationWindowViewModel viewModel)
        {
            SetAlignment(GUIAlignment.Stretch);
            Margin = new Thickness(400, 120, 0, 0);

            GUIImage image2 = new GUIImage();
            image2.Color = new Color(0.2f, 0.2f, 0.2f);
            image2.SetAlignment(GUIAlignment.Stretch);
            AddChild(image2);

            editor = GUINodeUtils.CreateNodeEditor();
            editor.SetAlignment(GUIAlignment.Stretch);
            AddChild(editor);
            editor.OnNodeEvent.AddListener(args =>
            {
                if (nodeToComponent.TryGetValue(args.Node, out var viewModel))
                {
                    if (args.EventType == GUINodeEventType.Select)
                    {
                        viewModel.Component.IsSelect = true;
                    }
                    else if (args.EventType == GUINodeEventType.Unselect)
                    {
                        viewModel.Component.IsSelect = false;
                    }

                    viewModel.Component.Position.Value = args.Node.LocalPosition;
                }
            });
            editor.OnCreateConnection.AddListener(args =>
            {
                if (nodeToComponent.ContainsKey(args.PinA.Node) && nodeToComponent.ContainsKey(args.PinB.Node))
                {
                    if (nodeToComponent[args.PinA.Node].ConnectorMap.TryGetValue(args.PinA, out LogicConnectorViewModel viewModel0) &&
                        nodeToComponent[args.PinB.Node].ConnectorMap.TryGetValue(args.PinB, out LogicConnectorViewModel viewModel1))
                    {
                        viewModel.CreateConnect(viewModel0, viewModel1, args.Points);
                    }
                }
            });

            GUIPopup popup;

            //popup = new GUIPopup();
            //editorContextMenu = GUI.CreateContextMenu();
            //editorContextMenu.AddItem("...");
            //popup.Window = editorContextMenu;
            //popup.AddChild(editorContextMenu);
            //AddChild(popup);
            //GUIContextMenuManager.SetContextMenu(editor, editorContextMenu);

            popup = new GUIPopup();
            nodeContextMenu = GUI.CreateContextMenu();
            nodeContextMenu.AddItem("Destroy");
            //nodeContextMenu.AddItem("TODO rename IN/OUT ");
            nodeContextMenu.OnSelectItem.AddListener((m, a) =>
            {
                if (m.Target is GUINode node && nodeToComponent.TryGetValue(node, out LogicComponentBinder store))
                {
                    switch (a.Index)
                    {
                        case 0:
                            viewModel.DestroyComponent(store.Component);
                            break;
                        case 1:

                            //TODO RENAME
                            break;
                        case 2:
                            break;
                    }
                }
            });
            popup.Window = nodeContextMenu;
            popup.AddChild(nodeContextMenu);
            AddChild(popup);


            popup = new GUIPopup();
            wireContextMenu = GUI.CreateContextMenu();
            wireContextMenu.AddItem("Destroy");
            wireContextMenu.AddItem("Red");
            wireContextMenu.AddItem("Green");
            wireContextMenu.AddItem("Blue");
            wireContextMenu.OnSelectItem.AddListener((m, a) =>
            {
                if (m.Target is GUIWire wire && wireToConnection.TryGetValue(wire, out LogicConnectionViewModel connectionViewModel))
                {
                    switch (a.Index)
                    {
                        case 0:
                            viewModel.DestroyConnect(connectionViewModel);
                            break;
                        case 1:
                            connectionViewModel.ThemeId.Value = 0;
                            break;
                        case 2:
                            connectionViewModel.ThemeId.Value = 1;
                            break;
                        case 3:
                            connectionViewModel.ThemeId.Value = 2;
                            break;
                    }
                }
            });
            popup.Window = wireContextMenu;
            popup.AddChild(wireContextMenu);
            AddChild(popup);

            foreach (var component in viewModel.Components)
            {
                AddComponent(component);
            }
            viewModel.Components.ObserveAdd().Subscribe(e =>
            {
                AddComponent(e.Value);
            });
            viewModel.Components.ObserveRemove().Subscribe(e =>
            {
                RemoveComponent(e.Value);
            });

            foreach (var connection in viewModel.Connections)
            {
                AddConnection(connection);
            }
            viewModel.Connections.ObserveAdd().Subscribe(e =>
            {
                AddConnection(e.Value);
            });
            viewModel.Connections.ObserveRemove().Subscribe(e =>
            {
                RemoveConnection(e.Value);
            });

            viewModel.ToLocal = position =>
            {
                return ToLocal(position) - editor.Content.LocalPosition;
            };

            return null;
        }

        private void AddComponent(LogicComponentViewModel viewModel)
        {
            GUINode node = editor.CreateNode(viewModel.Name);
            LogicComponentBinder binder = new LogicComponentBinder(viewModel, node);
            GUIComponentUtils.Process(binder);
            GUIContextMenuManager.SetContextMenu(node, nodeContextMenu);

            componentToNode.Add(viewModel, binder);
            nodeToComponent.Add(node, binder);
        }

        private void RemoveComponent(LogicComponentViewModel viewModel)
        {
            if (componentToNode.Remove(viewModel, out LogicComponentBinder binder))
            {
                nodeToComponent.Remove(binder.Node);
                editor.RemoveItem(binder.Node);
            }
        }

        private void AddConnection(LogicConnectionViewModel viewModel)
        {
            GUIPin pinA = componentToNode[viewModel.Output.Component].PinMap[viewModel.Output];
            GUIPin pinB = componentToNode[viewModel.Input.Component].PinMap[viewModel.Input];

            GUIWire wire = GUINodeUtils.CreateWire(GUIUtils.GetWireStyle(ResourceDictionary.DefaultResources, viewModel.ThemeId.Value));
            wire.CreateConnection(pinA, pinB);
            wire.Anchors.AddRange(viewModel.Points);
            GUIContextMenuManager.SetContextMenu(wire, wireContextMenu);
            editor.AddItem(wire);
            connectionToWire.Add(viewModel, wire);
            wireToConnection.Add(wire, viewModel);

            viewModel.ThemeId.Subscribe(value =>
            {
                wire.Style = GUIUtils.GetWireStyle(ResourceDictionary.DefaultResources, value);
            });
            viewModel.Signal.Subscribe(value =>
            {
                wire.SetValue(GUIStyleHelpers.IsPoweredProperty, value.IsHigh);
            });
            viewModel.Points.ObserveChanged().Subscribe(e =>
            {
                wire.Anchors.Clear();
                wire.Anchors.AddRange(viewModel.Points);
            });
        }

        private void RemoveConnection(LogicConnectionViewModel viewModel)
        {
            if (connectionToWire.Remove(viewModel, out GUIWire wire))
            {
                wireToConnection.Remove(wire);
                editor.RemoveItem(wire);
            }
        }
    }
}
