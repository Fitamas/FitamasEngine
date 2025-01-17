using Fitamas.Math2D;
using Fitamas.UserInterface.Scripting;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Fitamas.UserInterface
{
    public class GUIScrollRect : GUIComponent, IMouseEvent
    {
        public GUIScrollBar HorizontalScrollBar;
        public GUIScrollBar VerticalScrollBar;
        public GUIComponent Viewport;
        public GUIComponent Content;
        public float Sensitivity;
        public bool Horizontal;
        public bool Vertical;

        private Vector2 position;

        public GUIEvent<GUIScrollRect, Vector2> OnValueChanged = new GUIEvent<GUIScrollRect, Vector2>();

        public GUIScrollRect(GUIComponent content, GUIComponent viewport) : this(content, viewport, new Rectangle())
        {

        }
        public GUIScrollRect(GUIComponent content, Rectangle rectangle) : this(content, null, rectangle)
        {

        }

        private GUIScrollRect(GUIComponent content, GUIComponent _viewport = null, Rectangle rectangle = default) : base(rectangle)
        {
            Viewport = _viewport != null ? _viewport : this;
            Content = content;

            Sensitivity = 0.1f;
            Horizontal = true;
            Vertical = true;
        }

        protected override void OnInit()
        {
            HorizontalScrollBar.OnValueChanged.AddListener((s, v) =>
            {
                Vector2 value = new Vector2(v, VerticalScrollBar.Value);
                SetValue(value);
            });

            VerticalScrollBar.OnValueChanged.AddListener((s, v) =>
            {
                Vector2 value = new Vector2(HorizontalScrollBar.Value, v);
                SetValue(value);
            });
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Content != null)
            {
                Vector2 contentSize = Content.LocalScale.ToVector2();
                Vector2 viewportSize = Viewport.LocalScale.ToVector2();

                if (contentSize != Vector2.Zero && viewportSize != Vector2.Zero)
                {
                    HorizontalScrollBar.Size = viewportSize.X / contentSize.X;
                    VerticalScrollBar.Size = viewportSize.Y / contentSize.Y;
                }
                else
                {
                    HorizontalScrollBar.Size = 1;
                    VerticalScrollBar.Size = 1;
                }

                Vector2 contentOffset = new Vector2(Content.LocalScale.X / 2, -Content.LocalScale.Y / 2);
                Vector2 vieportOffset = new Vector2(Viewport.LocalScale.X * Content.Anchor.X, -Viewport.LocalScale.Y * Content.Anchor.Y);

                Content.LocalPosition = (contentOffset - position - vieportOffset).ToPoint();
            }
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {

        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {
            if (OnMouse)
            {
                float direction = MathV.Sign(mouse.ScrollWheelDelta); 
                VerticalScrollBar.Value += Sensitivity * direction;
            }
        }

        public void SetValue(Vector2 value) 
        {
            if (Content != null)
            {
                Point scale = Content.LocalScale - Viewport.LocalScale;

                if (scale.X < 0)
                {
                    value.X = 0;
                }
                if (scale.Y < 0)
                {
                    value.Y = 0;
                }

                position = new Vector2(scale.X * value.X, -scale.Y * value.Y);
            }

            OnValueChanged?.Invoke(this, value);            
        }
    }
}
