using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    internal class AreaController : EditorController
    {
        private bool moving;

        public AreaController(GUINodeEditor editor) : base(editor)
        {
            editor.OnMouseEvent.AddListener(OnMouseEvent);
        }

        private void OnMouseEvent(GUINodeEditorEventArgs args)
        {
            if (args.Button == MouseButton.Middle)
            {
                if (args.EventType == GUINodeEditorEventType.Drag)
                {
                    editor.Content.LocalPosition += args.DragDelta;

                    moving = true;
                }
                else if (args.EventType == GUINodeEditorEventType.EndDrag)
                {
                    moving = false;
                }
            }
        }

        public override bool IsBusy()
        {
            return moving;
        }
    }
}
