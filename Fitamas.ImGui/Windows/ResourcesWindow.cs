using Fitamas.DebugTools;
using Fitamas.Serialization;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.ImGuiNet.Windows
{
    public class ResourcesWindow : EditorWindow
    {
        //private AssetTree assetTree;
        //private TreeNode<AssetData> root;
        //private TreeNode<AssetData> selected;
        //private TreeNode<AssetData> dragAndDrop;
        //private AssetData toRename;

        private Type[] createAssetTypes;
        //private string[] extensions = { ".png" };

        public ResourcesWindow()
        {
            Name = "Resources";
        }

        protected override void OnOpen()
        {
            //createAssetTypes = AssemblyUtils.GetTypes<AssetMenuAttribute>();
            //assetTree = new AssetTree();
            //root = assetTree.Root;

            //editor.FileManager.OnChanged += BuildTree;

            //BuildTree();
        }

        private void BuildTree()
        {
            //assetTree.Clear();

            //foreach (string directory in AssetDatabase.GetAllFolderPaths())
            //{
            //    assetTree.AddAsset(directory);
            //}

            //foreach (string file in AssetDatabase.GetAllAssetPaths(ContentExtensionUtils.GetAllExtensions()))
            //{
            //    assetTree.AddAsset(file);
            //}
        }

        protected override void OnGUI()
        {
            ImGui.Text("Search:");
            ImGui.SameLine();
            string input = "";
            if (ImGui.InputText("##textInput", ref input, 1000, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                //TODO search asset in tree
            }

            //DrawNode(root);

            //if (ImGui.IsKeyPressed(ImGuiKey.Delete))
            //{
            //    if (selected != null)
            //    {
            //        AssetDatabase.DeleteAsset(selected.Data.fullPath);
            //        selected = null;
            //        BuildTree();
            //    }
            //}
        }

        //private void DrawNode(TreeNode<AssetData> node)
        //{
        //    if (node.IsRoot)
        //    {
        //        for (int i = 0; i < node.Count; i++)
        //        {
        //            DrawNode(node[i]);
        //        }
        //    }
        //    else
        //    {
        //        string nodeName = node.Data.ToString();

        //        bool isLeaf = node.IsLeaf;
        //        bool select = selected == node;
        //        ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
        //        if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
        //        if (select) flag |= ImGuiTreeNodeFlags.Selected;
        //        bool open = ImGui.TreeNodeEx(nodeName, flag);

        //        //Popup
        //        ImGui.PushID(nodeName);
        //        if (Popup(node.Data))
        //        {
        //            BuildTree();
        //            return;
        //        }
        //        ImGui.PopID();

        //        //Rename
        //        if (toRename == node.Data)
        //        {
        //            ImGui.SameLine();
        //            ImGui.SetKeyboardFocusHere();
        //            string newName = toRename.ToString();

        //            if (ImGui.InputText("###rename", ref newName, 1000,
        //                ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll))
        //            {
        //                AssetDatabase.RenameAsset(toRename.fullPath, newName);
        //                toRename = null;
        //                BuildTree();
        //                return;
        //            }

        //            if (ImGui.IsAnyMouseDown())
        //            {
        //                toRename = null;
        //            }
        //        }

        //        //DragDrop
        //        if (ImGui.BeginDragDropSource())
        //        {
        //            dragAndDrop = node;
        //            string file = dragAndDrop.Data.fullPath;
        //            string path = Path.GetRelativePath(EditorSystem.ObjectManager.RootDirectory, file);
        //            MonoObject monoObject = AssetDatabase.LoadAsset(path);

        //            EditorSystem.DragDropObject = monoObject == null ? file : monoObject;

        //            ImGui.SetDragDropPayload("object", IntPtr.Zero, 0);
        //            ImGui.Text(nodeName);
        //            ImGui.EndDragDropSource();
        //        }
        //        if (ImGui.BeginDragDropTarget())
        //        {
        //            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
        //            {
        //                string oldPath = dragAndDrop.Data.fullPath;
        //                string newPath = Path.Combine(node.Data.fullPath, dragAndDrop.Data.path);

        //                AssetDatabase.MoveAsset(oldPath, newPath);
        //                dragAndDrop = null;
        //                BuildTree();
        //                return;
        //            }

        //            ImGui.EndDragDropTarget();
        //        }

        //        //Draw
        //        if (open)
        //        {
        //            if (ImGui.IsItemClicked(ImGuiMouseButton.Left) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
        //            {
        //                selected = node;
        //                EditorSystem.SelectObject = null;
        //                string file = node.Data.fullPath;

        //                if (File.Exists(file))
        //                {
        //                    string path = Path.GetRelativePath(EditorSystem.ObjectManager.RootDirectory, file);
        //                    MonoObject monoObject = AssetDatabase.LoadAsset(path);
        //                    EditorSystem.SelectObject = monoObject;

        //                    if (monoObject is SerializebleScene scene)
        //                    {
        //                        EditorSystem.OpenScene = scene;
        //                    }
        //                }
        //            }

        //            //DrawNode
        //            if (!isLeaf)
        //            {
        //                for (int i = 0; i < node.Count; i++)
        //                {
        //                    DrawNode(node[i]);
        //                }
        //            }

        //            ImGui.TreePop();
        //        }
        //    }
        //}

        //private bool Popup(AssetData assetData)
        //{
        //    if (ImGui.BeginPopupContextItem("resourceMenu", ImGuiPopupFlags.MouseButtonRight))
        //    {
        //        if (ImGui.Selectable("Delete"))
        //        {
        //            AssetDatabase.DeleteAsset(assetData.fullPath);
        //            return true;
        //        }
        //        if (ImGui.Selectable("Rename"))
        //        {
        //            toRename = assetData;
        //        }

        //        ImGui.Text("Create:");

        //        if (ImGui.Selectable("Folder"))
        //        {
        //            AssetDatabase.CreateFolder(assetData.fullPath, "Folder");
        //            return true;
        //        }

        //        foreach (var item in createAssetTypes)
        //        {
        //            var attribute = item.GetCustomAttribute<AssetMenuAttribute>();
        //            string title = attribute.Title;

        //            ImGui.PushID(title);
        //            if (ImGui.Selectable(title))
        //            {
        //                string folder = assetData.fullPath;
        //                if (File.Exists(folder))
        //                {
        //                    folder = Path.GetDirectoryName(folder);
        //                }
        //                string path = Path.Combine(folder, attribute.FileName);
        //                AssetDatabase.CreateAsset(item, path);
        //                return true;
        //            }
        //        }

        //        ImGui.EndPopup();
        //    }

        //    return false;
        //}
    }
}
