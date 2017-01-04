using UnityEngine;
using System.Collections;

class MetaBallRenderer
{
    private Rect bounds;
    private Vector2 segmentation;

    private float xStep;
    private float yStep;

    private int cols;
    private int rows;


    private Vector3[] vectices;
    private int[] indicies;
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
        
        Vector3[] vectices = new Vector3[(cols+1) * (rows + 1)];
        for (int y = 0 , i = 0; y <= rows; y++)
        {
            for (int x  = 0; x <= cols ; x++,i++)
            {
                vectices[i] = new Vector3(x * xStep - 0.5f * cols * xStep, y * yStep - 0.5f * rows * yStep, 0f);
            }
        }

        int[] indices = new int[cols * rows * 6];
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

        mesh.vertices = vectices;
        mesh.triangles = indices;
       

    }


    public void Update(ArrayList balls)
    {
            
    }
    
}

