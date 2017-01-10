using UnityEngine;
using UnityEditor;
using System.Collections;

public class VerletPhysicsSystem : MonoBehaviour
{
    public Rect Bounds;
    public Vector2 Gravity;
    public float SurfaceBounce = 0.8f;

    public void OnEnable()
    {

    }

    public void AddCircleCollider(VerletCircleCollider collider)
    {
        if (!bodies.Contains(collider))
            bodies.Add(collider);
    }

    private ArrayList bodies = new ArrayList();

    void Update()
    {
        float deltaTime = Time.deltaTime;
        updatePositions(deltaTime);
        solveCollision();
        
        apply();
    }


    private void updatePositions(float deltaTime)
    {

        for (int i = 0; i < bodies.Count; i++)
        {
            VerletCircleCollider collider = (VerletCircleCollider)bodies[i];

            float vx = collider.x - collider.px;
            float vy = collider.y - collider.py;

            vy += Gravity.y;
            vx += Gravity.x;

            collider.lastVX = vx;
            collider.lastVY = vy;

            collider.px = collider.x;
            collider.py = collider.y;

            collider.x += vx;
            collider.y += vy;
            
            boundryCheck(collider, vx, vy);
        }
    }

    private void apply()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            VerletCircleCollider collider = (VerletCircleCollider)bodies[i];
            collider.Apply();
        }
    }


    private void boundryCheck(VerletCircleCollider collider, float vx, float vy)
    {
        if (collider.x >= Bounds.xMax)
        {
            collider.x = Bounds.xMax;
            collider.px = Bounds.xMax + vx * SurfaceBounce;
        }

        if (collider.y >= Bounds.yMax)
        {
            collider.y = Bounds.yMax;
            collider.py = Bounds.yMax + vy * SurfaceBounce;
        }
        
        if (collider.x <= Bounds.xMin)
        {
            collider.x = Bounds.xMin;
            collider.px = Bounds.xMin + vx * SurfaceBounce;
        }

        if (collider.y <= Bounds.yMin)
        {
            collider.y = Bounds.yMin;
            collider.py = Bounds.yMin + vy * SurfaceBounce;
            //friction is always perpendicular to the axis of collision
        }
    }


    private void solveCollision()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = 0; j < bodies.Count; j++)
            {
                if (i <= j)
                    continue;
                VerletCircleCollider collider1 = (VerletCircleCollider)bodies[i];
                VerletCircleCollider collider2 = (VerletCircleCollider)bodies[j];
                float distance = collider1.getDistance(collider2);
                if (collider1.radius + collider2.radius >= distance)
                {
                    //collision;
                    handleCollision(collider1, collider2);
                }

            }
        }
    }

    private void handleCollision(VerletCircleCollider body1, VerletCircleCollider body2)
    {
        float x = body1.x - body2.x;
        float y = body1.y - body2.y;
        float slength = x * x + y * y;
        float length = Mathf.Sqrt(slength);
        float target = body1.radius + body2.radius;

        if (length < target)
        {
            var factor = (length - target) / length;

            body1.x -= x * factor * 0.5f;
            body1.x += body1.friction * body1.mass;

            body1.y -= y * factor * 0.5f;
            body1.y += body1.friction * body1.mass;

            body2.x += x * factor * 0.5f;
            body2.x -= body2.friction * body2.mass;

            body2.y += y * factor * 0.5f;
            body2.y -= body2.friction * body2.mass;
        }
    }
}
