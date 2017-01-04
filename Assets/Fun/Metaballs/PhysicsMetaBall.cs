using UnityEngine;
using System.Collections;

public class PhysicsMetaBallWorld : GPUMetaBallCreator
{
    
    public GameObject Parent;


    protected override void onStart()
    {
        NumBalls = Parent.transform.childCount;
        base.onStart();
    }

     void Update()
    {
        for (int idx = 0; idx < Parent.transform.childCount; idx++)
        {
            ((Metaball)balls[idx]).Position = Parent.transform.GetChild(idx).transform.position;
        }
        
        OnUpdate();
        
    }
    
}






