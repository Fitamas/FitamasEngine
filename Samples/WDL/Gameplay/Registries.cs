using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitamas.Core;
using Fitamas.UserInterface.ViewModel;

namespace WDL.DigitalLogic
{
    public static class Registries
    {
        //public static ResourceKey<ReadonlyRegistry<ItemModel>> Item = RegistryKey<ItemModel>("item");

        public static ResourceKey<Registry<GUIWindowType>> Windows = RegistryKey<GUIWindowType>("Windows");

        private static ResourceKey<Registry<T>> RegistryKey<T>(string key)
        {
            return new ResourceKey<Registry<T>>(key);
        }
    }
}
