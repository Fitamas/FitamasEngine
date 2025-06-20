using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using NLua;
using System.Linq;
using Fitamas.ECS;
using Fitamas.Extended.Entities;
using Microsoft.Xna.Framework.Content;

namespace Fitamas.Scripting
{
    public class ScriptingSystem : ILoadContentSystem
    {
        readonly FileSystemWatcher watcher = new();
        readonly string luaSrc;

        Lua lua;
        LuaFunction luaInitialize, luaLoadContent, luaUpdate, luaDraw;

        public ScriptingSystem()
        {
            //luaSrc = Path.Combine(Directory.GetCurrentDirectory(), "Scripting\\Lua");
            //ConfigureWatcher();
        }

        public void Initialize(GameWorld world)
        {
            try
            {
                lua?.Dispose();
                lua = new();
                lua.LoadCLRPackage();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void LoadContent(ContentManager content)
        {
            //string path = Path.Combine(Directory.GetCurrentDirectory(), content.RootDirectory);

            //path = Path.Combine(path, "Screens\\MainMenuScreen.lua");

            //lua.DoFile(path);
            //luaInitialize = lua["Initialize"] as LuaFunction;
            //luaLoadContent = lua["LoadContent"] as LuaFunction;
            //luaUpdate = lua["Update"] as LuaFunction;
            //luaDraw = lua["Draw"] as LuaFunction;
            //luaInitialize?.Call();
        }

        void ConfigureWatcher()
        {
            watcher.Filter = "*.*";
            watcher.Path = luaSrc;
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Created += WatcherHandler;
            watcher.Deleted += WatcherHandler;
            watcher.Renamed += WatcherHandler;
            watcher.Changed += WatcherHandler;
        }

        void WatcherHandler(object sender, FileSystemEventArgs e)
        {
            if (e.Name is null || File.GetAttributes(e.FullPath.TrimEnd('~'))
                    .HasFlag(FileAttributes.Directory))
                return;

            //forceReload = true;
        }

        public void Dispose()
        {

        }
    }
}
