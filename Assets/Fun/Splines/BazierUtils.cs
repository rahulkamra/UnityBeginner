﻿using UnityEngine;
using System.Collections;

public static class BazierUtils
{
    
    public static Vector3 GetPoint(Vector3 p0 , Vector3 p1, Vector3 p2 , float t)
    {
        return Vector3.Lerp(p0,p2,t);    
    }
}
