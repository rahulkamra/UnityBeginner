using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class GPUMetaBallCreator : BasicMetaBallCreator
{

    private Vector4[] shaderData;
    override protected void onStart()
    {
        base.onStart();
        this.ShowDebug = false;

        Bounds bounds = this.GetComponent<MeshRenderer>().bounds;
        float scale = Bounds.width / bounds.size.x;
        this.transform.localScale = new Vector3(scale, scale, 1);

        shaderData = new Vector4[NumBalls];
    }


    protected override void OnUpdate()
    {
        for(int idx = 0; idx < shaderData.Length; idx++)
        {
            shaderData[idx].x = ((Metaball)balls[idx]).Position.x + Bounds.size.x / 2;
            shaderData[idx].y = ((Metaball)balls[idx]).Position.y + Bounds.size.y / 2;
        }

   

        Shader.SetGlobalVectorArray("metaBalls", shaderData);
        Shader.SetGlobalFloat("width", Bounds.size.x);
        Shader.SetGlobalFloat("height", Bounds.size.y);
        Shader.SetGlobalFloat("radius", BallRadius / Bounds.size.x);
        Shader.SetGlobalFloat("numBalls", shaderData.Length);


    }
}
