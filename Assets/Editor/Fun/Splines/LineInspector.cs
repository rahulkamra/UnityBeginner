using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
    private static Vector3 pointSnap = Vector3.one * 0.1f;
    void OnSceneGUI()
    {
        Line line = target as Line;
        Transform lineTransform =  line.transform;
        Vector3 p0World = lineTransform.TransformPoint(line.p0);
        Vector3 p1World = lineTransform.TransformPoint(line.p1);

        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
             lineTransform.rotation : Quaternion.identity;

        Handles.DrawLine(p0World, p1World);



        EditorGUI.BeginChangeCheck();
        Vector3 p0Changed = Handles.PositionHandle(p0World, lineTransform.rotation);
        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(line);
            Undo.RecordObject(line, "MoviePoint");
            line.p0 = lineTransform.InverseTransformPoint(p0Changed);
        }

        EditorGUI.BeginChangeCheck();

        Vector3 p1Changed = Handles.PositionHandle(p1World, line.transform.rotation);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(line);
            Undo.RecordObject(line, "MoviePoint");
            line.p1 = lineTransform.InverseTransformPoint(p1Changed);
        }
        EditorGUI.BeginChangeCheck();

    }

}
