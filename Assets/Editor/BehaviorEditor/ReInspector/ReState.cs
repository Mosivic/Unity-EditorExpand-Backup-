using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ms.BehaviorEditor;

[CustomEditor(typeof(State))]
public class ReState :Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        State state = (State)target;
        if(GUILayout.Button("Test"))
        {
            Debug.Log("Test ok");
        }
    }
}
