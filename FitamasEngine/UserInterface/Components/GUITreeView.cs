using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUITreeView : GUIItemsControl
    {
        public static readonly DependencyProperty<int> IndentProperty = new DependencyProperty<int>();

        public int Indent
        {
            get
            {
                return GetValue(IndentProperty);
            }
            set 
            { 
                SetValue(IndentProperty, value); 
            }
        }

        public GUIEvent<SelectNodeArgs> OnSelectTreeNode = new GUIEvent<SelectNodeArgs>();

        public GUITreeView()
        {

        }

        protected override bool IsItemItsOwnContainerOverride(GUIComponent component)
        {
            if (component is GUITreeNode node)
            {
                node.TreeView = this;

                return true;
            }

            return false;
        }

        public GUITreeNode CreateTreeNode(string text)
        {
            GUITreeNode node = GUI.CreateTreeNode(Style.Resources, text);
            AddItem(node);
            return node;
        }

        public void Select(GUITreeNode node, ClickButtonEventArgs click)
        {
            SelectNodeArgs args = new SelectNodeArgs();
            args.Node = node;
            if (node.ParentNode != null)
            {
                args.Id = node.ParentNode.Items.IndexOf(node);
            }
            else
            {
                args.Id = node.TreeView.Items.IndexOf(node);
            }
            args.Button = click.Button;
            args.MousePosition = click.MousePosition;
            OnSelectTreeNode.Invoke(args);
        }
    }

    public class SelectNodeArgs
    {
        public GUITreeNode Node { get; set; }
        public int Id { get; set; }
        public MouseButton Button { get; set; }
        public Point MousePosition { get; set; }
    }
}
