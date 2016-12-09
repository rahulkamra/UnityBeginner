using UnityEngine;
using System.Collections;
using System;

public class BezierSpline : MonoBehaviour
{

    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f),
        };
    }


    public Vector3 GetPoint(float t)
    {
        //we need to get the index first
        float scaledT = GetCurveDeltaT(t);
        int index = GetCurveStartingIndex(t);
        return transform.TransformPoint(BezierUtils.GetPoint(points[index], points[index + 1], points[index + 2], points[index + 3], scaledT));
    }

    public Vector3 GetVelocity(float t)
    {
        float scaledT = GetCurveDeltaT(t);
        int index = GetCurveStartingIndex(t);
        return transform.TransformPoint(BezierUtils.GetFirstDerivative(points[index], points[index+ 1], points[index + 2], points[index + 3], scaledT)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

   
    public void AddCurve()
    {
        int numCurvePoints = 3;

        Vector3 lastPoint = points[points.Length - 1];
        Array.Resize<Vector3>(ref points, points.Length + numCurvePoints);
        for (int idx = 0; idx < numCurvePoints; idx++)
        {
            points[points.Length - 3] = new Vector3(lastPoint.x + 1, lastPoint.y, lastPoint.z);
            points[points.Length  - 2] = new Vector3(lastPoint.x + 2, lastPoint.y, lastPoint.z);
            points[points.Length - 1] = new Vector3(lastPoint.x + 3, lastPoint.y, lastPoint.z);
        }
    }

    private float GetCurveDeltaT(float t)
    {
        if(t >= 1f)
        {
            //means last curve
            return points.Length - 4;
        }
        else
        {
            //we are keeping in mind that each curve has same weight.
            t = Mathf.Clamp01(t)* CurveCount(); //this will scale the value from 0 - curve count
            return t - (int)t;
        }
    }

    private int GetCurveStartingIndex(float t)
    {
        if(t >= 1f )
        {
            return points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount(); //this will scale the value from 0 - curve count
            int i = (int)t;
            return i *3;
        }
    }


    public int CurveCount()
    {
        return (points.Length - 1) / 3;
    }

}

