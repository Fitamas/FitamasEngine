﻿using Fitamas.Math2D;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.UserInterface.Components
{
    public abstract class GUIRange : GUIComponent
    {
        private GUITrack track;

        public GUITrack Track
        {
            get
            {
                return track;
            }
            set
            {
                if (track != value)
                {
                    if (track != null)
                    {
                        track.Thumb.DragDelta.RemoveListener(UpdateThumb);
                    }

                    track = value;
                    track.Thumb.DragDelta.AddListener(UpdateThumb);
                    UpdateThumb();
                }

            }
        }

        public GUIRange()
        {
            RaycastTarget = true;
        }

        private void UpdateThumb(GUIThumb thumb, DragEventArgs args)
        {
            track.Value = FindNearestValue(args.Position);
        }

        private void UpdateThumb()
        {
            SetDirty();
        }

        protected virtual float FindNearestValue(Point position)
        {
            int remainingSize = track.Lenght - (int)(track.Lenght * track.ThumbScale);
            Point localPosition = position - Rectangle.Location;
            Point offset = track.Thumb.StartDragOffset;
            float result;

            switch (track.Direction)
            {
                case GUISliderDirection.LeftToRight:
                    result = FindNearestValue(remainingSize, localPosition.X - offset.X);
                    break;
                case GUISliderDirection.RightToLeft:
                    result = FindNearestValue(remainingSize, remainingSize - localPosition.X + offset.X);
                    break;
                case GUISliderDirection.TopToBottom:
                    result = FindNearestValue(remainingSize, remainingSize - localPosition.Y + offset.Y);
                    break;
                case GUISliderDirection.BottomToTop:
                    result = FindNearestValue(remainingSize, localPosition.Y - offset.Y);
                    break;
                default:
                    result = track.MinValue;
                    break;
            }

            return result;
        }

        protected float FindNearestValue(float lenght, float progress)
        {
            if (progress < 0)
            {
                return track.MinValue;
            }
            else if (progress > lenght)
            {
                return track.MaxValue;
            }
            else
            {
                float distance = Math.Abs(track.MaxValue - track.MinValue);
                float k = 0;

                if (distance > 0)
                {
                    k = lenght / distance;
                }

                return progress / k;
            }
        }
    }
}