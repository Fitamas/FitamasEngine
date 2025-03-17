using Fitamas.Graphics;

namespace Fitamas.UserInterface.Components
{
    public class GUITreeNode : GUIHeaderedItemsControl
    {
        public static readonly DependencyProperty<bool> IsOpenProperty = new DependencyProperty<bool>(IsOpenChangedCallback, false, false);

        public GUITreeView TreeView { get; internal set; }
        public GUITreeNode ParentNode { get; internal set; }

        public bool IsLeaf => Items.Count == 0;
        public int Count => Items.Count;

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

        protected override void UpdateSize()
        {
            base.UpdateSize();

            if (Container != null && TreeView != null)
            {
                Thickness thickness = Container.Margin;

                thickness.Left = TreeView.Indent * (Level + 1);

                Container.Margin = thickness;
            }
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
