using Fitamas.Container;
using Fitamas.Events;
using Fitamas.UserInterface.Components;
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
        }

        public void OnOpen(GUISystem system)
        {
            lua["System"] = system;
            lua["Container"] = system.Container;
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
    }
}
