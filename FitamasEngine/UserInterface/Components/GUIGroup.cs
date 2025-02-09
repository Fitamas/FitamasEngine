using Fitamas.Serializeble;
using Fitamas.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Fitamas.UserInterface.Components
{
    public abstract class GUIGroup : GUIComponent
    {
        protected int horizontalMaxCount;
        protected int verticalMaxCount;

        public Point CellSize;
        public Point Spacing;

        public GUIGroup(int horizontalMaxCount = -1, int verticalMaxCount = -1)
        {
            this.horizontalMaxCount = horizontalMaxCount;
            this.verticalMaxCount = verticalMaxCount;
        }

        protected override void OnAddChild(GUIComponent component)
        {
            CalculateElementPosition();
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            CalculateElementPosition();
        }

        protected void CalculateElementPosition()
        {
            Point scale = GetGroupScale();
            Point offset = new Point(CellSize.X / 2, -CellSize.Y / 2);
            int i = 0;

            foreach (var component in ChildrensComponent)
            {
                Point position = new Point(i % scale.X, -i / scale.X) * (CellSize + Spacing);

                component.LocalPosition = position + offset;

                i++;
            }

            LocalScale = GetGroupSize();
        }

        public Point GetGroupScale()
        {
            int count = ChildrensComponent.Count();
            Point maxCount = new Point();

            if (horizontalMaxCount > 0)
            {
                maxCount.X = horizontalMaxCount;
                maxCount.Y = count / horizontalMaxCount + (count % horizontalMaxCount > 0 ? 1 : 0);
            }
            else if (verticalMaxCount > 0)
            {
                maxCount.X = count / verticalMaxCount + (count % verticalMaxCount > 0 ? 1 : 0);
                maxCount.Y = verticalMaxCount;
            }

            return maxCount;
        }

        public Point GetGroupSize()
        {
            return GetGroupScale() * (CellSize + Spacing) - Spacing;
        }
    }
}
