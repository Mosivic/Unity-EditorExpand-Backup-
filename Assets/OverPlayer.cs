using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(Player))]
public class OverPlayer : Editor
{
    public override void OnInspectorGUI()
    {
        Player player = (Player)target;

        player.AttackPower = EditorGUILayout.Slider("AttackPower",player.AttackPower, 0, 100);
        player.Denfendar = EditorGUILayout.Slider("DenfendarPower", player.Denfendar, 0, 100);

        

        base.OnInspectorGUI();
    }
}
