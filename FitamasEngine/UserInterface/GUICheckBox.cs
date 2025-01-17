using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Input;
using System;
using Fitamas.Serializeble;
using Fitamas.UserInterface.Scripting;

namespace Fitamas.UserInterface
{
    public class GUICheckBox : GUIComponent, IMouseEvent
    {
        [SerializableField] private bool value;

        public GUIImage BackGround;
        public GUIImage Checkmark;

        public GUIEvent<GUICheckBox, bool> OnValueChanged = new GUIEvent<GUICheckBox, bool>();

        public bool Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnValueChanged?.Invoke(this, this.value);
                }
            }
        }

        public GUICheckBox(Rectangle rectangle) : base(rectangle)
        {
            RaycastTarget = true;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (Checkmark != null)
            {
                Checkmark.Enable = value;
            }
        }

        public void OnClickedMouse(MouseEventArgs mouse)
        {
            if (OnMouse)
            {
                if (mouse.Button == MouseButton.Left)
                {
                    Value = !Value;
                }
            }
        }

        public void OnReleaseMouse(MouseEventArgs mouse)
        {

        }

        public void OnScrollMouse(MouseEventArgs mouse)
        {

        }
    }
}
