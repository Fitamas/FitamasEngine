using Fitamas.MVVM;
using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using WDL.DigitalLogic;

namespace WDL.Gameplay.ViewModel
{
    public class LogicConnectionViewModel : IViewModel
    {
        private LogicConnection connection;

        public LogicConnectorViewModel Output { get; }
        public LogicConnectorViewModel Input { get; }

        public int Id => connection.Id;
        public ReactiveProperty<int> ThemeId => connection.ThemeId;
        public ReadOnlyReactiveProperty<LogicSignal> Signal => connection.Signal;
        public ObservableList<Point> Points => connection.Points;

        public LogicConnectionViewModel(LogicConnection connection, LogicConnectorViewModel output, LogicConnectorViewModel input)
        {
            this.connection = connection;
            Output = output;
            Input = input;
        }
    }
}
