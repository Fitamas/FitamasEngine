using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components
{
    public class GUIHeaderedItemsControl : GUIItemsControl
    {
        public GUIComponent Header { get; set; }

        protected override void OnAddChild(GUIComponent component)
        {
            UpdateSize();
        }

        protected override void OnChildSizeChanged(GUIComponent component)
        {
            UpdateSize();
        }

        protected virtual void UpdateSize()
        {
            Thickness thickness = Margin;

            thickness.Bottom = 0;

            if (Header != null && Header.Enable)
            {
                thickness.Bottom += Header.Rectangle.Height;
            }

            if (Container != null && Container.Enable)
            {
                thickness.Bottom += Container.Rectangle.Height;
            }

            Margin = thickness;
        }
    }
}
