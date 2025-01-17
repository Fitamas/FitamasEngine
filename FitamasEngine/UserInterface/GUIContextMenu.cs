using Fitamas.Events;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Scripting;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Fitamas.UserInterface
{
    public class GUIContextMenu : GUIComponent, IMouseEvent
    {
        [SerializableField] private Point offsetToBorder;

        private static GUIContextMenu menu;

        private GUIVerticalGroup group;

        public GUIEvent OnClose = new GUIEvent();

        public Point OffsetToBorder
        {
            get
            {
                return offsetToBorder;
            }
            set
            {
                offsetToBorder = value;
                group.LocalPosition = new Point(offsetToBorder.X, -offsetToBorder.Y);
            }
        }

        public GUIVerticalGroup Group => group;

        public GUIContextMenu()
        {
            group = new GUIVerticalGroup();
            group.Pivot = new Vector2(0, 0);
            group.Anchor = new Vector2(0, 0);
            AddChild(group);

            OffsetToBorder = offsetToBorder;
            Pivot = new Vector2(0, 0);
            RaycastTarget = true;
            AutoSortingLayer = false;
            Layer = 100;
        }

        protected override void OnInit()
        {
            if (menu != null && menu.IsInHierarchy)
            {
                menu.Close();
            }

            menu = this;
        }

        public GUIButton AddItem(string name, MonoAction callback = null)
        {
            Rectangle rectangle = new Rectangle(new Point(), group.CellSize);
            GUIButton button = GUI.CreateButton(rectangle, name);
            //button.Stretch = GUIStretch.Horizontal;
            button.OnClicked.AddListener((b) => Close());
            button.OnClicked.AddListener((b) => callback?.Invoke());
            button.TextBlock.TextAligment = GUITextAligment.Left;

            AddItem(button);
            return button;
        }

        public void AddItem(GUIComponent component)
        {
            group?.AddChild(component);
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (!Contain(mouse.Position) && !group.Contain(mouse.Position))
            {
                Close();
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }

        public void Close()
        {
            OnClose.Invoke();
            Destroy();
        }
    }
}
