using Fitamas.Input.InputListeners;
using Fitamas.UserInterface.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Fitamas.UserInterface.Components.NodeEditor.Controllers
{
    internal class NodeController : EditorController
    {
        private List<GUINode> selectNodes = new List<GUINode>();
        private Point oldMousePosition;
        private bool isSelect;
        private bool isDragNode;

        public NodeController(GUINodeEditor editor) : base(editor)
        {
            editor.OnMouseEvent.AddListener(OnMouseEvent);
            editor.OnNodeInteractMouseEvent.AddListener(OnNodeEvent);
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

            Point localPosition = editor.ToLocal(args.MousePosition);

            if (args.EventType == GUINodeEditorEventType.Click && editor.IsMouseOver)
            {
                CancelSelects();
            }

            if (args.Button == MouseButton.Left)
            {
                if (args.EventType == GUINodeEditorEventType.StartDrag)
                {
                    if (editor.IsMouseOver)
                    {
                        isSelect = true;
                        editor.StartSelectRegion(localPosition);
                    }
                    else
                    {
                        oldMousePosition = localPosition;
                        isDragNode = true;
                    }
                }
                else if (args.EventType == GUINodeEditorEventType.Drag)
                {
                    if (isSelect)
                    {
                        editor.DoSelectRegion(localPosition);
                    }
                    else if (isDragNode)
                    {
                        Point delta = localPosition - oldMousePosition;
                        foreach (var node in selectNodes)
                        {
                            node.LocalPosition += delta;
                            editor.OnNodeEvent.Invoke(new GUINodeEventArgs(node, GUINodeEventType.Moved));
                        }
                        oldMousePosition = localPosition;
                    }
                }
                else if (args.EventType == GUINodeEditorEventType.EndDrag)
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
            if (args.Button == MouseButton.Left && args.EventType == GUINodeEditorEventType.Click)
            {
                if (args.Target is GUINode node)
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
                editor.OnNodeEvent.Invoke(new GUINodeEventArgs(node, GUINodeEventType.Select));
                node.IsSelect = true;
            }
        }

        public void CancelSelects()
        {
            foreach (var node in selectNodes)
            {
                node.IsSelect = false;
                editor.OnNodeEvent.Invoke(new GUINodeEventArgs(node, GUINodeEventType.Unselect));
            }

            selectNodes.Clear();
        }
    }
}
