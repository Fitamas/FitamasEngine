using Fitamas.Core;
using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.ImGuiNet.Assets;
using Fitamas.ImGuiNet.TreeView;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Reflection;

namespace Fitamas.ImGuiNet.Windows
{
    public class ResourcesWindow : EditorWindow
    {
        private readonly FileSystemWatcher watcher;
        private AssetTree assetTree;
        private bool treeIsDirty;
        private TreeNode<AssetData> root;
        private TreeNode<AssetData> selected;
        private TreeNode<AssetData> dragAndDrop;
        private AssetData toRename;

        private static AssetTypeAttribute[] assetTypes = ReflectionUtils.GetTypeAttributes<AssetTypeAttribute>();

        public ResourcesWindow() : base("Resources")
        {
            watcher = new FileSystemWatcher(GameEngine.Instance.Content.RootDirectory)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite |
                 NotifyFilters.CreationTime | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            watcher.Created += OnFileChanged;
            watcher.Changed += OnFileChanged;
            watcher.Renamed += OnFileChanged;
            watcher.Deleted += OnFileChanged;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            DebounceFileOperation();
        }

        private void DebounceFileOperation()
        {
            SetTreeDirty();
        }

        protected override void OnOpen()
        {
            assetTree = new AssetTree();
            root = assetTree.Root;

            SetTreeDirty();
        }

        public void SetTreeDirty()
        {
            treeIsDirty = true;
        }

        protected override void OnGUI(GameTime gameTime)
        {
            ImGui.Text("Search:");
            ImGui.SameLine();
            string input = "";
            if (ImGui.InputText("##textInput", ref input, 1000, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                //TODO search asset in tree
            }

            if (treeIsDirty)
            {
                treeIsDirty = false;
                assetTree.Clear();

                foreach (string directory in AssetDatabase.GetAllFolderPaths())
                {
                    assetTree.AddAsset(directory);
                }

                foreach (string file in AssetDatabase.GetAllAssetPaths())
                {
                    assetTree.AddAsset(file);
                }
            }

            DrawNode(root);

            if (focusedWindow)
            {
                if (ImGui.IsKeyPressed(ImGuiKey.Delete))
                {
                    if (selected != null)
                    {
                        AssetDatabase.DeleteAsset(selected.Data.fullPath);
                        selected = null;
                    }
                }
            }

            if (mouseOverWindow && !ImGui.IsAnyItemHovered() && ImGui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                selected = null;
                ImGui.OpenPopup("resourceMenu");
            }

            Popup();
        }

        protected override void OnClose()
        {
            watcher.Dispose();
        }

        private void DrawNode(TreeNode<AssetData> node)
        {
            if (node.IsRoot)
            {
                for (int i = 0; i < node.Count; i++)
                {
                    DrawNode(node[i]);
                }
            }
            else
            {
                string nodeName = $"{node.Data.path}##{node.Level}";
                bool isLeaf = node.IsLeaf;
                bool select = selected == node;
                ImGuiTreeNodeFlags flag = ImGuiTreeNodeFlags.None;
                if (isLeaf) flag |= ImGuiTreeNodeFlags.Leaf;
                if (select) flag |= ImGuiTreeNodeFlags.Selected;
                bool open = ImGui.TreeNodeEx(nodeName, flag);

                ImGui.PushID(nodeName);
                Popup(node);
                ImGui.PopID();

                //Rename
                if (toRename == node.Data)
                {
                    ImGui.SameLine();
                    ImGui.SetKeyboardFocusHere();
                    string newName = toRename.ToString();

                    if (ImGui.InputText("###rename", ref newName, 1000,
                        ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll))
                    {
                        AssetDatabase.RenameAsset(toRename.fullPath, newName);
                        toRename = null;
                    }

                    if (ImGui.IsAnyMouseDown() || ImGui.IsKeyReleased(ImGuiKey.Escape))
                    {
                        toRename = null;
                    }
                }

                //DragDrop
                if (ImGui.BeginDragDropSource())
                {
                    dragAndDrop = node;
                    string file = dragAndDrop.Data.fullPath;
                    object asset = AssetDatabase.LoadAsset(file);

                    ImGuiManager.DragDropObject = asset == null ? file : asset;

                    ImGui.SetDragDropPayload("object", IntPtr.Zero, 0);
                    ImGui.Text(nodeName);
                    ImGui.EndDragDropSource();
                }
                if (ImGui.BeginDragDropTarget())
                {
                    if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                    {
                        string oldPath = dragAndDrop.Data.fullPath;
                        string newPath = Path.Combine(node.Data.fullPath, dragAndDrop.Data.path);

                        AssetDatabase.MoveAsset(oldPath, newPath);
                        dragAndDrop = null;
                    }

                    ImGui.EndDragDropTarget();
                }

                if (ImGui.IsItemHovered())
                {
                    if (ImGui.IsMouseReleased(ImGuiMouseButton.Right))
                    {
                        selected = node;
                    }

                    if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                    {
                        selected = node;

                        ImGuiManager.SelectObject = null;
                        string file = node.Data.fullPath;

                        if (AssetDatabase.Contains(file))
                        {
                            ImGuiManager.SelectObject = AssetDatabase.LoadAsset(file);
                        }
                    }

                    if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                    {
                        //TODO open scene or file

                        //if (monoObject is SerializebleScene scene)
                        //{
                        //    EditorSystem.OpenScene = scene;
                        //}
                    }
                }

                //Draw
                if (open)
                {
                    if (!isLeaf)
                    {
                        for (int i = 0; i < node.Count; i++)
                        {
                            DrawNode(node[i]);
                        }
                    }

                    ImGui.TreePop();
                }
            }
        }

        private void Popup(TreeNode<AssetData> treeNode = null)
        {
            bool flag = treeNode != null ? ImGui.BeginPopupContextItem("resourceMenu") 
                                         : ImGui.BeginPopup("resourceMenu");

            ImGuiSelectableFlags flags = ImGuiSelectableFlags.None;

            if (treeNode == null)
            {
                flags |= ImGuiSelectableFlags.Disabled;
            }

            if (flag)
            {
                if (ImGui.Selectable("Rename", false, flags))
                {
                    toRename = treeNode.Data;
                }

                if (ImGui.Selectable("Delete", false, flags))
                {
                    AssetDatabase.DeleteAsset(selected.Data.fullPath);
                }

                string path = selected != null ? selected.Data.fullPath : "";

                if (ImGui.Selectable("Create folder"))
                {
                    AssetDatabase.CreateFolder(path, "NewFolder");
                }

                foreach (var assetType in assetTypes)
                {
                    string title = $"{assetType.Title}##{assetType.TypeId}";

                    if (ImGui.Selectable(title))
                    {
                        //string folder = selected.Data.fullPath;
                        //if (AssetDatabase.Contains(folder))
                        //{
                        //    folder = Path.GetDirectoryName(folder);
                        //}

                        string assetPath = Path.Combine(path, assetType.FileName);
                        AssetDatabase.CreateAsset(assetPath, assetType.TargetType);
                    }
                }

                ImGui.EndPopup();
            }
        }
    }
}
