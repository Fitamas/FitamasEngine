using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Input;
using System;
using System.Linq;
using Fitamas.UserInterface.Scripting;

namespace Fitamas.UserInterface
{
    public class GUIButton : GUIComponent, IMouseEvent
    {
        public GUITextBlock TextBlock;
        public GUIImage Image;
        public Color DefoultColor = Color.White;
        public Color SelectColor = Color.DeepSkyBlue;
        public Color DisableColor = new Color(0.8f, 0.8f, 0.8f);

        public Color DefoultTextColor = Color.Black;
        public Color SelectTextColor = Color.White;
        public Color DisableTextColor = new Color(0.4f, 0.4f, 0.4f);

        public GUIEvent<GUIButton> OnClicked = new GUIEvent<GUIButton>();

        public GUIButton(Rectangle rectangle = new Rectangle()) : base(rectangle)
        {
            RaycastTarget = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Interecteble)
            {
                if (TextBlock != null)
                {
                    TextBlock.Color = OnMouse ? SelectTextColor : DefoultTextColor;
                }

                if (Image != null)
                {
                    Image.Color = OnMouse ? SelectColor : DefoultColor;
                }
            }
            else
            {
                if (TextBlock != null)
                {
                    TextBlock.Color = DisableTextColor;
                }

                if (Image != null)
                {
                    Image.Color = DisableColor;
                }
            }
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (OnMouse)
            {
                if (Interecteble && mouse.Button == MouseButton.Left)
                {
                    OnClicked?.Invoke(this);
                }

                OnClickedButton(mouse);
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }

        protected virtual void OnClickedButton(MouseEventArgs mouse)
        {

        }
    }
}
