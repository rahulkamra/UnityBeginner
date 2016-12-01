using UnityEngine;
using System.Collections;

[ExecuteInEditMode,RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Star : MonoBehaviour {

    [Range(1,20)]
    public int Frequency;
    public ColorPointModel Center;
    public ColorPointModel[] Points;

    private Mesh Mesh;

    private Vector3[] vertices;
    private int[] indicies;
    private Color[] colors;

    private void Start()
    {
        UpdateMesh();
    }


    public void UpdateMesh()
    {
        if(this.Mesh == null)
        {
            this.Mesh = new Mesh();
            this.Mesh.hideFlags = HideFlags.HideAndDontSave;
            this.GetComponent<MeshFilter>().mesh = this.Mesh;
            this.Mesh.name = "Star";
        }

        int NumVertices = Points.Length * Frequency;
        float numDegress = -360f / NumVertices;
        if (NumVertices < 3)
            return;


        if (vertices == null || vertices.Length != NumVertices +1)
        {
            //we need to clear everything 
            vertices = new Vector3[NumVertices + 1];
            indicies = new int[3 * NumVertices];
            colors = new Color[NumVertices + 1];
            Mesh.Clear();
        }
        

        //the first one is in the middle
        vertices[0] = Center.Position;
        colors[0] = Center.Color;

        //if vertices are 0 , 1, 2,3
        //indics are 0,1,2    0,2,3    0,3,4   0,4,1

        for (int fdx = 0, vdx = 1, idx = 1; fdx < Frequency; fdx++)
        {
            for (int pdx = 0; pdx < Points.Length; pdx++, vdx++, idx += 3)
            {

                vertices[vdx] = Quaternion.Euler(0f, 0f, numDegress * (vdx - 1)) * Points[pdx].Position;
                colors[vdx] = Points[pdx].Color;
                // indicies[3 * vdx - 3] = 0;//we can skip this if we want to
                indicies[idx] = vdx;
                indicies[idx + 1] = vdx + 1;
            }
        }


        indicies[indicies.Length - 1] = 1;
        this.Mesh.vertices = vertices;
        this.Mesh.triangles = indicies;
        this.Mesh.colors = colors;
    }

    void OnEnable()
    {
        UpdateMesh();

    }
    private void Reset()
    {
        UpdateMesh();
    }
}
