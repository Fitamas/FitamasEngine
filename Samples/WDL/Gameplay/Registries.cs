using System;
using Fitamas.Core;
using Fitamas.UserInterface.ViewModel;

namespace WDL.DigitalLogic
{
    public static class Registries
    {
        public static ResourceKey<Registry<GUIWindowType>> Windows = RegistryKey<GUIWindowType>("Windows");

        private static ResourceKey<Registry<T>> RegistryKey<T>(string key)
        {
            return new ResourceKey<Registry<T>>(key);
        }
    }
}
