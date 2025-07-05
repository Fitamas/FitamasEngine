using Fitamas.MVVM;
using Fitamas.UserInterface;
using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Microsoft.Xna.Framework;
using R3;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;

namespace WDL.Gameplay.View
{
    public static class GUIComponentUtils
    {
        public static Dictionary<LogicComponentDescription, Func<LogicComponentDescription, GUINode, GUINode>> CreatorMap;
        public static Dictionary<LogicComponentDescription, Func<LogicComponentBinder, GUINode, GUINode>> BinderMap;

        static GUIComponentUtils()
        {
            CreatorMap = new Dictionary<LogicComponentDescription, Func<LogicComponentDescription, GUINode, GUINode>>();

            CreatorMap.Add(LogicComponents.Input, (viewModel, node) =>
            {
                GUINodeItem item = GUIUtils.CreateNodeItemWithCheckBox(new Point(79, 67), GUINodeItemAlignment.Right, GUIPinType.Output);
                node.AddItem(item);
                return node;
            });
            CreatorMap.Add(LogicComponents.Output, (viewModel, node) =>
            {
                GUINodeItem item = GUIUtils.CreateNodeItemWithCheckBox(new Point(79, 67), GUINodeItemAlignment.Left, GUIPinType.Input);
                GUICheckBox checkBox = (GUICheckBox)item.Content;
                checkBox.Interacteble = false;
                checkBox.SetValue(GUIImage.ImageEffectProperty, GUIImageEffect.FlipHorizontally);
                node.AddItem(item);
                return node;
            });

            BinderMap = new Dictionary<LogicComponentDescription, Func<LogicComponentBinder, GUINode, GUINode>>();

            BinderMap.Add(LogicComponents.Input, (viewModel, node) =>
            {
                GUINodeItem item = (GUINodeItem)node.Items[0];
                LogicConnectorViewModel logicConnector = viewModel.Component.Connectors[0];
                viewModel.ConnectorMap[item.Pin] = logicConnector;
                viewModel.PinMap[logicConnector] = item.Pin;

                GUICheckBox checkBox = (GUICheckBox)item.Content;
                checkBox.OnValueChanged.AddListener((b, a) =>
                {
                    viewModel.Component.TrySetSignalValue(a);
                });
                return node;
            });

            BinderMap.Add(LogicComponents.Output, (viewModel, node) =>
            {
                GUINodeItem item = (GUINodeItem)node.Items[0];
                LogicConnectorViewModel logicConnector = viewModel.Component.Connectors[0];
                viewModel.ConnectorMap[item.Pin] = logicConnector;
                viewModel.PinMap[logicConnector] = item.Pin;

                GUICheckBox checkBox = (GUICheckBox)item.Content;
                if (viewModel.Component.TryGetSignalValue(out ReadOnlyReactiveProperty<bool> signal))
                {
                    signal.Subscribe(v =>
                    {
                        checkBox.Value = v;
                    });
                }
                return node;
            });
        }

        public static void Process(LogicComponentBinder binder)
        {
            binder.Node.LocalPosition = binder.Component.Position.Value;
            binder.Node.Style = GUIUtils.GetNodeStyle(ResourceDictionary.DefaultResources, binder.Component.ThemeId);

            if (CreatorMap.ContainsKey(binder.Component.Description))
            {
                CreatorMap[binder.Component.Description].Invoke(binder.Component.Description, binder.Node);
                BinderMap[binder.Component.Description].Invoke(binder, binder.Node);
            }
            else
            {
                foreach (var connector in binder.Component.Connectors)
                {
                    GUIPin pin = null;
                    if (connector.IsInput)
                    {
                        pin = binder.Node.CreateItem(connector.Name, GUINodeItemAlignment.Left, GUIPinType.Input).Pin;
                    }
                    else if (connector.IsOutput)
                    {
                        pin = binder.Node.CreateItem(connector.Name, GUINodeItemAlignment.Right, GUIPinType.Output).Pin;
                    }

                    if (pin != null)
                    {
                        binder.ConnectorMap[pin] = connector;
                        binder.PinMap[connector] = pin;
                    }
                }
            }
        }

        public static void Process(GUINode node, LogicComponentDescription description)
        {
            node.Style = GUIUtils.GetNodeStyle(ResourceDictionary.DefaultResources, description.ThemeId);

            if (CreatorMap.ContainsKey(description))
            {
                CreatorMap[description].Invoke(description, node);
            }
            else
            {
                foreach (var connector in description.InputConnectors)
                {
                    node.CreateItem(connector.Name, GUINodeItemAlignment.Left, GUIPinType.Input);
                }

                foreach (var connector in description.OutputConnectors)
                {
                    node.CreateItem(connector.Name, GUINodeItemAlignment.Right, GUIPinType.Output);
                }
            }
        }
    }
}
