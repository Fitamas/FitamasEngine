using Fitamas.Events;
using Fitamas.Graphics;
using Fitamas.Input;
using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUISelectNodeArgs : GUIEventArgs
    {
        public GUITreeNode Node { get; set; }
        public int Id { get; set; }
        public Point MousePosition { get; set; }

        public GUISelectNodeArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {

        }
    }

    public class GUITreeView : GUIItemsControl
    {
        public static readonly RoutedEvent OnSelectTreeNodeEvent = new RoutedEvent();

        public GUIComponent Container { get; set; }
        public MonoEvent<GUISelectNodeArgs> OnSelectTreeNode { get; }

        public GUITreeView()
        {
            OnSelectTreeNode = new MonoEvent<GUISelectNodeArgs>();

            RaycastTarget = true;
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

        protected override void OnAddItem(GUIComponent component)
        {
            Container.AddChild(component);
        }

        public GUITreeNode CreateTreeNode(string text)
        {
            GUITreeNode node = GUI.CreateTreeNode(Style.Resources, text);
            AddItem(node);
            return node;
        }

        internal void Select(GUITreeNode node)
        {
            GUISelectNodeArgs args = new GUISelectNodeArgs(OnSelectTreeNodeEvent, this);
            args.Node = node;
            if (node.ParentNode != null)
            {
                args.Id = node.ParentNode.Items.IndexOf(node);
            }
            else
            {
                args.Id = node.TreeView.Items.IndexOf(node);
            }
            OnSelectTreeNode.Invoke(args);
            RaiseEvent(args);
        }
    }
}
