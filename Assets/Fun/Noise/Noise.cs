using UnityEngine;
using System.Collections;

public static class Noise
{

	public static float Value(Vector3 point , int frequency)
    {
        point.x *= frequency;

        int intX = (int)point.x;
        return intX & 1;
    }
}
