using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Ms.BehaviorEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region Variables
        static List<BaseNode> nodes = new List<BaseNode>();
        static List<Connect> connects = new List<Connect>();

        Vector3 mousePostion;
        bool makeTransition;
        bool clickedOnWindow;

        BaseNode selectedNode;
        BaseNode targetNode;


        public enum UseActions
        {
            addState,
            deleteNode,
            deleteTransition,
            addCommentNode,
            addConditionNode,
            addConnection,
        }


        #endregion


        #region Init
        [MenuItem("Behavior Eitor/Editor _P")]
        static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.minSize = new Vector2(800, 600);

        }
        #endregion


        #region GUI Methods
        private void OnEnable()
        {
            nodes.Clear();
            connects.Clear();
        }

        private void OnGUI()
        {
            Event e = Event.current;
            mousePostion = e.mousePosition;


            UserInput(e);
            DrawConnects();
            DrawWindows();

            if (GUI.changed) Repaint();
        }

        private void DrawConnects()
        {
            if (connects == null) return;
            foreach (var connect in connects)
            {
                Rect leftRect = connect.LeftNode.windowRect;
                Rect rightRect = connect.RightNode.windowRect;
                Vector3 startPos = new Vector3(leftRect.x + leftRect.width, leftRect.y + leftRect.height * 0.5f, 0);
                Vector3 endPos = new Vector3(rightRect.x, rightRect.y + rightRect.height * 0.5f, 0);
                DrawConnect(startPos, endPos);
            }
        }

        void DrawWindows()
        {
            BeginWindows();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].windowRect = GUI.Window(i, nodes[i].windowRect,DrawWindow,nodes[i].windowTitle);
                GUI.changed = true;
            }
            EndWindows();
        }

        public void DrawWindow(int id)
        {
            nodes[id].DrawWindow();
            GUI.DragWindow();
        }


        void UserInput(Event e)
        {
            if (!makeTransition)
            {
                if (e.button == 1)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        RightClick(e);
                        e.Use();
                    }

                }
                if (e.button == 0)
                {

                }
            }
            else
            {
                if (selectedNode == null)
                {
                    makeTransition = false;
                    return;
                }
                else
                {
                    ConnectTo(e);
                }

            }


        }

        void ConnectTo(Event e)
        {
            Rect rect = selectedNode.windowRect;
            DrawConnect(new Vector3(rect.x + rect.width, rect.y + rect.height * 0.5f, 0), mousePostion);
            foreach (var r in nodes)
            {
                //鼠标位置在Node上,Node不存在此Connect,Node不为自身,按下鼠标左键
                if (r.windowRect.Contains(e.mousePosition)&&!connects.Exists(t=> { return (t.LeftNode == selectedNode && t.RightNode == r); })&& r != selectedNode && e.button == 0 && e.type == EventType.MouseDown)
                {
                    Connect connect = new Connect(selectedNode, r);
                    connects.Add(connect);
                    connect.LeftNode.selfConnects.Add(connect);
                    connect.RightNode.selfConnects.Add(connect);
                    makeTransition = false;
                    e.Use();
                }
                else if(e.button==1&&e.type==EventType.MouseDown)
                {
                    makeTransition = false;
                }
            }
        }

        private void RightClick(Event e)
        {
            selectedNode = null;
            clickedOnWindow = false;
            foreach (var r in nodes)
            {
                if (r.windowRect.Contains(e.mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = r;
                }
            }


            if (!clickedOnWindow)
            {
                AddNewNode(e);
            }
            else
            {
                ModifyNode(e);
            }

        }

        void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UseActions.addState);
            menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UseActions.addCommentNode);
            menu.AddItem(new GUIContent("Add Condition"), false, ContextCallback, UseActions.addConditionNode);
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            if(selectedNode is BaseNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Connect"), false, ContextCallback, UseActions.addConnection);
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UseActions.deleteNode);
            }
            menu.ShowAsContext();
            e.Use();
        }


        void ContextCallback(object obj)
        {
            UseActions a = (UseActions)obj;
            switch (a)
            {
                case UseActions.addState:
                    StateNode stateNode = ScriptableObject.CreateInstance<StateNode>();
                    stateNode.windowRect = new Rect(mousePostion.x, mousePostion.y, 200, 300);
                    stateNode.windowTitle = "State";
                    nodes.Add(stateNode);
                    break;
                case UseActions.deleteNode:
                    if (selectedNode != null)
                    {
                        foreach (var connect in selectedNode.selfConnects)
                        {
                            connects.Remove(connect);
                        }
                        nodes.Remove(selectedNode);
                    }
                    break;
                case UseActions.deleteTransition:
                    break;
                case UseActions.addCommentNode:
                    CommentNode node = new CommentNode()
                    {
                        windowRect = new Rect(mousePostion.x, mousePostion.y, 200, 100),
                        windowTitle = "Comment"
                    };
                    nodes.Add(node);
                    break;
                case UseActions.addConditionNode:
                    ConditionNode transtionNode = ScriptableObject.CreateInstance<ConditionNode>();
                    transtionNode.windowRect = new Rect(mousePostion.x, mousePostion.y, 200, 300);
                    transtionNode.windowTitle = "Condition";
                    nodes.Add(transtionNode);
                    break;
                case UseActions.addConnection:
                    makeTransition = true;
                    break;
                default:
                    break;
            }
        }
        #endregion


        #region Helper Methods
        public void DrawConnect(Vector3 startPos, Vector3 endPos)
        {
            Vector3 leftTangent = startPos - Vector3.left * 50;
            Vector3 rightTangent = endPos - Vector3.right * 50;
            Handles.DrawBezier(startPos, endPos, leftTangent, rightTangent, Color.green, null, 5f);
            GUI.changed = true;
        }

    }
    #endregion
}

