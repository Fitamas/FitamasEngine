﻿using Fitamas.UserInterface.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components.NodeEditor
{
    public class GUINode : GUIComponent
    {
        private GUINodeEditor nodeEditor;
        private List<GUIPin> pins = new List<GUIPin>();
        private bool isHovered = false;
        private bool isSelect = false;

        public GUITextBlock HeaderTextBlock;
        public GUIImage HeaderImage;
        public GUIImage Image;
        public GUIImage SelectedImage;

        public List<GUIPin> Pins => pins;

        //public bool IsHovered 
        //{ 
        //    get
        //    {
        //        return isHovered;
        //    }
        //    set
        //    {
        //        isHovered = value;


        //        //TODO
        //    }
        //}

        public bool IsSelect
        {
            get
            {
                return isSelect;
            }
            set
            {
                isSelect = value;
                if (SelectedImage != null)
                {
                    SelectedImage.Enable = isSelect;
                }
            }
        }

        public GUINode()
        {

        }

        public void Init(GUINodeEditor nodeEditor)
        {
            this.nodeEditor = nodeEditor;
            IsSelect = false;
            RecalculateNode();
        }

        public int IndexOf(GUIPin pin, GUIPinType type = GUIPinType.Input)
        {
            int k = 0;

            for (int i = 0; i < pins.Count; i++)
            {
                if (pins[i].PinType == type)
                {
                    if (pins[i] == pin)
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
            for (int i = 0; i < pins.Count; i++)
            {
                if (pins[i].PinType == type)
                {
                    if (k == index)
                    {
                        return pins[i];
                    }

                    k++;
                }
            }

            return null;
        }

        public void Add(GUIPin pin)
        {
            if (IsInHierarchy)
            {
                AddChild(pin);

                if (!pins.Contains(pin))
                {
                    pins.Add(pin);
                    RecalculateNode();
                    nodeEditor?.OnCreatePin.Invoke(pin);
                }
            }
        }

        public void Remove(GUIPin pin)
        {
            if (IsInHierarchy)
            {
                pins.Remove(pin);
                RecalculateNode();
                nodeEditor?.OnDeletePin.Invoke(pin);
            }
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

        public GUIPin CreateDefoultPin(string name, GUIPinType type = GUIPinType.Input, GUIPinAlignment pinAlignment = GUIPinAlignment.Left)
        {
            GUIPin pin = GUI.CreatePin(name, type, pinAlignment);
            ((GUITextBlock)pin.Content).Color = nodeEditor.Settings.PinTextColor;
            return CreateDefoultPin(pin);
        }

        public GUIPin CreateDefoultPin(GUIPinType type = GUIPinType.Input, GUIPinAlignment pinAlignment = GUIPinAlignment.Left)
        {
            return CreateDefoultPin(GUI.CreatePin(type, pinAlignment));
        }

        private GUIPin CreateDefoultPin(GUIPin pin)
        {
            pin.ImageOn.Sprite = nodeEditor.Settings.PinOn;
            pin.ImageOff.Sprite = nodeEditor.Settings.PinOff;
            pin.IsConnected = false;

            Add(pin);
            return pin;
        }

        public void RecalculateNode()
        {
            if (nodeEditor == null)
            {
                return;
            }

            int sizeY = 0;
            if (HeaderTextBlock != null)
            {
                sizeY = (int)(HeaderTextBlock.LocalSize.Y * nodeEditor.Settings.HeaderSize);
            }
            if (HeaderImage != null)
            {
                HeaderImage.LocalSize = new Point(0, sizeY);
            }

            int spacing = nodeEditor.Settings.PinSpacing;
            Point sizeLeft = new Point(0, sizeY + spacing);
            Point sizeRight = new Point(0, sizeY + spacing);
            Point posLeft = new Point(0, -sizeY - spacing);
            Point posRight = new Point(0, -sizeY - spacing);
            foreach (GUIPin pin in pins)
            {
                Point contentSize = nodeEditor.Settings.PinSize;
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

            Point size = nodeEditor.Settings.NodeSize;
            size.X = Math.Max(size.X, sizeLeft.X + sizeRight.X + nodeEditor.Settings.SiteSpacing);
            size.Y = Math.Max(size.Y, sizeLeft.Y);
            size.Y = Math.Max(size.Y, sizeRight.Y);
            LocalSize = size;
        }
    }
}
