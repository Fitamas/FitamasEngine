using Fitamas.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.UserInterface.Themes
{
    public class GUITextBlockStyle
    {
        public static GUIStyle Create(ResourceDictionary dictionary)
        {
            GUIStyle style = new GUIStyle();

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.ColorProperty, dictionary, CommonResourceKeys.TextBlockDefaultColor)));

            style.Setters.Add(new Setter(new ResourceReferenceExpression(GUITextBlock.FontProperty, dictionary, CommonResourceKeys.DefaultFont)));

            return style;
        }
    }
}
