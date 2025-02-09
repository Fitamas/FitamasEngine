using Fitamas.UserInterface.Components.NodeEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    public class AreaController : EditorController
    {
        private bool moving;

        public AreaController(GUINodeEditor editor) : base(editor)
        {
            editor.OnMouseEvent.AddListener(OnMouseEvent);
            editor.OnKeybordEvent.AddListener(OnKeyTyped);
        }

        public override void Init()
        {

        }

        private void OnMouseEvent(GUINodeEditorEventArgs args)
        {
            if (args.Button == MouseButton.Middle)
            {
                if (args.EventType == GUIEventType.Drag)
                {
                    Point delta = new Point(args.DragDelta.X, -args.DragDelta.Y);

                    editor.Frame.LocalPosition += delta;

                    moving = true;
                }
                else if (args.EventType == GUIEventType.EndDrag)
                {
                    moving = false;
                }
            }
        }

        private void OnKeyTyped(KeyboardEventArgs args)
        {
            if (args.Key == Keys.Space)
            {
                editor.Frame.LocalPosition = new Point(0, 0);
            }
        }

        public override bool IsBusy()
        {
            return moving;
        }
    }
}
