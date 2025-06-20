using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public class GUIHeaderedContentControl : GUIComponent
    {
        public GUIComponent Header { get; set; }
        public GUIComponent Content { get; set; }

        protected override void OnChildPropertyChanged(GUIComponent component, DependencyProperty property)
        {
            base.OnChildPropertyChanged(component, property);

            if (property == MarginProperty)
            {
                Point size0 = Header.LocalSize;
                Point size1 = Content.LocalSize;
                LocalSize = new Point(global::System.Math.Max(size0.X, size1.X), size0.Y + size1.Y);
            }
        }
    }
}
