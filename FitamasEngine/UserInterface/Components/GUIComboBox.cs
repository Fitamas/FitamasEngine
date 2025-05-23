﻿using Microsoft.Xna.Framework;
using System;
using Fitamas.Input.InputListeners;
using ObservableCollections;
using Fitamas.Input;
using Fitamas.UserInterface.Input;

namespace Fitamas.UserInterface.Components
{
    public class ComboBoxEventArgs
    {
        public int Index { get; set; }
        public string Item { get; set; }
    }

    public class GUIComboBox : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<int> SelectItemProperty = new DependencyProperty<int>(SelectItemChangedCallback, 0, false);

        public static readonly DependencyProperty<bool> IsDropDownOpenProperty = new DependencyProperty<bool>(IsDropDownOpenChangedCallback, false, false);

        public static readonly RoutedEvent OnSelectItemEvent = new RoutedEvent();

        public ObservableList<string> Items { get; }
        public GUIPopup Popup { get; set; }
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

        public bool IsDropDownOpen
        {
            get
            {
                return GetValue(IsDropDownOpenProperty);
            }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }

        public GUIComboBox()
        {
            Items = new ObservableList<string>();
            OnSelectItem = eventHandlersStore.Create<GUIComboBox, ComboBoxEventArgs>(OnSelectItemEvent);
            RaycastTarget = true;
        }

        protected override void OnFocus()
        {
            IsDropDownOpen = true;
        }

        protected override void OnUnfocus()
        {
            IsDropDownOpen = false;
        }

        public void AddItemsFromEnum<T>() where T : struct, Enum
        {
            var names = Enum.GetNames<T>();
            Items.AddRange(names);
        }

        internal void SelectItem(GUIContextMenu menu, GUISelectContextItemEventArgs args)
        {
            SelectedItem = args.Index;
        }

        public void OnMovedMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnClickedMouse(GUIMouseEventArgs mouse)
        {

        }

        public void OnReleaseMouse(GUIMouseEventArgs mouse)
        {
            if (IsMouseOver)
            {
                if (Interacteble && mouse.Button == MouseButton.Left)
                {
                    if (IsFocused)
                    {
                        IsDropDownOpen = !IsDropDownOpen;
                    }
                    else
                    {
                        Focus();
                    }
                }
            }
        }

        public void OnScrollMouse(GUIMouseEventArgs mouse)
        {

        }

        private static void SelectItemChangedCallback(DependencyObject dependencyObject, DependencyProperty<int> property, int oldValue, int newValue)
        {
            if (dependencyObject is GUIComboBox comboBox)
            {
                ComboBoxEventArgs args = new ComboBoxEventArgs();

                if (newValue >= 0 && comboBox.Items.Count > newValue)
                {
                    args.Index = newValue;
                    args.Item = comboBox.Items[newValue];
                }
                else
                {
                    args.Index = -1;
                    args.Item = "";
                }

                comboBox.OnSelectItem.Invoke(comboBox, args);
            }
        }

        private static void IsDropDownOpenChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (dependencyObject is GUIComboBox comboBox)
            {
                if (comboBox.Popup != null)
                {
                    comboBox.Popup.IsOpen = newValue;
                }
            }
        }
    }
}
