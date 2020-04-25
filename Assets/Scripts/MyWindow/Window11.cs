using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Window1: EditorWindow
{
    Rect textField;
    string text;
    private TextAsset source;
    private string saveFilePath;

    [MenuItem("Tool/Window1")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Window1), false, "Window1");
        

    }


    private void OnGUI()
    {
        

        GUILayout.BeginHorizontal();

        DrawLeft();
        DrawRight();


        GUILayout.EndHorizontal();

    }


    void DrawLeft()
    {


        GUILayout.BeginVertical();

        source = (TextAsset)EditorGUILayout.ObjectField(source, typeof(TextAsset), true);
        saveFilePath = EditorGUILayout.TextField("Save In", saveFilePath);
        
        if(GUILayout.Button(new GUIContent("Show")))
        {
            text = source.text;
        }

    

        if (GUILayout.Button(new GUIContent("Save")))
        {
            SaveFile();
        }

        GUILayout.EndVertical();
    }


    void DrawRight()
    {
        GUILayout.BeginVertical(GUILayout.Width(position.width/2));

        GUILayout.Space(5);
        GUILayout.Label("Preview");
        GUILayout.TextArea(text,GUILayout.ExpandHeight(true));
        
        GUILayout.EndVertical();
    }

    void SaveFile()
    {
        if(saveFilePath.Equals(""))
        {
            saveFilePath = "Assets/PlayerFile";
        }
        else
        {
            if(!Directory.Exists(saveFilePath))
            {
                Directory.CreateDirectory(saveFilePath);
            }
            else
            {
                StreamWriter writer = new StreamWriter(saveFilePath+source.name+".txt");
                writer.Write(source.text);
                writer.Close();
            }
        }
    }
}
