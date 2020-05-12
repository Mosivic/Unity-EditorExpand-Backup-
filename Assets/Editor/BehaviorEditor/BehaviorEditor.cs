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
        Rect menuBar;
        float menuBarHeight=20f;


        static List<BaseNode> nodes = new List<BaseNode>();
        static List<Vector2> connects = new List<Vector2>();

        Vector3 mousePostion;
        bool makeTransition;
        bool clickedOnWindow;

        BaseNode selectedNode;


        public static BehaviorGraph currentGraph;
        public float currentkey;
        public List<float> index=new List<float>();

        public enum UseActions
        {
            addNodeState,
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
            index.Clear();
            currentGraph = null;
            currentkey = 0;
        }

        private void OnGUI()
        {
            Event e = Event.current;
            mousePostion = e.mousePosition;
            UserInput(e);
            DrawMenuBar();
            DrawConnects();
            DrawWindows();

            if (GUI.changed) Repaint();
        }

        void DrawMenuBar()
        {
            menuBar = new Rect(0, 0, position.width,menuBarHeight );
            GUILayout.BeginArea(menuBar,EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Choose Current Graph:");
            currentGraph= (BehaviorGraph)EditorGUILayout.ObjectField(currentGraph, typeof(BehaviorGraph), false);
            if(currentGraph!=null)
            {
                if (GUILayout.Button("Load")) LoadGraph();
                if (GUILayout.Button("Save")) SaveGraph();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawConnects()
        {
            if (connects == null) return;
            foreach (var connect in connects)
            {
                Rect leftRect = FindNodeByKey(connect.x).windowRect;
                Rect rightRect = FindNodeByKey(connect.y).windowRect;
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
                if (r.windowRect.Contains(e.mousePosition)&&!connects.Exists(t=> { return (t.x == selectedNode.key && t.y == r.key); })&& r != selectedNode && e.button == 0 && e.type == EventType.MouseDown)
                {
                    Vector2 connect = new Vector2(selectedNode.key, r.key);
                    connects.Add(connect);
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
            menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UseActions.addNodeState);
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
                case UseActions.addNodeState:
                    StateNode stateNode = ScriptableObject.CreateInstance<StateNode>();
                    stateNode.key = currentkey;
                    stateNode.windowRect = new Rect(mousePostion.x, mousePostion.y, 200, 300);
                    stateNode.windowTitle = "State";
                    nodes.Add(stateNode);
                    index.Add(currentkey);
                    currentkey++;
                    break;
                case UseActions.deleteNode:
                    if (selectedNode != null)
                    {
                        for (int i = connects.Count-1; i >= 0; i--)
                        {
                            if (connects[i].x == selectedNode.key||connects[i].y==selectedNode.key)
                            {
                                connects.Remove(connects[i]);
                            }
                        }
                        nodes.Remove(selectedNode);
                    }
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
                    transtionNode.key = currentkey;
                    transtionNode.windowRect = new Rect(mousePostion.x, mousePostion.y, 200, 300);
                    transtionNode.windowTitle = "Condition";
                    nodes.Add(transtionNode);
                    index.Add(currentkey);
                    currentkey++;
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
        
        public BaseNode FindNodeByKey(float key)
        {
            foreach (var node in nodes)
            {
                if (node.key == key) return node;
            }
            return null;
        }

        public void SaveGraph()
        {
            //string path = "Assets/Resources/graph.asset";
            //if (currentGraph==null)
            //{    
            //    BehaviorGraph graph = ScriptableObject.CreateInstance<BehaviorGraph>();
            //    AssetDatabase.CreateAsset(graph, path);
            //    AssetDatabase.SaveAssets();
            //    AssetDatabase.Refresh();
            //}
            //currentGraph= AssetDatabase.LoadAssetAtPath<BehaviorGraph>(path);

            GraphConverter.ConverterToData(nodes, connects, currentGraph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        public void LoadGraph()
        {
            GraphConverter.ConverterToGraph(currentGraph,out nodes,out connects,out index);
               
        }
    }
    #endregion
}

