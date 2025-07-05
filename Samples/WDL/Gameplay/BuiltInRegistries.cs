using System;
using Fitamas.Core;
using Fitamas.UserInterface.ViewModel;

namespace WDL.DigitalLogic
{
    public static class BuiltInRegistries
    {
        private static WritableRegistry<Registry> registry = new WritableRegistry<Registry>();

        public static Registry<GUIWindowType> Windows = CreateRegister(Registries.Windows);

        private static Registry<T> CreateRegister<T>(ResourceKey<Registry<T>> resource)
        {
            return (Registry<T>)Registry.Register(registry, resource.TypeId, new WritableRegistry<T>());
        }
    }
}
