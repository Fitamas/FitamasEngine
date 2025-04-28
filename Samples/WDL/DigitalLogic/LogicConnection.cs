using Microsoft.Xna.Framework;
using ObservableCollections;
using R3;
using System.Linq;

namespace WDL.DigitalLogic
{
    public class LogicConnection
    {
        public LogicConnectionData Data { get; }
        public LogicConnectorInput Input { get; }
        public LogicConnectorOutput Output { get; }
        public ReactiveProperty<int> ThemeId { get; }
        public ReactiveProperty<LogicSignal> Signal { get; }
        public ObservableList<Point> Points { get; }

        public int Id => Data.Id;

        public LogicConnection(LogicConnectionData data, LogicConnectorInput input, LogicConnectorOutput output)
        {
            Data = data;
            Input = input;
            Output = output;
            ThemeId = new ReactiveProperty<int>(Data.ThemeId);
            ThemeId.Subscribe(value =>
            {
                Data.ThemeId = value;
            });
            Signal = new ReactiveProperty<LogicSignal>();
            Points = new ObservableList<Point>();
            if (Data.Points != null)
            {
                Points.AddRange(Data.Points);
            }
            Points.ObserveChanged().Subscribe(e =>
            {
                Data.Points = Points.ToList();
            });
        }
    }
}
