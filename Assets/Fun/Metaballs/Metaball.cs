using UnityEngine;
using System.Collections;

public class Metaball
{

    public Metaball(Vector2 Position)
    {
        this.Position = Position;
        this.Velocity = Vector2.zero;

    }
    public Vector2 Position;
    public Vector2 Velocity;
    public float Radius;

}
