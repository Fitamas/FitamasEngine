using Microsoft.Xna.Framework;
using System;
using Fitamas.Input.InputListeners;
using ObservableCollections;
using Fitamas.UserInterface.Input;
using Fitamas.Events;

namespace Fitamas.UserInterface.Components
{
    public class ComboBoxEventArgs : GUIEventArgs
    {
        public int Index { get; set; }
        public string Item { get; set; }

        public ComboBoxEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {

        }
    }

    public class GUIComboBox : GUIComponent, IMouseEvent
    {
        public static readonly DependencyProperty<int> SelectItemProperty = new DependencyProperty<int>(SelectItemChangedCallback, 0, false);

        public static readonly DependencyProperty<bool> IsDropDownOpenProperty = new DependencyProperty<bool>(IsDropDownOpenChangedCallback, false, false);

        public static readonly RoutedEvent OnSelectItemEvent = new RoutedEvent();

        public ObservableList<string> Items { get; }
        public GUIPopup Popup { get; set; }
        public MonoEvent<GUIComboBox, ComboBoxEventArgs> OnSelectItem { get; }

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
            OnSelectItem = new MonoEvent<GUIComboBox, ComboBoxEventArgs>();
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

        public void OnMovedMouse(GUIMousePositionEventArgs mouse)
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

        public void OnScrollMouse(GUIMouseWheelEventArgs mouse)
        {

        }

        private static void SelectItemChangedCallback(DependencyObject dependencyObject, DependencyProperty<int> property, int oldValue, int newValue)
        {
            if (dependencyObject is GUIComboBox comboBox)
            {
                ComboBoxEventArgs args = new ComboBoxEventArgs(OnSelectItemEvent, comboBox);

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
                comboBox.RaiseEvent(args);
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
