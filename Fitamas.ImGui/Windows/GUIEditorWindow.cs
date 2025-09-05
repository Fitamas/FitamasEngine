using Fitamas.ECS;
using Fitamas.UserInterface.Components;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Fitamas.ImGuiNet.Windows
{
    public class GUIEditorWindow : EditorWindow
    {
        public GUIEditorWindow()
        {
            Name = "GUI";
        }

        protected override void OnGUI(GameTime gameTime)
        {
            DrawItem(manager.Game.GUIManager.Canvas);
        }

        private bool DrawItem(GUIComponent component)
        {
            string uniqueId = component.GetHashCode().ToString();
            string nodeName = $"{component.GetType().Name} ({component.Name})##{uniqueId}";
            bool isLeaf = component.ChildrensComponent.Count() == 0;
            bool select = ImGuiManager.SelectObject == component;
            ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
            if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
            if (select) flag |= ImGuiTreeNodeFlags.Selected;

            if (ImGui.TreeNodeEx(nodeName, flag))
            {
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGuiManager.SelectObject = component;
                }

                if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                {
                    ImGuiManager.SelectObject = component;
                }

                foreach (var child in component.ChildrensComponent)
                {
                    DrawItem(child);
                }
                ImGui.TreePop();
            }

            return false;
        }
    }
}
