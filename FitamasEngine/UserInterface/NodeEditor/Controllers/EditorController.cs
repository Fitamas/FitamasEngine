using System;

namespace Fitamas.UserInterface.NodeEditor.Controllers
{
    public abstract class EditorController
    {
        protected GUINodeEditor editor;

        public EditorController(GUINodeEditor editor)
        {
            this.editor = editor;
        }

        public abstract void Init();

        public abstract bool IsBusy();

        public virtual void Update()
        {

        }
    }
}
