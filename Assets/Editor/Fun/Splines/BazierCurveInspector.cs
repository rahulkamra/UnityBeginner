using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BazierCurve))]
public class BazierCurveInspector : Editor
{
    private BazierCurve curve;
    private Transform  handleTransform;
    private Quaternion handleRotation;

    private const int lineSteps = 10;
    private void OnSceneGUI()
    {
        this.curve = target as BazierCurve;
        this.handleTransform = this.curve.transform;

        this.handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 =  showPoint(0);
        Vector3 p1 = showPoint(1);
        Vector3 p2 = showPoint(2);

        Handles.color = Color.gray;
        Handles.DrawLine(p0, p1);
        Handles.DrawLine(p1, p2);


        Handles.color = Color.white;
        Vector3 beginPoint = curve.GetPoint(0f);
        for (int idx = 1; idx <= lineSteps; idx++)
         {
            float step = (float)idx / lineSteps;
            Vector3 endPoint = curve.GetPoint(step);

            Handles.DrawLine(handleTransform.TransformPoint(beginPoint),handleTransform.TransformPoint(endPoint));
            beginPoint = endPoint;
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

