using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DrawGizmosExample
{
    [DrawGizmo(GizmoType.NotInSelectionHierarchy|
        GizmoType.InSelectionHierarchy|
        GizmoType.Selected|
        GizmoType.Active|
        GizmoType.Pickable
        )]
    private static void MyCusonOnDrawGzimos(TargetExample targetExample,GizmoType gizmoType)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(targetExample.transform.position, new Vector3(5,5,5));
    }
}
