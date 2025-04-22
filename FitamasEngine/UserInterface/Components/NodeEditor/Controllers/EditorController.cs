using System;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    public abstract class EditorController
    {
        protected GUINodeEditor editor;

        public EditorController(GUINodeEditor editor)
        {
            this.editor = editor;
        }

        public abstract bool IsBusy();
    }
}
