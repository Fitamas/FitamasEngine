using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.TreeView;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace Fitamas.ImGuiNet
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class MenuItemAttribute : Attribute
    {
        public string Path { get; }
        public ImGuiKey Modifier { get; set; }
        public ImGuiKey Hotkey { get; set; }
        public string Tooltip { get; set; }

        public MenuItemAttribute(string path, string tooltip = "")
        {
            Path = path;
            Tooltip = tooltip;
        }

        public MenuItemAttribute(string path, ImGuiKey modifier, ImGuiKey hotkey , string tooltip = "")
        {
            Path = path;
            Modifier = modifier;
            Hotkey = hotkey;
            Tooltip = tooltip;
        }
    }

    public static class ImGuiMainMenuBar
    {
        private class MenuItem
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public MethodInfo Method { get; set; }
            public ImGuiKey Modifier { get; set; }
            public ImGuiKey Hotkey { get; set; }
            public string Tooltip { get; set; }

            public override string ToString()
            {
                return Path;
            }

            public override int GetHashCode()
            {
                return Path.GetHashCode() + 10;
            }

            public override bool Equals(object obj)
            {
                MenuItem node = obj as MenuItem;
                return node != null && node.Path == Path;
            }
        }

        private static TreeNode<MenuItem> root = new TreeNode<MenuItem>(new MenuItem());

        static ImGuiMainMenuBar()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            var attributes = method.GetCustomAttributes<MenuItemAttribute>();
                            foreach (var attr in attributes)
                            {
                                if (!method.IsStatic)
                                {
                                    Debug.LogError($"Method {method.Name} must be static to use MenuItemAttribute");
                                    continue;
                                }

                                if (method.ReturnType != typeof(void) || method.GetParameters().Length > 0)
                                {
                                    Debug.LogError($"Method {method.Name} must be void and without parameters");
                                    continue;
                                }

                                var menuItem = CreateItem(attr.Path);
                                menuItem.Method = method;
                                menuItem.Modifier = attr.Modifier;
                                menuItem.Hotkey = attr.Hotkey;
                                menuItem.Tooltip = attr.Tooltip;
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogExeption(ex);
                }
            }
        }

        private static MenuItem CreateItem(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return null;
            }

            TreeNode<MenuItem> node = root;
            int startIndex = 0, length = assetPath.Length;
            MenuItem item = null;
            while (startIndex < length)
            {
                int endIndex = assetPath.IndexOf('/', startIndex);
                int subLength = endIndex == -1 ? length - startIndex : endIndex - startIndex;
                string directory = assetPath.Substring(startIndex, subLength);

                item = new MenuItem()
                {
                    Name = directory,
                    Path = assetPath.Substring(0, endIndex == -1 ? length : endIndex)
                };

                TreeNode<MenuItem> child = node.FindInChildren(item);
                if (child == null) child = node.AddChild(item);

                node = child;
                startIndex += subLength + 1;
            }

            return item;
        }

        private static void Draw(TreeNode<MenuItem> node)
        {
            if (node.IsLeaf)
            {
                string shortcut = "";
                if (node.Data.Modifier != ImGuiKey.None && node.Data.Hotkey != ImGuiKey.None)
                {
                    shortcut = $"{node.Data.Modifier.ToString()}+{node.Data.Hotkey.ToString()}";
                }

                if (ImGui.MenuItem(node.Data.Name, shortcut))
                {
                    node.Data.Method.Invoke(null, null);
                }

                if (!string.IsNullOrEmpty(node.Data.Tooltip) && ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip(node.Data.Tooltip);
                }
            }
            else
            {
                if (ImGui.BeginMenu(node.Data.Name))
                {
                    for (int i = 0; i < node.Count; i++)
                    {
                        Draw(node[i]);
                    }

                    ImGui.EndMenu();
                }
            }
        }

        public static void Draw()
        {
            if (ImGui.BeginMainMenuBar())
            {
                for (int i = 0; i < root.Count; i++)
                {
                    Draw(root[i]);
                }

                ImGui.EndMainMenuBar();
            }

            for (int i = 0; i < root.Count; i++)
            {
                HandleGlobalShortcuts(root[i]);
            }
        }

        private static void HandleGlobalShortcuts(TreeNode<MenuItem> node)
        {
            if (node.IsLeaf)
            {
                if (node.Data.Modifier != ImGuiKey.None && node.Data.Hotkey != ImGuiKey.None)
                {
                    if (ImGuiUtils.IsKeyPressedWithModifier(node.Data.Modifier, node.Data.Hotkey))
                    {
                        node.Data.Method.Invoke(null, null);
                    }
                }
            }
            else
            {
                for (int i = 0; i < node.Count; i++)
                {
                    HandleGlobalShortcuts(node[i]);
                }
            }
        }
    }
}
