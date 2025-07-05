using Fitamas;
using Fitamas.Events;
using Fitamas.UserInterface.ViewModel;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using WDL.DigitalLogic;
using WDL.DigitalLogic.Components;

namespace WDL.Gameplay.View
{
    public class LogicSimulationWindowViewModel : GUIWindowViewModel
    {
        private LogicSimulationViewModel simulation;

        public Func<Point, Point> ToLocal;

        public LogicComponentDescription Description => simulation.Description;
        public IObservableCollection<LogicComponentViewModel> Components => simulation.Components;
        public IObservableCollection<LogicConnectionViewModel> Connections => simulation.Connections;
        public override GUIWindowType Type => GUIWindowTypes.SimulationWindow;

        public LogicSimulationWindowViewModel(LogicSimulationViewModel simulation)
        {
            this.simulation = simulation;
        }

        public void CreateComponent(LogicComponentDescription description, Point position)
        {
            if (ToLocal != null)
            {
                position = ToLocal(position);
            }
            simulation.CreateComponent(description, position);
        }

        public void DestroyComponent(LogicComponentViewModel viewModel)
        {
            simulation.DestroyComponent(viewModel);
        }

        public void DestroySelectComponents()
        {
            foreach (var item in simulation.Components.Where(x => x.IsSelect).ToArray())
            {
                simulation.DestroyComponent(item);
            }
        }

        public void CreateConnect(LogicConnectorViewModel connectorA, LogicConnectorViewModel connectorB, List<Point> points)
        {
            simulation.CreateConnect(connectorA, connectorB, points);
        }

        public void DestroyConnect(LogicConnectorViewModel connectorA, LogicConnectorViewModel connectorB)
        {
            simulation.DestroyConnect(connectorA, connectorB);
        }

        public void DestroyConnect(LogicConnectionViewModel viewModel)
        {
            simulation.DestroyConnect(viewModel);
        }
    }

    public class LogicSimulationViewModel
    {
        private LogicSimulation simulation;
        private Dictionary<LogicComponent, LogicComponentViewModel> componentsMap;
        private ObservableList<LogicComponentViewModel> components;
        private Dictionary<LogicConnection, LogicConnectionViewModel> connectionsMap;
        private ObservableList<LogicConnectionViewModel> connections;

        public LogicComponentDescription Description => simulation.Description;
        public IObservableCollection<LogicComponentViewModel> Components => components;
        public IObservableCollection<LogicConnectionViewModel> Connections => connections;

        public LogicSimulationViewModel(LogicSimulation simulation)
        {
            this.simulation = simulation;

            componentsMap = new Dictionary<LogicComponent, LogicComponentViewModel>();
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
            components.Add(viewModel);
            componentsMap.Add(component, viewModel);
        }

        private void Remove(LogicComponent component)
        {
            if (componentsMap.Remove(component, out LogicComponentViewModel viewModel))
            {
                components.Remove(viewModel);
            }
        }

        private void Add(LogicConnection connection)
        {
            LogicConnectorViewModel output = componentsMap[connection.Output.Component].GetConnector(connection.Output.Id);
            LogicConnectorViewModel input = componentsMap[connection.Input.Component].GetConnector(connection.Input.Id);
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

        public void CreateComponent(LogicComponentDescription description, Point position)
        {
            simulation.CreateComponent(description, position);
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
