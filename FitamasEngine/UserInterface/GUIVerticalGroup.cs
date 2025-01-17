using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface
{
    public class GUIVerticalGroup : GUIGroup
    {
        public int Count
        {
            get
            {
                return verticalMaxCount;
            }
            set
            {
                verticalMaxCount = value;
            }
        }

        public GUIVerticalGroup() : base(horizontalMaxCount: 1)
        {

        }
    }
}
