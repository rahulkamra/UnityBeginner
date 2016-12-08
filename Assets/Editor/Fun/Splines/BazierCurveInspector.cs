using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BazierCurveInspector : Editor
{
    private BezierCurve curve;
    private Transform  handleTransform;
    private Quaternion handleRotation;

    private const int lineSteps = 10;
    private const float directionScale = 0.5f;

    private void OnSceneGUI()
    {
        this.curve = target as BezierCurve;
        this.handleTransform = this.curve.transform;

        this.handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = showPoint(0);
        Vector3 p1 = showPoint(1);
        Vector3 p2 = showPoint(2);
        Vector3 p3 = showPoint(3);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p2, p3);


        Handles.color = Color.white;
       
        Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);

        ShowDirections();

     

    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    private Vector3 showPoint(int index)
    {
        Vector3 pointLocalPosition = curve.points[index];
        Vector3 pointWorldPosition = handleTransform.transform.TransformPoint(pointLocalPosition);
        EditorGUI.BeginChangeCheck();

        Vector3 changedPoint = Handles.PositionHandle(pointWorldPosition, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "MoviePoint");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(changedPoint);
        }
        
        return changedPoint;
    }
}

