using Fitamas.Events;
using NLua;
using System;
using System.IO;

namespace Fitamas.UserInterface.Scripting
{
    public class GUIScripting
    {
        private Lua lua;
        private LuaFunction onOpen;
        private LuaFunction update;
        private LuaFunction onClose;

        public GUIScripting(string name) 
        {
            try
            {
                lua?.Dispose();
                lua = new();
                lua.LoadCLRPackage();

                string path = Path.Combine(Directory.GetCurrentDirectory(), name);

                lua.DoFile(path);
                onOpen = lua["OnOpen"] as LuaFunction;
                update = lua["Update"] as LuaFunction;
                onClose = lua["OnClose"] as LuaFunction;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            //lua.DoString(@" import ('WDL') "); TODO import dll in game
        }

        public void OnOpen(GUISystem system)
        {
            lua["system"] = system;
            onOpen?.Call();
        }

        public void Update()
        {
            update?.Call();
        }

        public void OnClose()
        {
            onClose?.Call();
        }

        public MonoAction CreateAction(string name)
        {
            if (!string.IsNullOrEmpty(name) && lua != null)
            {
                LuaFunction function = lua[name] as LuaFunction;

                return () => { function?.Call(); };
            }

            return null;
        }

        public MonoAction<T0> CreateAction<T0>(string name)
        {
            if (!string.IsNullOrEmpty(name) && lua != null)
            {
                LuaFunction function = lua[name] as LuaFunction;

                return (arg1) => { function?.Call(arg1); };
            }

            return null;
        }

        public MonoAction<T0, T1> CreateAction<T0, T1>(string name)
        {
            if (!string.IsNullOrEmpty(name) && lua != null)
            {
                LuaFunction function = lua[name] as LuaFunction;

                return (arg0, arg1) => { function?.Call(arg0, arg1); };
            }

            return null;
        }
    }
}
