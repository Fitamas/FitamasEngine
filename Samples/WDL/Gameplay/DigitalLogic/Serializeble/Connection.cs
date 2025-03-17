using Microsoft.Xna.Framework;
using System;

namespace WDL.Gameplay.DigitalLogic
{
    public struct Connection
    {
        public Point[] Points;
        public Color EnableColor;
        public Color DisableColor;
        public int OutputComponentId;
        public int OutputIndex;
        public int InputComponentId;
        public int InputIndex;
    }
}
