using Fitamas;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class LogicSimulationWindowViewModel : GUIWindowViewModel
    {
        private class ViewModelStore
        {
            public LogicComponentViewModel Component { get; }
            public Dictionary<LogicConnector, LogicConnectorViewModel> ConnectorMap { get; }

            public ViewModelStore(LogicComponentViewModel component)
            {
                Component = component;
                ConnectorMap = new Dictionary<LogicConnector, LogicConnectorViewModel>();
            }
        }

        private LogicSimulation simulation;
        private Dictionary<LogicComponent, ViewModelStore> componentsMap;
        private ObservableList<LogicComponentViewModel> components;
        private Dictionary<LogicConnection, LogicConnectionViewModel> connectionsMap;
        private ObservableList<LogicConnectionViewModel> connections;

        public LogicComponentDescription Description => simulation.Description;
        public IObservableCollection<LogicComponentViewModel> Components => components;
        public IObservableCollection<LogicConnectionViewModel> Connections => connections;
        public override GUIWindowType Type => GUIWindowTypes.SimulationWindow;

        public LogicSimulationWindowViewModel(LogicSimulation simulation)
        {
            this.simulation = simulation;

            componentsMap = new Dictionary<LogicComponent, ViewModelStore>();
            components = new ObservableList<LogicComponentViewModel>();
            connectionsMap = new Dictionary<LogicConnection, LogicConnectionViewModel>();
            connections = new ObservableList<LogicConnectionViewModel>();

            foreach (var component in simulation.Components)
            {
                Add(component);
            }
            simulation.Components.ObserveAdd().Subscribe(e =>
            {
                Add(e.Value);
            });
            simulation.Components.ObserveRemove().Subscribe(e =>
            {
                Remove(e.Value);
            });

            foreach (var component in simulation.Connections)
            {
                Add(component);
            }
            simulation.Connections.ObserveAdd().Subscribe(e =>
            {
                Add(e.Value);
            });
            simulation.Connections.ObserveRemove().Subscribe(e =>
            {
                Remove(e.Value);
            });
        }

        private void Add(LogicComponent component)
        {
            LogicComponentViewModel viewModel = new LogicComponentViewModel(component);
            ViewModelStore store = new ViewModelStore(viewModel);
            for (int i = 0; i < component.InputCount; i++)
            {
                LogicConnector connector = component.GetInputFromIndex(i);
                LogicConnectorViewModel connectorViewModel = new LogicConnectorViewModel(viewModel, connector, $"In{i}");
                viewModel.Connectors.Add(connectorViewModel);
                store.ConnectorMap.Add(connector, connectorViewModel);
            }

            for (int i = 0; i < component.OutputCount; i++)
            {
                LogicConnector connector = component.GetOutputFromIndex(i);
                LogicConnectorViewModel connectorViewModel = new LogicConnectorViewModel(viewModel, connector, $"Out{i}");
                viewModel.Connectors.Add(connectorViewModel);
                store.ConnectorMap.Add(connector, connectorViewModel);
            }

            foreach (var item in viewModel.Connectors)
            {
                if (viewModel.Description.Connectors.TryGetValue(item.Id, out string name))
                {
                    item.Name = name;
                }
            }

            components.Add(viewModel);
            componentsMap.Add(component, store);
        }

        private void Remove(LogicComponent component)
        {
            if (componentsMap.Remove(component, out ViewModelStore store))
            {
                components.Remove(store.Component);
            }
        }

        private void Add(LogicConnection connection)
        {
            LogicConnectorViewModel output = componentsMap[connection.Output.Component].ConnectorMap[connection.Output];
            LogicConnectorViewModel input = componentsMap[connection.Input.Component].ConnectorMap[connection.Input];
            LogicConnectionViewModel viewModel = new LogicConnectionViewModel(connection, output, input);
            connections.Add(viewModel);
            connectionsMap.Add(connection, viewModel);
        }

        private void Remove(LogicConnection connection)
        {
            if (connectionsMap.Remove(connection, out LogicConnectionViewModel viewModel))
            {
                connections.Remove(viewModel);
            }
        }

        public void CreateComponent(LogicComponentDescription description)
        {
            simulation.CreateComponent(description);
        }

        public void DestroyComponent(LogicComponentViewModel viewModel)
        {
            simulation.DestroyComponent(viewModel.Id);
        }

        public void CreateConnect(LogicConnectorViewModel connectorA, LogicConnectorViewModel connectorB, List<Point> points)
        {
            LogicConnectorViewModel output = null;
            LogicConnectorViewModel input = null;

            if (connectorA.IsInput && connectorB.IsOutput)
            {
                output = connectorB;
                input = connectorA;
                points.Reverse();
            }
            else if (connectorA.IsOutput && connectorB.IsInput)
            {
                output = connectorA;
                input = connectorB;
            }

            if (output != null && input != null)
            {
                LogicConnection connection = simulation.CreateConnection(output.Component.Id, output.Id, input.Component.Id, input.Id);
                connection.Points.AddRange(points);
            }
        }

        public void DestroyConnect(LogicConnectorViewModel connectorA, LogicConnectorViewModel connectorB)
        {
            if (connectorA.IsInput && connectorB.IsOutput)
            {
                simulation.DestroyConnection(connectorA.Component.Id, connectorA.Id, connectorB.Component.Id, connectorB.Id);
            }
            else if (connectorA.IsOutput && connectorB.IsInput)
            {
                simulation.DestroyConnection(connectorB.Component.Id, connectorB.Id, connectorA.Component.Id, connectorA.Id);
            }
        }

        public void DestroyConnect(LogicConnectionViewModel viewModel)
        {
            simulation.DestroyConnection(viewModel.Id);
        }
    }
}
