using Fitamas.ECS;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet.Windows
{
    public class HierarchyWindow : EditorWindow
    {
        private Entity selected;
        private Entity dragDrop;
        private Entity rename;

        public HierarchyWindow()
        {
            Name = "Entities";
        }

        protected override void OnGUI(GameTime gameTime)
        {
            if (ImGui.CollapsingHeader("Hierarchy"))
            {
                EntityManager entityManager = manager.Game.GameWorld.EntityManager;

                foreach (var id in entityManager.Entities)
                {
                    Entity entity = entityManager.Get(id);
                    DrawItem(entity);
                }

                Popup();
            }
        }

        private void DrawItem(Entity entity)
        {
            string nodeName = $"{entity.Name}##{entity.Id}";
            bool isLeaf = true;// node.IsLeaf;
            bool select = (Entity)ImGuiManager.SelectObject == entity;
            ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
            if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
            if (select) flag |= ImGuiTreeNodeFlags.Selected;
            bool open = ImGui.TreeNodeEx(nodeName, flag);

            //Rename
            if (rename == entity)
            {
                ImGui.SameLine();
                ImGui.SetKeyboardFocusHere();
                string newName = rename.Name;

                if (ImGui.InputText("###rename", ref newName, 1000,
                    ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll))
                {
                    rename.Name = newName;
                    rename = null;
                }

                if (ImGui.IsAnyMouseDown())
                {
                    rename = null;
                }
            }

            //DragDrop
            if (ImGui.BeginDragDropSource())
            {
                ImGuiManager.DragDropObject = entity;

                ImGui.SetDragDropPayload("object", IntPtr.Zero, 0);
                ImGui.Text(nodeName);
                ImGui.EndDragDropSource();
            }
            if (ImGui.BeginDragDropTarget())
            {
                if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                {
                    Debug.Log("TODO"); //TODO

                    //if (ImGuiManager.SelectObject is Prefab prefab)
                    //{
                    //    GameObject gameObject1 = PrefabUtility.ConvertToGameObject(prefab);
                    //    EditorSystem.OpenScene.GameObjects.Add(gameObject1);
                    //    return true;
                    //}
                }

                ImGui.EndDragDropTarget();
            }

            //Draw
            if (open)
            {
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGuiManager.SelectObject = entity;
                }

                if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
                {
                    ImGuiManager.SelectObject = entity;
                }

                ImGui.TreePop();

                //DrawNode
                //if (!isLeaf)
                //{
                //    for (int i = 0; i < node.Count; i++)
                //    {
                //        DrawNode(node[i]);
                //    }
                //}
            }

            ImGui.OpenPopupOnItemClick("selectEntity");

        }

        private void Popup()
        {
            if (ImGui.BeginPopup("selectEntity"))
            {
                if (ImGui.Selectable("Rename entity"))
                {
                    rename = selected;
                }
                if (ImGui.Selectable("Delete entity"))
                {
                    EntityManager entityManager = manager.Game.GameWorld.EntityManager;
                    entityManager.InstantDestroy(selected);
                }
                if (ImGui.Selectable("New entity"))
                {
                    EntityManager entityManager = manager.Game.GameWorld.EntityManager;
                    selected = entityManager.Create();
                    ImGuiManager.SelectObject = selected;
                }
                if (ImGui.Selectable("Convert to prefab"))
                {
                    //TODO
                    //PrefabUtility.CreatePrefab("GameObject.prefab", selected);
                }

                ImGui.EndPopup();
            }
        }
    }
}
