using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Fitamas.Serialization;
using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public class GUIComboBox : GUIButton
    {
        public static readonly DependencyProperty<int> SelectItemProperty = new DependencyProperty<int>(-1, false);

        public static readonly RoutedEvent OnSelectItemIndexEvent = new RoutedEvent();

        public static readonly RoutedEvent OnSelectItemEvent = new RoutedEvent();

        private GUIContextMenu contextMenu;
        private Dictionary<GUIContextItem, int> dictionary;
        private string[] items;

        public bool IsOpenMenu => contextMenu != null && contextMenu.IsActive;

        public GUIEvent<GUIComboBox, int> OnSelectItemIndex { get; }

        public GUIEvent<GUIComboBox, string> OnSelectItem { get; }

        public int SelectedItem 
        { 
            get
            {
                return GetValue(SelectItemProperty);
            }
            set
            {
                SetValue(SelectItemProperty, value);
            }
        }

        public GUIComboBox(IEnumerable<string> items)
        {
            OnSelectItemIndex = eventHandlersStore.Create<GUIComboBox, int>(OnSelectItemIndexEvent);
            OnSelectItem = eventHandlersStore.Create<GUIComboBox, string>(OnSelectItemEvent);

            SetItems(items);
        }

        protected override void OnClickedButton(MouseEventArgs mouse)
        {
            OpenMenu();

            //TODO CLOSE ON CLICK

            //if (!IsOpenMenu)
            //{
            //    OpenMenu();
            //}
            //else
            //{
            //    CloseMenu();
            //}
        }

        public void SetItems<T>() where T : Enum
        {
            //TODO add enum
        }

        public void SetItems(IEnumerable<string> items)
        {
            CloseMenu();

            this.items = items.ToArray();
        }

        public void OpenMenu()
        {
            if (!IsOpenMenu)
            {
                dictionary = new Dictionary<GUIContextItem, int>();

                GUIContextMenu menu = GUI.CreateContextMenu(Rectangle.Location);
                menu.OnSelectItem.AddListener(SelectItem);
                menu.SetFixedWidth(LocalSize.X);

                for (int i = 0; i < items.Length; i++)
                {
                    GUIContextItem contextItem = menu.AddItem(items[i]);
                    dictionary[contextItem] = i;
                }

                Root.OpenPopup(menu);
            }
        }

        public void CloseMenu()
        {
            if (IsOpenMenu)
            {
                contextMenu.OnSelectItem.RemoveListener(SelectItem);
                Root.ClosePopup();
            }
        }

        private void SelectItem(GUIContextMenu menu, GUIContextItem contextItem)
        {
            if (dictionary.TryGetValue(contextItem, out int index))
            {
                OnSelectItemIndex.Invoke(this, index);

                string item = items[index];

                OnSelectItem.Invoke(this, item);
            }
        }
    }
}
