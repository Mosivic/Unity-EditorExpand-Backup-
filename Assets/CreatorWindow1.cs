using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatorWindow:EditorWindow
{
    Rect head;
    Rect content;
    Texture2D sprite;

    [MenuItem("MTools/Cretor")]
    public static void OpenWindow()
    {
        CreatorWindow window =(CreatorWindow) GetWindow(typeof(CreatorWindow));

        window.minSize = new Vector2(300,300);
  
        window.Show();
    }


    private void OnEnable()
    {
        sprite = Resources.Load<Texture2D>("UIBack");
        
    }

    private void OnGUI()
    {
        head.x = 0;
        head.y = 0;
        head.width = Screen.width;
        head.height = 100;

        content.x = 0;
        content.y =100;
       content.width = Screen.width;
        content.height = Screen.height - 100;


        GUI.DrawTexture(head, sprite);
        GUI.DrawTexture(content, sprite);

        
        GUILayout.BeginArea(head);
        GUILayout.Label("Header");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        EditorGUILayout.TextField("");
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();

        GUILayout.BeginArea(content);
        GUILayout.Label("Context");
        GUILayout.EndArea();
    }


}
