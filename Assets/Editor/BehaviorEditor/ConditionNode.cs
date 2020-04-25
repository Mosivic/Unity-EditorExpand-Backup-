using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


namespace Ms.BehaviorEditor
{
    public class ConditionNode : BaseNode
    {
        public Condition condition;
        public bool isCollapse;
        
       
        public override void DrawWindow()
        {

            Layout();
            Collapse();
        }


        void Layout()
        {
            isCollapse = GUILayout.Toggle(isCollapse, "展开");
            GUILayout.BeginVertical("AppToolbar");
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Condition");
            condition = (Condition)EditorGUILayout.ObjectField(condition, typeof(Condition), false);
            GUILayout.Space(5);
            GUILayout.EndVertical();
            
        }

        void Collapse()
        {
            if(!isCollapse)
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

