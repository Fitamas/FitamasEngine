using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Fitamas.Serialization;
using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUITreeNode : GUIButton
    {
        private List<GUITreeNode> nodes = new List<GUITreeNode>();
        private TreeView treeView;
        private bool isOpen;
        private int level;

        public GUIImage FolderIconOpen;
        public GUIImage FolderIconClose;
        public GUIImage Icon;

        public TreeView TreeView => treeView;
        public bool IsOpen => isOpen;
        public bool IsLeaf => nodes.Count == 0;
        public int Count => nodes.Count;
        public int Level => level;

        public GUITreeNode ParentNode
        {
            get
            {
                if (Parent is GUITreeNode node)
                {
                    return node;
                }

                return null; 
            }
        }

        public GUITreeNode()
        {
            treeView = new TreeView(this);
            treeView.RecalculateTree();
        }

        public GUITreeNode this[int key]
        {
            get 
            { 
                return nodes[key]; 
            }
        }

        protected override void OnInit()
        {
            treeView?.Bind(this);
        }

        public GUITreeNode CreateNode(string text = "item")
        {
            GUITreeNode node = GUI.CreateTreeNode(text);
            //node.DefoultColor = treeView.DefoultColor;
            //node.SelectColor = treeView.SelectColor;
            //node.DisableColor = treeView.DisableColor;
            //node.DefoultTextColor = treeView.DefoultTextColor;
            //node.SelectTextColor = treeView.SelectTextColor;
            //node.DisableTextColor = treeView.DisableTextColor;

            node.FolderIconOpen.Sprite = treeView.FolderIconOpen;
            node.FolderIconClose.Sprite = treeView.FolderIconClose;
            node.Icon.Sprite = treeView.Icon;

            Add(node);
            return node;
        }

        public GUITreeNode FindChildrenWithName(string name)
        {
            //foreach (var item in nodes)
            //{
            //    if (item.TextBlock.Text == name)
            //    {
            //        return item;
            //    }
            //}

            return null;
        }

        public void Add(GUITreeNode node)
        {
            AddChild(node);
            
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
                treeView.Bind(node);
                node.treeView = treeView;
                node.level = level++;
            }

            treeView.RecalculateTree();
        }

        public void Remove(GUITreeNode node)
        {
            nodes.Remove(node);
            treeView.Unbind(node);
            node.treeView = new TreeView(node);

            treeView.RecalculateTree();
        }

        protected override void OnAddChild(GUIComponent component)
        {
            if (component is GUITreeNode node)
            {
                Add(node);
            }
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            if (component is GUITreeNode node)
            {
                Remove(node);
            }
        }

        protected override void OnDestroy()
        {
            treeView.Unbind(this);

            treeView.RecalculateTree();
        }

        protected override void OnClickedButton(MouseEventArgs mouse)
        {
            treeView.Select(mouse, this);

            if (Interacteble && mouse.Button == MouseButton.Left)
            {
                if (!IsLeaf)
                {
                    isOpen = !isOpen;

                    treeView.RecalculateTree();
                }
            }
        }
    }

    public class SelectNodeArgs
    {
        public GUITreeNode Node { get; set; }
        public int Id { get; set; }
        public MouseButton Button { get; set; }
        public Point MousePosition { get; set; }
    }

    public class TreeView
    {
        private List<GUITreeNode> allNodes = new List<GUITreeNode>();
        private GUITreeNode root;

        public int Indent = 40;
        public int NodeHeight = 40;
        public Point FolderIconSize = new Point(40, 40);
        public Point IconSize = new Point(40, 40);
        public Sprite FolderIconOpen;
        public Sprite FolderIconClose;
        public Sprite Icon;
        public Color DefoultColor = Color.White; 
        public Color SelectColor = Color.LightBlue;
        public Color DisableColor = new Color(0.8f, 0.8f, 0.8f);
        public Color DefoultTextColor = Color.Black;
        public Color SelectTextColor = Color.White;
        public Color DisableTextColor = new Color(0.4f, 0.4f, 0.4f);
        //public Sprite DefoultIcon;

        public GUIEvent<SelectNodeArgs> OnSelectTreeNode = new GUIEvent<SelectNodeArgs>();
        public GUIEvent<GUITreeNode, int> OnBindNode = new GUIEvent<GUITreeNode, int>();
        public GUIEvent<GUITreeNode, int> OnUnbindNode = new GUIEvent<GUITreeNode, int>();

        public TreeView(GUITreeNode root)
        {
            this.root = root;
        }

        public void Bind(GUITreeNode node)
        {
            if (!allNodes.Contains(node))
            {
                OnBindNode.Invoke(node, allNodes.Count);
                allNodes.Add(node);                
            }
        }

        public void Unbind(GUITreeNode node)
        {
            int index = allNodes.IndexOf(node);
            if (index != -1)
            {
                OnUnbindNode.Invoke(node, index);
            }
            allNodes.Remove(node);
        }

        public void Select(MouseEventArgs mouse, GUITreeNode node)
        {
            int index = allNodes.IndexOf(node);
            if (index != -1)
            {
                SelectNodeArgs args = new SelectNodeArgs();
                args.Node = node;
                args.Id = index;
                args.Button = mouse.Button;
                args.MousePosition = mouse.Position;
                OnSelectTreeNode.Invoke(args);
            }
        }

        public void RecalculateTree()
        {
            RecalculateNode(root, 0, 0);
        }

        private int RecalculateNode(GUITreeNode node, int yPos, int indent)
        {
            GUIImage icon;
            GUITextBlock textBlock;
            if (node.FolderIconOpen != null)
            {
                icon = node.FolderIconOpen;
                icon.LocalPosition = new Point(indent, 0);
                icon.LocalSize = FolderIconSize;
                icon.VerticalAlignment = GUIVerticalAlignment.Center;
                icon.Pivot = new Vector2(0, 0.5f);
                icon.Enable = node.IsOpen && !node.IsLeaf;
            }
            if (node.FolderIconClose != null)
            {
                icon = node.FolderIconClose;
                icon.LocalPosition = new Point(indent, 0);
                icon.LocalSize = FolderIconSize;
                icon.VerticalAlignment = GUIVerticalAlignment.Center;
                icon.Pivot = new Vector2(0, 0.5f);
                icon.Enable = !node.IsOpen && !node.IsLeaf;
            }
            if (node.Icon != null)
            {
                icon = node.Icon;
                //icon.Sprite = DefoultIcon;
                icon.LocalPosition = new Point(indent + FolderIconSize.X, 0);
                icon.LocalSize = IconSize;
                icon.VerticalAlignment = GUIVerticalAlignment.Center;
                icon.Pivot = new Vector2(0, 0.5f);
                icon.Enable = icon.Sprite != null;
            }
            //if (node.Image != null)
            //{
            //    icon = node.Image;
            //    icon.Layer = node.Layer;
            //}
            //if (node.TextBlock != null)
            //{
            //    textBlock = node.TextBlock;
            //    textBlock.Layer = node.Layer + 1;
            //    textBlock.LocalPosition = new Point(indent + FolderIconSize.X + IconSize.X, 0);
            //    textBlock.VerticalAlignment = GUIVerticalAlignment.Center;
            //    textBlock.Pivot = new Vector2(0, 0.5f);
            //}

            node.HorizontalAlignment = GUIHorizontalAlignment.Stretch;
            node.VerticalAlignment = GUIVerticalAlignment.Top;
            node.Pivot = new Vector2(0, 0);
            node.LocalPosition = new Point(0, yPos);
            node.LocalSize = new Point(0, NodeHeight);

            int nodeSize = -NodeHeight;
            for (int i = 0; i < node.Count; i++)
            {
                var child = node[i];
                child.Enable = node.IsOpen;

                if (child.Enable)
                {
                    nodeSize += RecalculateNode(child, nodeSize, indent + Indent);
                }
            }

            return nodeSize;
        }

        public GUITreeNode AddNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                return null;
            }

            GUITreeNode node = root;

            int startIndex = 0;
            int length = nodeName.Length;
            while (startIndex < length)
            {
                int endIndex = nodeName.IndexOf('\\', startIndex);
                int subLength = endIndex == -1 ? length - startIndex : endIndex - startIndex;
                string directory = nodeName.Substring(startIndex, subLength);

                //AssetData pathNode = new AssetData(directory, assetPath.Substring(0, endIndex == -1 ? length : endIndex), node.Level == 0);

                GUITreeNode child = node.FindChildrenWithName(directory);
                if (child == null)
                {
                    child = node.CreateNode(directory);
                }

                node = child;
                startIndex += subLength + 1;
            }

            return node;
        }
    }
}
