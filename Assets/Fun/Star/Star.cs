using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour {

    public int NumVertices;
    private Mesh Mesh;

    private Vector3[] vertices;
    private int[] indicies;

    private void Start()
    {
        this.Mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = this.Mesh;
        this.Mesh.name = "Star";

        vertices = new Vector3[NumVertices + 1];

        //
        float numDegress = -360f / NumVertices;

        indicies = new int[3 * NumVertices];

        //the first one is in the middle
        vertices[0] = new Vector3(0, 0, 0);

        //if vertices are 0 , 1, 2,3
        //indics are 0,1,2    0,2,3    0,3,4   0,4,1
        for (int vdx = 1 , idx = 1; vdx < vertices.Length; vdx++,idx +=3)
        {
            vertices[vdx] = Quaternion.Euler(0f, 0f, numDegress * (vdx - 1)) * Vector3.up;

           // indicies[3 * vdx - 3] = 0;//we can skip this if we want to
            indicies[idx] = vdx;
            indicies[idx+1] = vdx + 1;
        }
        
        indicies[indicies.Length - 1] = 1;
        this.Mesh.vertices = vertices;
        this.Mesh.triangles  = indicies;

    }

}
