using System.Collections;

/**
 * TreeNode.cs
 * Author: Luke Holland (http://lukeholland.me/)
 */

namespace Fitamas.ImGuiNet.TreeView
{
    public class AssetTree
    {
        private TreeNode<AssetData> _root;

        public AssetTree()
        {
            _root = new TreeNode<AssetData>(null);
        }

        public TreeNode<AssetData> Root { get { return _root; } }

        public void Clear()
        {
            _root.Clear();
        }

        public void AddAsset(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return;

            TreeNode<AssetData> node = _root;

            //string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            int startIndex = 0, length = assetPath.Length;
            while (startIndex < length)
            {
                int endIndex = assetPath.IndexOf('\\', startIndex);
                int subLength = endIndex == -1 ? length - startIndex : endIndex - startIndex;
                string directory = assetPath.Substring(startIndex, subLength);

                AssetData pathNode = new AssetData(/*endIndex == -1 ? guid : null,*/ directory, assetPath.Substring(0, endIndex == -1 ? length : endIndex));

                TreeNode<AssetData> child = node.FindInChildren(pathNode);
                if (child == null) child = node.AddChild(pathNode);

                node = child;
                startIndex += subLength + 1;
            }
        }
    }

    public class AssetData
    {
        public readonly string path;
        public readonly string fullPath;

        public AssetData(string path, string fullPath)
        {
            this.path = path;
            this.fullPath = fullPath;
        }

        public override string ToString()
        {
            return path;
        }

        public override int GetHashCode()
        {
            return path.GetHashCode() + 10;
        }

        public override bool Equals(object obj)
        {
            AssetData node = obj as AssetData;
            return node != null && node.path == path;
        }
    }
}