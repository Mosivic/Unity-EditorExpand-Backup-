using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class NodeBaseEditor : EditorWindow
{

    List<Node> nodes;
    List<Connection> connections;

    GUIStyle nodeStyle;
    GUIStyle selectedNodeStyle;

    GUIStyle inPointStyle;
    GUIStyle outPointStyle;


    ConnectPoint selectedInPoint;
    ConnectPoint selectedOutPoint;

    private Vector2 drag;
    private Vector2 offset;


    [MenuItem("Tool/NodeEditor")]
    private static void OpenWindow()
    {
        EditorWindow window= GetWindow(typeof(NodeBaseEditor), false, "NodeEditor");    
    }


    private void OnEnable()
    {
   
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);

    }

    private void OnGUI()
    {
        DrawNodes();
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
        DrawConnections();
        DrawConnectionLine(Event.current);

        ProcessEvents(Event.current);
        ProcessNodeEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
            selectedInPoint.rect.center,
            e.mousePosition,
            selectedInPoint.rect.center + Vector2.left * 50f,
            e.mousePosition - Vector2.left * 50f,
            new Color32(43, 133, 20, 255),
            null,
            6f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
            selectedOutPoint.rect.center,
            e.mousePosition,
            selectedOutPoint.rect.center - Vector2.left * 50f,
            e.mousePosition + Vector2.left * 50f,
            new Color32(43, 133, 20, 255),
            null,
            6f
            );

            GUI.changed = true;
        }
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw();
            }
        }


    }


    private void DrawConnections()
    {
        if(connections!=null)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Draw();
            }
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }


    private void ProcessEvents(Event e)
    {
       drag = Vector2.zero;

        switch(e.type)
        {
            case EventType.MouseDown:
                if(e.button==0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProccessContextMenu(e.mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if(e.alt==true)
                {
                    OnDrag(e.delta);
   
                }
                break;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;
        if(nodes!=null)
        {
            foreach (var node in nodes)
            {
                node.Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProccessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }


        }
    }


    private void ProccessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"),false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }


    void OnClickAddNode(Vector2 mousePosition)
    {
        if(nodes==null)
        {
            nodes = new List<Node>();
        }
        nodes.Add(new Node(mousePosition, 200,100 , nodeStyle,selectedNodeStyle, inPointStyle,outPointStyle,OnClickInPoint,OnClickOutPoint,OnClickRemoveNode));
    }



    private void OnClickRemoveNode(Node node)
    {
        if(connections!=null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }
        nodes.Remove(node);
    }



    private void OnClickOutPoint(ConnectPoint OutPoint)
    {
        selectedOutPoint = OutPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickInPoint(ConnectPoint inPoint)
    {
        selectedInPoint = inPoint;

        if(selectedOutPoint!=null)
        {
            if(selectedOutPoint.node!=selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void CreateConnection()
    {
        if(connections==null)
        {
            connections = new List<Connection>();
        }
        connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }


}
