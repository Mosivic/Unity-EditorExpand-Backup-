using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionPointType
{
    Null,
    In,
    Out,
}

public class ConnectPoint 

{
    public Rect rect;
    public ConnectionPointType type;
    public Node node;
    public GUIStyle style;


    public Action<ConnectPoint> OnClickConnectionPoint;

    public ConnectPoint(Node node,ConnectionPointType type,GUIStyle style,Action<ConnectPoint> OnClickConnectionPoint)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;

        rect = new Rect(0, 0, 10, 20);
    }


    public void Draw()
    {
        rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

        switch(type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;
            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        if(GUI.Button(rect,"",style))
        {
            OnClickConnectionPoint?.Invoke(this);
        }

    }


}
