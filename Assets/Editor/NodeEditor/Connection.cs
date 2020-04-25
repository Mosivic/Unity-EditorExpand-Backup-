using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class Connection 
{
    public ConnectPoint inPoint;
    public ConnectPoint outPoint;
    public Action<Connection> OnClickRemoveConnection;

    public Connection(ConnectPoint inPoint,ConnectPoint outPoint,Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;

    }


    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f,
            new Color32(43,133,20,255),
            null,
            6f
            );



        if(Handles.Button((inPoint.rect.center+outPoint.rect.center)*0.5f,Quaternion.identity,4,8,Handles.RectangleCap))
        {
            OnClickRemoveConnection?.Invoke(this);
        }
            
    }
}
