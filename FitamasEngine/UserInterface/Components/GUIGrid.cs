using Microsoft.Xna.Framework;
using System.Data;

namespace Fitamas.UserInterface.Components
{
    public enum GUIGridGroupStartCorner
    {
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
    }

    public enum GUIGridGroupConstraint
    {
        Flexible,
        FixedColumnCount,
        FixedRowCount,
    }

    public class GUIGrid : GUIGroup
    {
        public static readonly DependencyProperty<Point> CellSizeProperty = new DependencyProperty<Point>(Point.Zero);

        public static readonly DependencyProperty<Point> SpacingProperty = new DependencyProperty<Point>(Point.Zero);

        public static readonly DependencyProperty<GUIGridGroupStartCorner> StartCornerProperty = new DependencyProperty<GUIGridGroupStartCorner>(GUIGridGroupStartCorner.LeftTop);

        public static readonly DependencyProperty<GUIGridGroupConstraint> ConstraintProperty = new DependencyProperty<GUIGridGroupConstraint>(GUIGridGroupConstraint.Flexible);

        public static readonly DependencyProperty<int> CountProperty = new DependencyProperty<int>(0);

        public Point CellSize
        {
            get
            {
                return GetValue(CellSizeProperty);
            }
            set
            {
                SetValue(CellSizeProperty, value);
            }
        }

        public Point Spacing
        {
            get
            {
                return GetValue(SpacingProperty);
            }
            set
            {
                SetValue(SpacingProperty, value);
            }
        }

        public GUIGridGroupStartCorner StartCorner
        {
            get
            {
                return GetValue(StartCornerProperty);
            }
            set
            {
                SetValue(StartCornerProperty, value);
            }
        }

        public GUIGridGroupConstraint Constraint
        {
            get
            {
                return GetValue(ConstraintProperty);
            }
            set
            {
                SetValue(ConstraintProperty, value);
            }
        }

        public int Count
        {
            get
            {
                return GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
            }
        }

        protected override Point CalculateSize(GUIComponent[] components, Point size)
        {
            if (Constraint == GUIGridGroupConstraint.Flexible)
            {
                return size;
            }
            else
            {
                return (GetCount(size) + new Point(1, 1)) * (CellSize + Spacing);
            }
        }

        protected override void CalculateComponents(GUIComponent[] components)
        {
            Rectangle rectangle = AvailableRectangle(this);
            Point cellSize = CellSize;
            Point spacing = Spacing;
            GUIGridGroupStartCorner startCorner = StartCorner;
            GUIGroupOrientation orientation = Orientation;
            Point position;
            Point direction;
            switch (startCorner)
            {
                case GUIGridGroupStartCorner.LeftTop:
                    position = Point.Zero;
                    direction = new Point(1, 1);
                    break;
                case GUIGridGroupStartCorner.LeftBottom:
                    position = new Point(0, -rectangle.Height);
                    direction = new Point(1, -1);
                    break;
                case GUIGridGroupStartCorner.RightTop:
                    position = new Point(rectangle.Width, 0);
                    direction = new Point(-1, 1);
                    break;
                default:
                    position = new Point(rectangle.Width, -rectangle.Height);
                    direction = new Point(-1, -1);
                    break;
            }

            Point count = GetCount(rectangle.Size);
            Point index = Point.Zero;

            foreach (var component in components)
            {
                Vector2 pivot = component.Pivot;
                Point localPosition = new Point((int)(cellSize.X * pivot.X), (int)(cellSize.Y * pivot.Y));
                component.LocalSize = CellSize;
                component.LocalPosition = (index * (cellSize + spacing) + localPosition) * direction + position;

                if (orientation == GUIGroupOrientation.Horizontal)
                {
                    index.X += 1;

                    if (count.X > 0 && count.X <= index.X)
                    {
                        index.X = 0;
                        index.Y += 1;
                    }
                }
                else
                {
                    index.Y += 1;

                    if (count.Y > 0 && count.Y <= index.Y)
                    {
                        index.Y = 0;
                        index.X += 1;
                    }
                }
            }
        }

        public Point GetCount(Point size)
        {
            Point cellSize = CellSize;
            Point spacing = Spacing;
            GUIGroupOrientation orientation = Orientation;
            GUIGridGroupConstraint constraint = Constraint;

            Point count = Point.Zero;
            switch (constraint)
            {
                case GUIGridGroupConstraint.Flexible:
                    if (orientation == GUIGroupOrientation.Horizontal)
                    {
                        int l = cellSize.X + spacing.X;
                        count.X = l > 0 ? size.X/ l : 1;
                    }
                    else
                    {
                        int l = cellSize.Y + spacing.Y;
                        count.Y = l > 0 ? size.Y / l : 1;
                    }
                    break;
                case GUIGridGroupConstraint.FixedColumnCount:
                    count.X = Count;
                    break;
                case GUIGridGroupConstraint.FixedRowCount:
                    count.Y = Count;
                    break;
            }

            return count;
        }
    }
}
