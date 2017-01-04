using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CPUMetaBallCreator : BasicMetaBallCreator
{
    
    
    private MeshFilter filter;
    private CPUMetaBallRenderer metaBallRenderer;
    
    override protected void onStart()
    {
        base.onStart();
       
        metaBallRenderer = new CPUMetaBallRenderer();
        Mesh mesh = metaBallRenderer.Init(Bounds, new Vector2(Segmentation, Segmentation));
        this.filter = this.GetComponent<MeshFilter>();
        this.filter.mesh = mesh;
    }


    protected override void OnUpdate()
    {
        metaBallRenderer.Update(balls);
        
    }

}
