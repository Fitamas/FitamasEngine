using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Fitamas.Serialization;
using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public class ComboBoxEventArgs
    {
        public int Index { get; set; }
        public string Item { get; set; }
    }

    public class GUIComboBox : GUIButton
    {
        public static readonly DependencyProperty<int> SelectItemProperty = new DependencyProperty<int>(SelectItemChangedCallback, 0, false);

        public static readonly RoutedEvent OnSelectItemEvent = new RoutedEvent();

        private GUIContextMenu contextMenu;
        private Dictionary<GUIContextItem, int> dictionary;
        private string[] items;

        public bool IsOpenMenu => contextMenu != null && contextMenu.IsActive;

        public GUIEvent<GUIComboBox, ComboBoxEventArgs> OnSelectItem { get; }

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

        public GUIComboBox()
        {
            OnSelectItem = eventHandlersStore.Create<GUIComboBox, ComboBoxEventArgs>(OnSelectItemEvent);
        }

        protected override void OnClickedButton(MouseEventArgs mouse)
        {
            OpenMenu();
        }

        public void SetItems<T>() where T : struct, Enum
        {
            var names = Enum.GetNames<T>();
            SetItems(names);
        }

        public void SetItems(IEnumerable<string> items)
        {
            if (items == null)
            {
                return;
            }

            CloseMenu();
            this.items = items.ToArray();
        }

        public void OpenMenu()
        {
            if (!IsOpenMenu)
            {
                dictionary = new Dictionary<GUIContextItem, int>();

                GUIContextMenu menu = GUI.CreateContextMenu(new Point(Rectangle.Left, Rectangle.Bottom));
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
                SelectedItem = index;
            }
        }

        private static void SelectItemChangedCallback(DependencyObject dependencyObject, DependencyProperty<int> property, int oldValue, int newValue)
        {
            if (dependencyObject is GUIComboBox comboBox)
            {
                ComboBoxEventArgs args = new ComboBoxEventArgs();

                if (newValue >= 0 && comboBox.items.Length > newValue)
                {
                    args.Index = newValue;
                    args.Item = comboBox.items[newValue];
                }
                else
                {
                    args.Index = -1;
                    args.Item = "";
                }

                comboBox.OnSelectItem.Invoke(comboBox, args);
            }
        }
    }
}
