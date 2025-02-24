using Fitamas.Input.InputListeners;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Fitamas.Input
{
    public abstract class InputAction<T> where T : InputListener
    {
        public T InputListener { get; }

        public InputAction(T input)
        {
            InputListener = input;
        }

        public virtual void Update() { }
    }

    public class ScrollAction : InputAction<MouseListener>
    {
        private float value;
        private bool scroll = false;
        private bool reset = false;

        public float Value => value;

        public ScrollAction(MouseListener mouse) : base(mouse)
        {
            mouse.MouseWheelMoved += (s, e) =>
            {
                Calculate(e);
            };


        }

        private void Calculate(MouseEventArgs e)
        {
            scroll = true;

            if (e.ScrollWheelDelta > 0)
            {
                value = 1;
            }
            else
            {
                value = -1;
            }
        }

        public override void Update()
        {
            if (reset)
            {
                reset = false;
                scroll = false;
                value = 0;
            }

            if (scroll)
            {
                reset = true;
            }
        }
    }

    public class Vector2DAction : InputAction<KeyboardListener>
    {
        private Vector2 value;

        public Vector2 Value => value;

        public Vector2DAction(KeyboardListener keyboard) : base(keyboard)
        {
            keyboard.KeyPressed += (s, e) =>
            {
                Calculate(e);
            };
            keyboard.KeyReleased += (s, e) =>
            {
                Calculate(e);
            };
        }

        private void Calculate(KeyboardEventArgs e)
        {
            Vector2 result = Vector2.Zero;

            if (e.PressedKeys.Contains(Keys.W))
            {
                result += new Vector2(0, 1);
            }
            if (e.PressedKeys.Contains(Keys.S))
            {
                result += new Vector2(0, -1);
            }
            if (e.PressedKeys.Contains(Keys.A))
            {
                result += new Vector2(-1, 0);
            }
            if (e.PressedKeys.Contains(Keys.D))
            {
                result += new Vector2(1, 0);
            }

            if (result != Vector2.Zero)
            {
                value = Vector2.Normalize(result);
            }
            else
            {
                value = Vector2.Zero;
            }
        }
    }

    public class KeyboardAction : InputAction<KeyboardListener>
    {
        private bool isPressed;
        private bool isTap;
        private bool isDown;

        public bool IsTap => isTap;

        public KeyboardAction(KeyboardListener keyboard, Keys key) : base(keyboard)
        {
            keyboard.KeyPressed += (s, e) =>
            {
                if (e.Key == key)
                {
                    isPressed = true;
                }
            };
            keyboard.KeyReleased += (s, e) =>
            {
                if (e.Key == key)
                {
                    isPressed = false;
                }
            };
        }

        public override void Update()
        {
            if (isPressed)
            {
                if (!isDown)
                {
                    isTap = true;
                }
                else
                {
                    isTap = false;
                }

                isDown = true;
            }
            else
            {
                isDown = false;
            }
        }
    }

    public class MouseAction : InputAction<MouseListener>
    {
        private bool isPressed;
        private bool isDown;
        private bool isUp;

        public bool IsUp => isUp;
        public bool IsDown => isDown;
        public bool IsPressed => isPressed;

        public MouseAction(MouseListener mouse, MouseButton button) : base(mouse)
        {
            mouse.MouseDown += (s, e) =>
            {
                if (e.Button == button)
                {
                    isDown = true;
                }
            };
            mouse.MouseUp += (s, e) =>
            {
                if (e.Button == button)
                {
                    isUp = true;
                }
            };
        }

        public override void Update()
        {
            if (isPressed)
            {
                isDown = false;
            }
            else
            {
                isUp = false;
            }

            if (isDown)
            {
                isPressed = true;
            }

            if (isUp)
            {
                isPressed = false;
            }
        }
    }

    public class ActionMaps
    {
        private MouseListener mouse;
        private KeyboardListener keyboard;

        public Vector2DAction Direction { get; }
        public ScrollAction Scroll { get; }
        public KeyboardAction DropItem { get; }
        public MouseAction Use { get; }

        public ActionMaps(MouseListener mouse, KeyboardListener keyboard)
        {
            this.mouse = mouse;
            this.keyboard = keyboard;

            Direction = new Vector2DAction(keyboard);
            DropItem = new KeyboardAction(keyboard, Keys.G);
            Scroll = new ScrollAction(mouse);
            Use = new MouseAction(mouse, MouseButton.Left);
        }

        public void Update()
        {
            DropItem.Update();
            Scroll.Update();
            Use.Update();
        }
    }
}
