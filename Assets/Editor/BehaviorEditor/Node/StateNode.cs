using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ms.BehaviorEditor
{
    public class StateNode : BaseNode
    {
        bool isCollapse;
        public State state;

        Vector2 scrollDistance;

        SerializedObject serializedState;
        ReorderableList preStateList;
        ReorderableList onStateList;
        ReorderableList aftStateList;


        public override void DrawWindow()
        {
            Collapse();
            Layout();
        }


        void Layout()
        {
            
            isCollapse = GUILayout.Toggle(isCollapse, "展开");
            GUILayout.Label("Choose State");
            state = (State)EditorGUILayout.ObjectField(state, typeof(State), false);

            

            if(state!=null&&serializedState==null)
            {
                serializedState = new SerializedObject(state);
                preStateList = new ReorderableList(serializedState, serializedState.FindProperty("PreActions"), true, true, true, true);
                onStateList = new ReorderableList(serializedState, serializedState.FindProperty("OnActions"), true, true, true, true);
                aftStateList = new ReorderableList(serializedState, serializedState.FindProperty("AftActions"), true, true, true, true);
            }

            if(serializedState!=null)
            {
                serializedState.Update();
                HandleReorderableList(preStateList, "Pre State");
                HandleReorderableList(onStateList, "On State");
                HandleReorderableList(aftStateList, "Ater State");
                scrollDistance=GUILayout.BeginScrollView(scrollDistance);
                preStateList.DoLayoutList();
                onStateList.DoLayoutList();
                aftStateList.DoLayoutList();
                GUILayout.EndScrollView();

                serializedState.ApplyModifiedProperties();
            }

        }

        //重绘Reorderablelist
        void HandleReorderableList(ReorderableList list,string targetName)
        {
            list.drawHeaderCallback = (Rect rect) =>
              {
                  EditorGUI.LabelField(rect, targetName);
              };
            list.drawElementCallback = (Rect rect,int index,bool isActive,bool isFocus ) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element,GUIContent.none);
            };
        }

        void Collapse()
        {
            if (!isCollapse)
            {
                windowRect.height = 100;
            }
            else
            {
                windowRect.height = 300;
            }
        }




    }
}