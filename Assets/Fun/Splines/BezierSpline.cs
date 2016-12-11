using UnityEngine;
using System.Collections;
using System;

public class BezierSpline : MonoBehaviour
{

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private BezierContolPointMode[] modes;

    private bool loop;
    public bool Loop
    {
        get { return loop; }
        set
        {
            this.loop = value;
            if (value)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0,points[0]);
            }
            
        }
    }
    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f),
        };

        modes = new BezierContolPointMode[]{BezierContolPointMode.Free, BezierContolPointMode.Free };
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

        Array.Resize(ref modes,modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];

        EnforceMode(points.Length-4);
        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
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

    public int CurvePointCount
    {
        get { return points.Length; }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index , Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = points[index] - point;
            if (loop)
            {
                if (index == 0)
                {
                    points[points.Length -2] += delta;
                    points[1] += delta;
                    points[points.Length - 1] = point;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }     
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                else if (index < points.Length - 1)
                {
                    points[index + 1] += delta;
                }
            }
        }

        points[index] = point;
        EnforceMode(index);
    }

    public BezierContolPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1)/3];
    }

    public void SetControlPointMode(int index, BezierContolPointMode mode)
    {
        int modeIndex = (index + 1)/3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }

        EnforceMode(index);
    }

    public int CurveCount()
    {
        return (points.Length - 1) / 3;
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1)/3;
        BezierContolPointMode mode = modes[modeIndex];
        if (mode == BezierContolPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1 || !loop)
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex = 0;
        int enforcedIndex = 0;

        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
                fixedIndex = points.Length - 2;
            

            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
                enforcedIndex = 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
                fixedIndex = 1;
            
            enforcedIndex = middleIndex -1 ;
            if (enforcedIndex < 0)
                enforcedIndex = points.Length - 2;
        }

        Vector3 middlePoint = points[middleIndex];
        Vector3 tangent = middlePoint - points[fixedIndex];

        if (mode == BezierContolPointMode.Aligned)
        {
            tangent = tangent.normalized*Vector3.Distance(middlePoint,points[enforcedIndex]);
            
        }        
        points[enforcedIndex] = middlePoint + tangent;
    }

             
}



