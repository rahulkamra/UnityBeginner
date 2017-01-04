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

    public bool ContainsPoint(Vector3 point)
    {
        float radiusSquare = Radius * Radius;
        float xDistSquare = (point.x - Position.x) * (point.x - Position.x);
        float yDistSquare = (point.y - Position.y) * (point.y - Position.y);
        float temp = radiusSquare / (xDistSquare + yDistSquare);
        return temp >= 1f;
            
        //return ((Radius * Radius) / ((Position.x - point.x) * (Position.x - point.x)) + ((Position.y - point.y) * (Position.y - point.y))) <= 1f;
    }

    public float  getContribution(Vector3 point)
    {
        float radiusSquare = Radius * Radius;
        float xDistSquare = (point.x - Position.x) * (point.x - Position.x);
        float yDistSquare = (point.y - Position.y) * (point.y - Position.y);
        float temp = radiusSquare / (xDistSquare + yDistSquare);
        return temp;
    }
}
