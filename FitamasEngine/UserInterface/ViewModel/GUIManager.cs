using Fitamas.Container;

namespace Fitamas.UserInterface.ViewModel
{
    public abstract class GUIManager
    {
        public DIContainer Container { get; }

        protected GUIManager(DIContainer container)
        {
            Container = container;
        }
    }
}