using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface
{
    public class GUIHorizontalGroup : GUIGroup
    {
        public int Count
        {
            get
            {
                return horizontalMaxCount;
            }
            set
            {
                horizontalMaxCount = value;
            }
        }

        public GUIHorizontalGroup() : base(verticalMaxCount: 1)
        {

        }
    }
}
