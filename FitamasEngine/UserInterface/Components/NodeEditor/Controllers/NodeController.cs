using Fitamas.UserInterface.Components;
using Fitamas.UserInterface.Components.NodeEditor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    public class NodeController : EditorController
    {
        private List<GUINode> selectNodes = new List<GUINode>();
        private Point oldMousePosition;
        private bool isSelect;
        private bool isDragNode;

        public NodeController(GUINodeEditor editor) : base(editor)
        {

        }

        public override void Init()
        {
            editor.OnMouseEvent.AddListener(OnMouseEvent);
            editor.OnNodeInteractMouseEvent.AddListener(OnNodeEvent);
            editor.OnKeybordEvent.AddListener(OnKeyTyped);



            //editor.OnDeleteNode.AddListener(n => Debug.Log(n));
            //editor.OnDeletePin.AddListener(n =>  Debug.Log(n));
            //editor.OnDeleteWire.AddListener(n => Debug.Log(n));
        }

        public override bool IsBusy()
        {
            return isSelect || isDragNode;
        }

        private void OnMouseEvent(GUINodeEditorEventArgs args)
        {
            if (editor.GetController<WireController>().IsBusy())
            {
                return;
            }

            if (args.EventType == GUIEventType.Click && args.Button == MouseButton.Right && editor.OnEptyField)
            {
                GUIContextMenu contextMenu = GUI.CreateContextMenu(new Rectangle(args.MousePosition, new Point(100, 100)));
                contextMenu.AddItem("Item");

                editor.OnCreateContextMenu.Invoke(args, contextMenu);
                editor.AddChild(contextMenu);
            }

            if (args.EventType == GUIEventType.Click && editor.OnEptyField)
            {
                CancelSelects();
            }

            if (args.Button == MouseButton.Left)
            {
                if (args.EventType == GUIEventType.StartDrag)
                {
                    if (editor.OnEptyField)
                    {
                        isSelect = true;
                        editor.StartSelectRegion(args.MousePosition);
                    }
                    else
                    {
                        oldMousePosition = args.MousePosition;
                        isDragNode = true;
                    }
                }
                else if (args.EventType == GUIEventType.Drag)
                {
                    if (isSelect)
                    {
                        editor.SelectRegion(args.MousePosition);
                    }
                    else if (isDragNode)
                    {
                        Point delta = args.MousePosition - oldMousePosition;
                        foreach (var node in selectNodes)
                        {
                            node.LocalPosition += delta;
                        }
                        oldMousePosition = args.MousePosition;
                    }
                }
                else if (args.EventType == GUIEventType.EndDrag)
                {
                    if (isSelect)
                    {
                        isSelect = false;
                        Rectangle region = editor.EndSelectRegion();

                        foreach (var node in editor.Nodes)
                        {
                            if (region.Intersects(node.Rectangle))
                            {
                                Select(node);
                            }
                        }
                    }
                    else if (isDragNode)
                    {
                        isDragNode = false;
                    }
                }
            }
        }

        private void OnNodeEvent(GUINodeEditorEventArgs args)
        {
            if (args.EventType == GUIEventType.Click && args.Button == MouseButton.Right)
            {
                GUIContextMenu contextMenu = GUI.CreateContextMenu(new Rectangle(args.MousePosition, new Point(100, 100)));
                //contextMenu.AddItem("Delete node", () => editor.Remove((GUINode)args.Component));

                editor.OnCreateContextMenu.Invoke(args, contextMenu);
                editor.AddChild(contextMenu);
            }

            if (args.Button == MouseButton.Left && args.EventType == GUIEventType.Click)
            {
                if (args.Component is GUINode node)
                {
                    if (!selectNodes.Contains(node))
                    {
                        CancelSelects();
                    }
                    Select(node);
                }
            }
        }

        public void Select(GUINode node)
        {
            if (!selectNodes.Contains(node))
            {
                selectNodes.Add(node);
                node.IsSelect = true;
            }
        }

        public void CancelSelects()
        {
            foreach (var node in selectNodes)
            {
                node.IsSelect = false;
            }

            selectNodes.Clear();
        }

        private void OnKeyTyped(KeyboardEventArgs args)
        {
            if (args.Key == Keys.Back)
            {
                foreach (var node in selectNodes)
                {
                    editor.Remove(node);
                }
            }
        }
    }
}
