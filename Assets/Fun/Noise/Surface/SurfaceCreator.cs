using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour {

    // Use this for initialization

    private Mesh mesh;
    private int currentResolution;

    [Range(3, 100)]
    public int Resolution = 10;

    [Range(2, 1024)]
    public int Frequency = 512;

    [Range(1, 3)]
    public int Dimention = 3;

    [Range(1, 8)]
    public int Octaves = 1;

    [Range(1f, 4f)]
    public float Lacunarity = 1;

    [Range(0f, 1f)]
    public float Persistence = 1;

    public Vector3 Offset;
    public Vector3 Rotation;

    public Gradient Coloring;
    public SurfaeNoiseMthodType MethodType;

    [Range(0f, 1f)]
    public float Strength = 1f;


    public bool ColoringForStrength;
    public bool Damping;
    public bool ShowNormals;
    public bool AnalyticalDerivatives;

    private Vector3[] vertices;
    private Vector3[] normals;
    private Color[] colors;

    private void OnEnable()
    {
        if (this.mesh == null)
        {
            this.mesh = new Mesh();
            this.mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = this.mesh;
        }
        Refresh();
    }


    public void Refresh()
    {
        if (Resolution != this.currentResolution)
        {
            createGrid();
        }

        Quaternion rotation  = Quaternion.Euler(Rotation);
        Vector3 point00 = rotation * transform.TransformPoint(new Vector3(-0.5f, -0.5f) + Offset);
        Vector3 point10 = rotation * transform.TransformPoint(new Vector3(0.5f, -0.5f)) + Offset;
        Vector3 point01 = rotation * transform.TransformPoint(new Vector3(-0.5f, 0.5f)) + Offset;
        Vector3 point11 = rotation * transform.TransformPoint(new Vector3(0.5f, 0.5f)) + Offset;

        //UnityEngine.Random.InitState(42);

        float stepSize = 1f / Resolution;
        SurfaceNoiseMethod method = SurfaceNoise.NoiseMethods[(int)MethodType][Dimention - 1];
        float amplitude = Damping ? Strength / Frequency : Strength;

        for (int idx = 0 , idv = 0; idx <= Resolution; idx++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, stepSize * (idx));
            Vector3 point1 = Vector3.Lerp(point10, point11, stepSize * (idx));

            for (int jdx = 0; jdx <= Resolution; jdx++ , idv++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, stepSize * (jdx));

                
                NoiseSample sample = SurfaceNoise.Sum(method, point, Frequency, Octaves, Lacunarity, Persistence);

                if (MethodType == SurfaeNoiseMthodType.Value)
                    sample = sample - 0.5f;
                else
                    sample = sample * 0.5f;

                if(ColoringForStrength)
                {
                    sample = sample * amplitude;
                    colors[idv] = Coloring.Evaluate(sample.Value + 0.5f);
                }
                else
                {
                    colors[idv] = Coloring.Evaluate(sample.Value + 0.5f);
                    sample = sample * amplitude;
                }

                vertices[idv].y = sample.Value;
                if(AnalyticalDerivatives)
                {
                    normals[idx] = new Vector3(-sample.Derivative.x, 1f, -sample.Derivative.y);
                }
                
            }
        }

        mesh.colors = colors;
        mesh.vertices = vertices;
        if(!AnalyticalDerivatives)
            CalculateNormals();
        mesh.normals = normals;
    }

    private void createGrid()
    {
        this.currentResolution = Resolution;
        this.mesh.Clear();

        vertices = new Vector3[(Resolution + 1) * (Resolution + 1)];
        normals = new Vector3[vertices.Length];
        colors = new Color[vertices.Length];
        Vector2[] uv = new Vector2[vertices.Length];
        
        float stepSize = 1f / Resolution;

        for (int v = 0, z = 0; z <= Resolution; z++)
        {
            for (int x = 0; x <= Resolution; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f,0f, z * stepSize - 0.5f);
                uv[v] = new Vector2(x * stepSize, z * stepSize);
                normals[v] = Vector3.up;
                colors[v] = Color.black;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        


        int[] triangles = new int[Resolution * Resolution * 6];
        for (int t = 0, v = 0, y = 0; y < Resolution; y++, v++)
        {
            for (int x = 0; x < Resolution; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + Resolution + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + Resolution + 1;
                triangles[t + 5] = v + Resolution + 2;
            }
        }

        mesh.triangles = triangles;
    }



    private void OnDrawGizmosSelected()
    {
        if(ShowNormals && vertices != null)
        {
            float scale = 1f / Resolution;
            Gizmos.color = Color.yellow;
            for(int idx = 0; idx < vertices.Length; idx++)
            {
                Gizmos.DrawRay(vertices[idx],normals[idx] * scale);
            }

            
        }

    }
    
    private void CalculateNormals()
    {
        for(int v = 0 , z = 0; z <= Resolution; z++)
        {
            for (int x = 0;  x <= Resolution; x++ , v++)
            {
                normals[v] = Vector3.Cross(new Vector3(1f, -getXDerivative(x, z), 0f) , -new Vector3(0f, getZDerivative(x, z), 1f));
            }
        } 
    }

    private float getXDerivative(int x , int z)
    {
        int rowOffset = z * (Resolution + 1);
        float left, right, scale;

        if(x > 0)
        {
            left = vertices[rowOffset + x - 1].y;
            if(x < Resolution)
            {
                right = vertices[rowOffset + x + 1].y;
                scale = 0.5f * Resolution;
            }
            else
            {
                right = vertices[rowOffset + x].y;
                scale = Resolution;
            }
            
        }
        else
        {
            left = vertices[rowOffset + x].y;
            right = vertices[rowOffset + x + 1].y;
            scale = Resolution;
        }
        
        
        return (right - left) * scale;
    }

    private float getZDerivative(int x, int z)
    {
        int rowLength = Resolution + 1;
        float back, forward, scale;
        if (z > 0)
        {
            back = vertices[(z - 1) * rowLength + x].y;
            if (z < Resolution)
            {
                forward = vertices[(z + 1) * rowLength + x].y;
                scale = 0.5f * Resolution;
            }
            else
            {
                forward = vertices[z * rowLength + x].y;
                scale = Resolution;
            }
        }
        else
        {
            back = vertices[z * rowLength + x].y;
            forward = vertices[(z + 1) * rowLength + x].y;
            scale = Resolution;
        }
        return (forward - back) * scale;
    }

}
