using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Components
{
    public class GUIWindow : GUIComponent
    {
        public static readonly DependencyProperty<bool> IsActiveProperty = new DependencyProperty<bool>(true, false);

        public bool IsActive
        {
            get
            {
                return GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public GUIWindow()
        {
            
        }

        protected override void OnInit()
        {

        }

        public virtual void Open()
        {
            IsActive = true;
        }

        public virtual void Close()
        {
            IsActive = false;
            Destroy();
        }
    }
}
