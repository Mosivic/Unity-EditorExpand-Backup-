
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class script101 
{

#if UNITY_EDITOR
    [MenuItem("EditorExpand/Show101")]
    private static void Show101()
    {

        //显示一个窗口，返回确定键是否被按下
      if(EditorUtility.DisplayDialog(
            "Hello World",
            "Do you really want to do this?",
            "Create",
            "Cancel"
            ))
        {
            new GameObject("Hello World");
        }

    }
#endif

}
