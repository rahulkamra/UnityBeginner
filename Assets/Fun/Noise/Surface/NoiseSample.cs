using UnityEngine;
using System.Collections;

public struct NoiseSample
{
    public float Value;
    public Vector3 Derivative; 


    public static NoiseSample operator +(NoiseSample lhs , NoiseSample rhs)
    {
        lhs.Value += rhs.Value;
        lhs.Derivative += rhs.Derivative;
        return lhs;
    }

    public static NoiseSample operator +(NoiseSample lhs, float rhs)
    {
        lhs.Value += rhs;
        return lhs;
    }

    public static NoiseSample operator +(float lhs, NoiseSample rhs)
    {
        rhs.Value += lhs;
        return rhs;
    }



    public static NoiseSample operator -(NoiseSample lhs, NoiseSample rhs)
    {
        lhs.Value -= rhs.Value;
        lhs.Derivative -= rhs.Derivative;
        return lhs;
    }

    
    public static NoiseSample operator -(NoiseSample lhs, float rhs)
    {
        lhs.Value -= rhs;
        return lhs;
    }

    public static NoiseSample operator -(float lhs, NoiseSample rhs)
    {
        rhs.Value = lhs - rhs.Value;
        rhs.Derivative = -rhs.Derivative;
        return rhs;
    }



    public static NoiseSample operator *(NoiseSample lhs, float rhs)
    {
        lhs.Value *= rhs;
        lhs.Derivative *= rhs;
        return lhs;
    }

    public static NoiseSample operator *(float lhs, NoiseSample rhs)
    {
        rhs.Value *= lhs;
        rhs.Derivative *= lhs;
        return rhs;
    }

    public static NoiseSample operator *(NoiseSample lhs, NoiseSample rhs)
    {
        lhs.Value *= rhs.Value;
        lhs.Derivative = lhs.Derivative * rhs.Value + rhs.Derivative * lhs.Value ;
        return lhs;
    }

}
