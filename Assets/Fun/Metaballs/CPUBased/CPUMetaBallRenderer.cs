using UnityEngine;
using System.Collections;

class CPUMetaBallRenderer
{
    private Rect bounds;
    private Vector2 segmentation;

    private float xStep;
    private float yStep;

    private int cols;
    private int rows;


    private Vector3[] vertices;
    private int[] indices;
    private Color[] colors;

    private Mesh mesh;
    


    public Mesh Init(Rect bounds, Vector2 segmentation)
    {
        this.bounds = bounds;
        this.segmentation = segmentation;

        //we need to create a mesh here
        mesh = new Mesh();
        mesh.name = "MetaBallMesh";

        cols = Mathf.FloorToInt(segmentation.x);
        rows = Mathf.FloorToInt(segmentation.y);

        xStep = bounds.width / cols;
        yStep = bounds.height / rows;

        createMesh();
        return mesh;

       
    }

    
    private void createMesh()
    {
        
        vertices = new Vector3[(cols+1) * (rows + 1)];
        colors = new Color[(cols + 1) * (rows + 1)];

        for (int y = 0 , i = 0; y <= rows; y++)
        {
            for (int x  = 0; x <= cols ; x++,i++)
            {
                vertices[i] = new Vector3(x * xStep - 0.5f * cols * xStep, y * yStep - 0.5f * rows * yStep, 0f);
                colors[i] = Color.white;
            }
        }

        indices = new int[cols * rows * 6];
        for (int x = 0, i = 0 , v= 0; x < cols; x++, v++)
        {
            for (int y = 0; y < rows; y++, i+=6 , v++)
            {

                indices[i] = v;
                indices[i + 1] = v + cols + 1;
                indices[i + 2] = v +1;

                indices[i + 3] = v  +1;
                indices[i + 4] = v +  cols + 1;
                indices[i + 5] = v +cols + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.colors = colors;
    }


    public void Update(ArrayList balls)
    {
        for(int v = 0; v < vertices.Length; v++)
        {
            bool isInside = false;
            float contrib = 0f;
            for (int b = 0; b < balls.Count; b++)
            {
                contrib += ((Metaball)balls[b]).getContribution(vertices[v]);
            }

            if(contrib >= 1f)
            {
                colors[v] = Color.red;
            }
            else
            {
                colors[v] = Color.white;
            }
        }
        mesh.colors = colors;
    }


    
}

