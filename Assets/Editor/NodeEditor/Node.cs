using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public  class Node 
{
    public Rect rect;
    public string title;

    public GUIStyle style;
    public GUIStyle defaultStyle;
    public GUIStyle selectedStyle;

    public bool isDragged;
    public bool isSelected;

    public ConnectPoint inPoint;
    public ConnectPoint outPoint;

    public Action<Node> OnRemoveNode;



    public Node(Vector2 position,float width,float height,GUIStyle nodeStyle,GUIStyle selectedStyle, GUIStyle inPointStyle,GUIStyle outPointStyle,
        Action<ConnectPoint> OnClickInPoint,Action<ConnectPoint> OnClickOutPoint,Action<Node> OnClickRemoveNode
        )
    {
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;

        inPoint = new ConnectPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
        outPoint = new ConnectPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);

        defaultStyle = nodeStyle;
        this.selectedStyle = selectedStyle;

        OnRemoveNode = OnClickRemoveNode;

    }




    public void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public virtual void Draw()
    {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);
    }

    public bool ProccessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button==0)
                {
                    if(rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        isSelected = true;
                        GUI.changed = true;

                        style = selectedStyle;
                    }
                    else
                    {
                        isSelected = false;
                        GUI.changed = true;
                        style = defaultStyle;
                    }
                }
                if(e.button==1&&isSelected&&rect.Contains(e.mousePosition))
                {
                    ProccessContextMenu();
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                isDragged = false;
                break;
            case EventType.MouseDrag:
                if(e.button==0&&isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;

        }
        return false;

    }

    private void ProccessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove Node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }


    private void OnClickRemoveNode()
    {
        OnRemoveNode?.Invoke(this);
    }
}
