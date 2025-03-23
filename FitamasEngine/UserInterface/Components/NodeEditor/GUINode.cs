using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Themes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUINode : GUIComponent
    {
        public static readonly DependencyProperty<bool> IsSelectProperty = new DependencyProperty<bool>(false, false);

        public List<GUIPin> Pins { get; }
        public GUINodeEditor NodeEditor { get; set; }

        public bool IsSelect
        {
            get
            {
                return GetValue(IsSelectProperty);
            }
            set
            {
                SetValue(IsSelectProperty, value);
            }
        }

        public GUINode()
        {
            Pins = new List<GUIPin>();
        }

        public int IndexOf(GUIPin pin, GUIPinType type = GUIPinType.Input)
        {
            int k = 0;

            for (int i = 0; i < Pins.Count; i++)
            {
                if (Pins[i].PinType == type)
                {
                    if (Pins[i] == pin)
                    {
                        return k;
                    }

                    k++;
                }
            }

            return -1;
        }

        public GUIPin GetPin(int index, GUIPinType type = GUIPinType.Input)
        {
            int k = 0;
            for (int i = 0; i < Pins.Count; i++)
            {
                if (Pins[i].PinType == type)
                {
                    if (k == index)
                    {
                        return Pins[i];
                    }

                    k++;
                }
            }

            return null;
        }

        public void Add(GUIPin pin)
        {
            AddChild(pin);

            if (!Pins.Contains(pin))
            {
                Pins.Add(pin);
                RecalculateNode();
                NodeEditor?.OnCreatePin.Invoke(pin);
            }
        }

        public void Remove(GUIPin pin)
        {
            Pins.Remove(pin);
            RecalculateNode();
            NodeEditor?.OnDeletePin.Invoke(pin);
        }

        protected override void OnAddChild(GUIComponent component)
        {
            if (component is GUIPin pin)
            {
                Add(pin);
            }
        }

        protected override void OnRemoveChild(GUIComponent component)
        {
            if (component is GUIPin pin)
            {
                Remove(pin);
            }
        }

        public GUIPin CreatePin(string name, GUIPinType type = GUIPinType.Input, GUIPinAlignment pinAlignment = GUIPinAlignment.Left)
        {
            GUIPin pin = GUINodeUtils.CreatePin(name, type, pinAlignment);
            Add(pin);
            return pin;
        }

        public GUIPin CreatePin(GUIPinType type = GUIPinType.Input, GUIPinAlignment pinAlignment = GUIPinAlignment.Left)
        {
            GUIPin pin = GUINodeUtils.CreatePin(type, pinAlignment);
            Add(pin);
            return pin;
        }

        private void RecalculateNode()
        {
            int sizeY = 0;
            //if (HeaderTextBlock != null)
            //{
            //    //sizeY = (int)(HeaderTextBlock.LocalSize.Y * nodeEditor.Settings.HeaderSize);
            //}
            //if (HeaderImage != null)
            //{
            //    HeaderImage.LocalSize = new Point(0, sizeY);
            //}

            int spacing = 0;// nodeEditor.Settings.PinSpacing;
            Point sizeLeft = new Point(0, sizeY + spacing);
            Point sizeRight = new Point(0, sizeY + spacing);
            Point posLeft = new Point(0, -sizeY - spacing);
            Point posRight = new Point(0, -sizeY - spacing);
            foreach (GUIPin pin in Pins)
            {
                Point contentSize = Point.Zero; //nodeEditor.Settings.PinSize;
                pin.LocalSize = contentSize;
                contentSize.X += pin.ContentScale.X;
                contentSize.Y = Math.Max(contentSize.Y, pin.ContentScale.Y);
                contentSize.Y += spacing;

                if (pin.PinAlignment == GUIPinAlignment.Left)
                {
                    pin.SetAlignment(GUIAlignment.LeftTop);
                    pin.Pivot = new Vector2(0, 0.5f);
                    pin.LocalPosition = posLeft - new Point(0, contentSize.Y / 2);
                    posLeft.Y -= contentSize.Y;
                    sizeLeft.X = Math.Max(sizeLeft.X, contentSize.X);
                    sizeLeft.Y += contentSize.Y;
                }
                else if (pin.PinAlignment == GUIPinAlignment.Right)
                {
                    pin.SetAlignment(GUIAlignment.RightTop);
                    pin.Pivot = new Vector2(1, 0.5f);
                    pin.LocalPosition = posRight - new Point(0, contentSize.Y / 2);
                    posRight.Y -= contentSize.Y;
                    sizeRight.X = Math.Max(sizeRight.X, contentSize.X);
                    sizeRight.Y += contentSize.Y;
                }
            }

            Point size = Point.Zero;// nodeEditor.Settings.NodeSize;
            size.X = Math.Max(size.X, sizeLeft.X + sizeRight.X /*+ nodeEditor.Settings.SiteSpacing*/);
            size.Y = Math.Max(size.Y, sizeLeft.Y);
            size.Y = Math.Max(size.Y, sizeRight.Y);
            LocalSize = size;
        }
    }
}
