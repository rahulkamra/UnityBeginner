using UnityEngine;
using System.Collections;

public class BazierCurve : MonoBehaviour
{

    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[]
        {
             new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f)
        };
    }


    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(BazierUtils.GetQuadraticPoint(points[0], points[1], points[2], t));
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(BazierUtils.GetFirstDerivative(points[0], points[1], points[2], t));
    }

    public Vector3 Direction(float t)
    {
        return GetVelocity(t).normalized;
    }

}
