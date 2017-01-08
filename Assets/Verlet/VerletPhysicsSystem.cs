using UnityEngine;
using UnityEditor;
using System.Collections;

public class VerletPhysicsSystem : MonoBehaviour
{
    public Rect Bounds;
    public Vector2 Gravity;

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

          //  vy += Gravity.y;
          //  vx += Gravity.x;

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
        if (collider.x > Bounds.xMax)
        {
            collider.x = Bounds.xMax;
            collider.px = Bounds.xMax + vx;
        }

        if (collider.y > Bounds.yMax)
        {
            collider.y = Bounds.yMax;
            collider.py = Bounds.yMax + vy;
        }


        if (collider.x < Bounds.xMin)
        {
            collider.x = Bounds.xMin;
            collider.px = Bounds.xMin + vx;
        }

        if (collider.y < Bounds.yMin)
        {
            collider.y = Bounds.yMin;
            collider.py = Bounds.yMin + vy;
        }


    }

    private void solveCollision()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = 0; j < bodies.Count; j++)
            {
                if (i >= j)
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

    private void handleCollision(VerletCircleCollider collider1, VerletCircleCollider collider2)
    {
        float collisionPointX = ((collider1.x * collider2.radius) + (collider2.x + collider1.radius)) / (collider1.radius + collider2.radius);
        float collisionPointY = ((collider1.y * collider2.radius) + (collider2.y + collider1.radius)) / (collider1.radius + collider2.radius);

        float vx1 = (collider1.lastVX * (collider1.mass - collider2.mass) +(2 * collider2.mass * collider2.lastVX)) / (collider1.mass + collider2.mass);
        float vy1 = (collider1.lastVY * (collider1.mass - collider2.mass) +(2 * collider2.mass * collider2.lastVY)) / (collider1.mass + collider2.mass);
        float vx2 = (collider2.lastVX * (collider2.mass - collider1.mass) +(2 * collider1.mass * collider1.lastVX)) / (collider1.mass + collider2.mass);
        float vy2 = (collider2.lastVY * (collider2.mass - collider1.mass) +(2 * collider1.mass * collider1.lastVY)) / (collider1.mass + collider2.mass);

        
        collider1.px = collider1.x;
        collider1.py = collider1.y;

        collider1.x += vx1 * (collider1.dynamicFriction);
        collider1.y += vy1 * (collider1.dynamicFriction);



        collider2.px = collider2.x;
        collider2.py = collider2.y;

      //  collider2.x += vx2 * (collider2.dynamicFriction);
       // collider2.y += vy2 * (collider2.dynamicFriction);
    }
}
