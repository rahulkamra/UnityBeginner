using UnityEngine;
using System.Collections;

public static class BazierUtils
{


    public static Vector3 GetQuadraticPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return (1 - t) * (1 - t)* p0 + 2 * (1 - t)*  t * p1 + t * t * p2;
        //return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

    public static Vector3 GetLinearPoint(Vector3 p0 , Vector3 p1, Vector3 p2 , float t)
    {
        return (1 - t) * p0 + t * p2;
    }


    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
    }
}
