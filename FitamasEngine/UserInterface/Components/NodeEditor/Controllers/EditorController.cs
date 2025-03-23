using System;
using Fitamas.UserInterface.Components.NodeEditor;

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

        public virtual void Update()
        {

        }
    }
}
