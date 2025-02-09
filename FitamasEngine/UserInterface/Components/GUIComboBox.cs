using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Fitamas.Serializeble;
using MonoGame.Extended.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public class GUIComboBox : GUIButton
    {
        private Dictionary<GUIComponent, int> dictionary;
        private string[] items;

        public GUIVerticalGroup Group;
        public Color DefoultItemColor = Color.White;
        public Color SelectItemColor = Color.DeepSkyBlue;

        public GUIEvent<GUIComboBox, int> OnSelectItem = new GUIEvent<GUIComboBox, int>();

        public bool EnableGroup
        {
            get
            {
                return Group != null ? Group.Enable : false;
            }
            set
            {
                if (Group != null)
                {
                    Group.Enable = value;
                }
            }
        }

        public GUIComboBox(Rectangle rectangle, IEnumerable<string> items, int selectItem = 0)
        {
            LocalRectangle = rectangle;
            SetItems(items);
            SelectItem(selectItem);            
        }

        protected override void OnInit()
        {
            EnableGroup = false;
        }

        protected override void OnClickedButton(MouseEventArgs mouse)
        {
            EnableGroup = !EnableGroup;
        }

        public void SetItems<T>() where T : Enum
        {
            //TODO add enum
            //add scroll rect
        }

        public void SetItems(IEnumerable<string> items)
        {




            dictionary = new Dictionary<GUIComponent, int>();
            this.items = items.ToArray();

            if (Group == null)
            {
                return;
            }

            //Group.CellSize = Rectangle.Size;

            foreach (var child in Group.ChildrensComponent)
            {
                child.Destroy();
            }

            for (int i = 0; i < this.items.Length; i++)
            {
                GUIButton button = new GUIButton();
                button.LocalScale = Rectangle.Size;
                //button.TextBlock.Text = this.items[i];
                //button.DefoultColor = DefoultItemColor;
                //button.SelectColor = SelectItemColor;
                button.OnClicked.AddListener(SelectItem);

                Group.AddChild(button);
                dictionary[button] = i;
            }
        }

        private void SelectItem(GUIButton button)
        {
            if (dictionary.TryGetValue(button, out int index))
            {
                SelectItem(index);
            }

            EnableGroup = false;
        }

        private void SelectItem(int index)
        {
            string item = items[index];

            //TextBlock.Text = item;

            OnSelectItem?.Invoke(this, index);
        }
    }
}
