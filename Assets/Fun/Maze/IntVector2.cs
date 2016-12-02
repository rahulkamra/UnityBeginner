using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

[Serializable]
public struct IntVector2
{
    public int x, z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator + (IntVector2 lhs, IntVector2 rhs)
    {
        return new IntVector2(lhs.x + rhs.x ,lhs.z + rhs.z);
    }

    public override string ToString()
    {
        return x + ", " + z;
    }
}
