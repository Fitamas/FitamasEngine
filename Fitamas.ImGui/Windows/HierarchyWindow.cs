using Fitamas.ECS;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ImGuiNet.Windows
{
    public class HierarchyWindow : EditorWindow
    {
        //private GameObject selected;
        //private GameObject dragDrop;
        //private GameObject rename;

        public HierarchyWindow()
        {
            Name = "Entities";
        }

        protected override void OnGUI()
        {
            if (ImGui.CollapsingHeader("Hierarchy"))
            {
                //if (EditorSystem.RuntimeMode)
                {
                    EntityManager entityManager = manager.Game.GameWorld.EntityManager;

                    foreach (var id in entityManager.Entities)
                    {
                        Entity entity = entityManager.Get(id);
                        DrawItem(entity);
                    }
                }
                //else
                //{
                //    foreach (var gameObject in EditorSystem.OpenScene.GameObjects)
                //    {
                //        if (DrawItem(gameObject))
                //        {
                //            break;
                //        }
                //    }

                //    Popup();
                //}
            }
        }

        private bool DrawItem(Entity entity)
        {
            string nodeName = $"{entity.Name}##{entity.Id}";
            bool isLeaf = true;// node.IsLeaf;
            bool select = (Entity)ImGuiManager.SelectObject == entity;
            ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
            if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
            if (select) flag |= ImGuiTreeNodeFlags.Selected;

            if (ImGui.TreeNodeEx(nodeName, flag))
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
            }

            return false;
        }

        //private bool DrawItem(GameObject gameObject)
        //{
        //    string nodeName = gameObject.Name;
        //    bool isLeaf = true;// node.IsLeaf;
        //    bool select = EditorSystem.SelectObject == gameObject;
        //    ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
        //    if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
        //    if (select) flag |= ImGuiTreeNodeFlags.Selected;
        //    bool open = ImGui.TreeNodeEx(nodeName, flag);

        //    //Rename
        //    if (rename == gameObject)
        //    {
        //        ImGui.SameLine();
        //        ImGui.SetKeyboardFocusHere();
        //        string newName = rename.Name;

        //        if (ImGui.InputText("###rename", ref newName, 1000,
        //            ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll))
        //        {
        //            rename.Name = newName;
        //            rename = null;
        //        }

        //        if (ImGui.IsAnyMouseDown())
        //        {
        //            rename = null;
        //        }
        //    }

        //    //DragDrop
        //    if (ImGui.BeginDragDropSource())
        //    {
        //        EditorSystem.DragDropObject = gameObject;

        //        ImGui.SetDragDropPayload("object", IntPtr.Zero, 0);
        //        ImGui.Text(nodeName);
        //        ImGui.EndDragDropSource();
        //    }
        //    if (ImGui.BeginDragDropTarget())
        //    {
        //        if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
        //        {
        //            if (EditorSystem.SelectObject is Prefab prefab)
        //            {
        //                GameObject gameObject1 = PrefabUtility.ConvertToGameObject(prefab);

        //                EditorSystem.OpenScene.GameObjects.Add(gameObject1);

        //                return true;
        //            }
        //        }

        //        ImGui.EndDragDropTarget();
        //    }

        //    //Draw
        //    if (open)
        //    {
        //        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
        //        {
        //            selected = gameObject;
        //            EditorSystem.SelectObject = gameObject;
        //        }

        //        if (ImGui.IsItemClicked(ImGuiMouseButton.Left))
        //        {
        //            selected = gameObject;
        //            EditorSystem.SelectObject = gameObject;
        //        }

        //        //DrawNode
        //        //if (!isLeaf)
        //        //{
        //        //    for (int i = 0; i < node.Count; i++)
        //        //    {
        //        //        DrawNode(node[i]);
        //        //    }
        //        //}

        //        ImGui.TreePop();
        //    }

        //    ImGui.OpenPopupOnItemClick("selectEntity");

        //    return false;
        //}

        private void Popup()
        {
            if (ImGui.BeginPopup("selectEntity"))
            {
                //if (ImGui.Selectable("Rename entity"))
                //{
                //    rename = selected;
                //}
                //if (ImGui.Selectable("Delete entity"))
                //{
                //    EditorSystem.OpenScene.GameObjects.Remove(selected);
                //}
                //if (ImGui.Selectable("New entity"))
                //{
                //    selected = new GameObject();
                //    EditorSystem.SelectObject = selected;
                //}
                //if (ImGui.Selectable("Convert to prefab"))
                //{
                //    PrefabUtility.CreatePrefab("GameObject.prefab", selected);
                //}

                ImGui.EndPopup();
            }
        }
    }
}
