﻿using Fitamas.Input.InputListeners;

namespace Fitamas.UserInterface.Components
{
    public class GUICheckBox : GUIButton
    {
        public static readonly DependencyProperty<bool> ValueProperty = new DependencyProperty<bool>();

        public static readonly RoutedEvent OnValueChangedEvent = new RoutedEvent();

        public GUIEvent<GUICheckBox, bool> OnValueChanged;

        public bool Value
        {
            get
            {
                return GetValue(ValueProperty);
            }
            set
            {
                if (this.Value != value)
                {
                    SetValue(ValueProperty, value);
                    OnValueChanged?.Invoke(this, value);
                }
            }
        }

        public GUICheckBox()
        {
            OnValueChanged = eventHandlersStore.Create<GUICheckBox, bool>(OnValueChangedEvent);
        }

        protected override void OnClickedButton(MouseEventArgs mouse)
        {
            Value = !Value;
        }
    }
}
