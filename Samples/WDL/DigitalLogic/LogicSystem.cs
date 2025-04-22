using Fitamas;
using Fitamas.Entities;
using Microsoft.Xna.Framework;
using R3;

namespace WDL.DigitalLogic
{
    public class LogicSystem : IUpdateSystem
    {
        public ReactiveProperty<LogicSimulation> Simulation { get; }
        public LogicComponentManager Manager { get; }

        public LogicSystem()
        {
            Simulation = new ReactiveProperty<LogicSimulation>();
            Manager = new LogicComponentManager();
        }

        public void Initialize(GameWorld world)
        {
            Manager.LoadAll();
        }

        public void Update(GameTime gameTime)
        {
            Simulation.Value?.Update();
        }

        public void CreateSimulation(LogicComponentDescription description)
        {
            Simulation.Value = new LogicSimulation(Manager, description);
        }

        public void Dispose()
        {

        }
    }
}
