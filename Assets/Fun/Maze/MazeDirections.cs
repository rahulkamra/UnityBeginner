

using System;
using Random = UnityEngine.Random;

public static  class MazeDirections
{

    public const int Count = 4;

    //North,East, West, South
    private static IntVector2[] vectors =
    {
        new IntVector2(0,1),
        new IntVector2(1,0),
        new IntVector2(-1,0),
        new IntVector2(0,-1)
    };
    public static MazeDirection GetRandomDirection
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

}

