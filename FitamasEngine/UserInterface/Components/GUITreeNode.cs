using Fitamas.Graphics;
using Newtonsoft.Json.Linq;

namespace Fitamas.UserInterface.Components
{
    public class GUITreeNode : GUIHeaderedItemsControl
    {
        public static readonly DependencyProperty<bool> IsOpenProperty = new DependencyProperty<bool>(IsOpenChangedCallback, false, false);

        public static readonly DependencyProperty<bool> IsLeafProperty = new DependencyProperty<bool>(false, false);

        public static readonly DependencyProperty<bool> IsSelectedProperty = new DependencyProperty<bool>(false, false);

        public GUITreeView TreeView { get; internal set; }
        public GUITreeNode ParentNode { get; internal set; }

        public bool IsOpen
        {
            get
            {
                return GetValue(IsOpenProperty);
            }
            set
            {
                SetValue(IsOpenProperty, value);
            }
        }

        public bool IsLeaf
        {
            get
            {
                return GetValue(IsLeafProperty);
            }
            private set
            {
                SetValue(IsLeafProperty, value);
            }
        }

        public bool IsSelect
        {
            get
            {
                return GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        public int Level 
        { 
            get
            {
                return ParentNode != null ? ParentNode.Level + 1 : 0;
            }
        }

        public GUITreeNode()
        {

        }

        protected override void OnInit()
        {
            Container.Enable = IsOpen;
            UpdateSize();
        }

        protected override bool IsItemItsOwnContainerOverride(GUIComponent component)
        {
            if (component is GUITreeNode node)
            {
                node.ParentNode = this;
                node.TreeView = TreeView;

                return true;
            }

            return false;
        }

        protected override void OnAddItem(GUIComponent component)
        {
            base.OnAddItem(component);

            IsLeaf = Items.Count == 0;
        }

        protected override void OnRemoveItem(GUIComponent component)
        {
            base.OnRemoveItem(component);

            IsLeaf = Items.Count == 0;
        }

        public GUITreeNode CreateTreeNode(string text)
        {
            GUITreeNode node = GUI.CreateTreeNode(Style, text);
            AddItem(node);
            return node;
        }

        private static void IsOpenChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (dependencyObject is GUITreeNode node)
            {
                node.Container.Enable = newValue;
                node.UpdateSize();
            }
        }
    }
}
