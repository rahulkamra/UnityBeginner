using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private const int lineSteps = 10;
    private const float directionScale = 0.5f;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.spline = target as BezierSpline;
        if(GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Adding Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }
    
    private void OnSceneGUI()
    {
        this.spline = target as BezierSpline;
        this.handleTransform = this.spline.transform;

        this.handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;


        Handles.color = Color.gray;
        Vector3 p0 = showPoint(0);
        for (int idx = 1; idx < spline.points.Length; idx+=3)
        {
            Vector3 p1 = showPoint(idx);
            Vector3 p2 = showPoint(idx + 1);
            Vector3 p3 = showPoint(idx + 2);

            
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        
        

        ShowDirections();



    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        for (int i = 1; i <= lineSteps; i++)
        {
            point = spline.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)lineSteps) * directionScale);
        }
    }

    private Vector3 showPoint(int index)
    {
        Vector3 pointLocalPosition = spline.points[index];
        Vector3 pointWorldPosition = handleTransform.transform.TransformPoint(pointLocalPosition);
        EditorGUI.BeginChangeCheck();

        Vector3 changedPoint = Handles.PositionHandle(pointWorldPosition, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "MoviePoint");
            EditorUtility.SetDirty(spline);
            spline.points[index] = handleTransform.InverseTransformPoint(changedPoint);
        }

        return changedPoint;
    }
}

