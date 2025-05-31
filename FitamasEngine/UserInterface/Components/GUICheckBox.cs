using Fitamas.Events;
using Fitamas.Input.InputListeners;
using Fitamas.Math2D;

namespace Fitamas.UserInterface.Components
{
    public class GUICheckBox : GUIButton
    {
        public static readonly DependencyProperty<bool> ValueProperty = new DependencyProperty<bool>(ValueChangedCallback, false, false);

        public static readonly RoutedEvent OnValueChangedEvent = new RoutedEvent();

        public MonoEvent<GUICheckBox, bool> OnValueChanged;

        public bool Value
        {
            get
            {
                return GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public GUICheckBox()
        {
            OnValueChanged = eventHandlersStore.Create<GUICheckBox, bool>(OnValueChangedEvent);
        }

        protected override void OnClickedButton()
        {
            Value = !Value;
        }

        private static void ValueChangedCallback(DependencyObject dependencyObject, DependencyProperty<bool> property, bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
            {
                if (dependencyObject is GUICheckBox checkBox)
                {
                    checkBox.OnValueChanged.Invoke(checkBox, newValue);
                }
            }
        }
    }
}
