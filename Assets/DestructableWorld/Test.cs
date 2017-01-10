using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClipperLib;
using Polygon = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Polygons = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
public class Test : MonoBehaviour
{

    public Rect Bounds;
    public Vector2 Segmentation;

    private float xStep;
    private float yStep;

    private int cols;
    private int rows;

    private Vector3[] vertices;
    private int[] indices;
    private Color[] colors;

    private Mesh mesh;

    private ArrayList PolygonsToDraw = new ArrayList();

    void Start()
    {
        this.GetComponent<MeshFilter>().mesh = this.Init(Segmentation);
    }


    // Use this for initialization
    public Mesh Init(Vector2 segmentation)
    {
        
        //we need to create a mesh here
        mesh = new Mesh();
        mesh.name = "Mesh";

        cols = Mathf.FloorToInt(segmentation.x);
        rows = Mathf.FloorToInt(segmentation.y);

        xStep = Bounds.width / cols;
        yStep = Bounds.height / rows;

        createMesh();
        return mesh;
        
    }


    private void createMesh()
    {
        return;
        vertices = new Vector3[(cols + 1) * (rows + 1)];
        colors = new Color[(cols + 1) * (rows + 1)];

        for (int y = 0, i = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++, i++)
            {
                vertices[i] = new Vector3(x * xStep - 0.5f * cols * xStep, y * yStep - 0.5f * rows * yStep, 0f);
                colors[i] = Color.white;
            }
        }

        indices = new int[cols * rows * 6];
        for (int x = 0, i = 0, v = 0; x < cols; x++, v++)
        {
            for (int y = 0; y < rows; y++, i += 6, v++)
            {

                indices[i] = v;
                indices[i + 1] = v + cols + 1;
                indices[i + 2] = v + 1;

                indices[i + 3] = v + 1;
                indices[i + 4] = v + cols + 1;
                indices[i + 5] = v + cols + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.colors = colors;
    }

    public void AddAHole()
    {
        Polygons sub = new Polygons(1);
        Polygon polygon = new Polygon(4);

        polygon.Add(new IntPoint(-2, -2));
        polygon.Add(new IntPoint(2 , -2));
        polygon.Add(new IntPoint(2, 2));
        polygon.Add(new IntPoint(-2, 2));

        sub.Add(polygon);

      

        Polygon intersect = new Polygon(4);
        Polygons clip = new Polygons(1);


        intersect.Add(new IntPoint(-1, -1));
        intersect.Add(new IntPoint(1, -1));
        intersect.Add(new IntPoint(1, 1));
        intersect.Add(new IntPoint(-1, 1));

        clip.Add(intersect);

        
        

        Clipper clipper = new Clipper();
        clipper.AddPaths(sub, PolyType.ptSubject, true);
        clipper.AddPaths(clip, PolyType.ptClip, true);

        Polygons solution = new Polygons();


        addPolygonsToDraw(clip, Color.red);
       // addPolygonsToDraw(sub, Color.blue);

        bool isSuccess = clipper.Execute(ClipType.ctDifference, solution,PolyFillType.pftEvenOdd , PolyFillType.pftEvenOdd);
        Debug.Log(isSuccess);
       // TriangulateAndShow(solution);
        Debug.Log(solution);

        addPolygonsToDraw(solution, Color.white);

    }

    private void DrawPolygons()
    {
        for (int i = 0; i < PolygonsToDraw.Count; i++)
        {
            ArrayList list = (ArrayList) PolygonsToDraw[i];
            drawEachPolygon((Polygon)list[0], (Color)list[1]);
        }

            
    }

    private void addPolygonToDraw(Polygon polygon , Color color)
    {
        ArrayList list = new ArrayList{polygon , color};
        PolygonsToDraw.Add(list);
    }

    private void addPolygonsToDraw(Polygons polygons, Color color)
    {
        for(int idx = 0; idx < polygons.Count; idx++)
        {
            ArrayList list = new ArrayList { polygons[idx], color };
            PolygonsToDraw.Add(list);
        }
        
    }

    private void drawEachPolygon(Polygon polygon,Color color)
    {
        for (int i = 0; i < polygon.Count - 1; i++)
        {
            Debug.DrawLine(new Vector3(polygon[i].X, polygon[i].Y),
                new Vector3(polygon[i + 1].X, polygon[i + 1].Y), color);
        }

        Debug.DrawLine(new Vector3(polygon[polygon.Count - 1].X, polygon[polygon.Count - 1].Y),
                new Vector3(polygon[0].X, polygon[0].Y), color);
    }
    private void Update()
    {
        DrawPolygons();
    }


    private void TriangulateAndShow(Polygons solution)
    {
        int numVertices = 0;
        for(int i = 0; i < solution.Count; i ++)
        {
            Polygon polygon = solution[i];
            numVertices += polygon.Count;
        }

       
        Vector2[] vertices2 = new Vector2[numVertices];
        Vector3[] vertices3 = new Vector3[numVertices];

        List<TriangleNet.Geometry.Vertex> vertices = new List<TriangleNet.Geometry.Vertex>();
        for (int idx = 0 , vdx = 0; idx < solution.Count; idx++)
        {
            Polygon polygon =  solution[idx];
            for (int jdx = 0; jdx < polygon.Count; jdx++ , vdx ++)
            {
                vertices3[vdx] = new Vector3(polygon[jdx].X, polygon[jdx].Y);
                vertices.Add(new TriangleNet.Geometry.Vertex(polygon[jdx].X, polygon[jdx].Y));
            }
        }

        TriangleNet.Meshing.Algorithm.Dwyer triangulator = new TriangleNet.Meshing.Algorithm.Dwyer();
        TriangleNet.Meshing.IMesh tmesh = triangulator.Triangulate(vertices, new TriangleNet.Configuration());
        
        ICollection<TriangleNet.Topology.Triangle> triangles = tmesh.Triangles;
        List<int> indices = new List<int>(); 
        foreach (TriangleNet.Topology.Triangle triangle in triangles)
        {
         //   int vertexId = triangle.GetVertexID(0);
           // int vertexId = triangle.GetVertexID(1);
           // int vertexId = triangle.GetVertexID(2);

            indices.Add(triangle.GetVertexID(2));
            indices.Add(triangle.GetVertexID(1));
            indices.Add(triangle.GetVertexID(0));

            Debug.Log(triangle.GetVertexID(0) + "  " + triangle.GetVertexID(1) + "  " + triangle.GetVertexID(2));

        }

        //for(int  t = 0; t < )
        //  ICollection<TriangleNet.Data.Triangle> triangles = mesh.Triangles;

        //  Debug.Log(triangles);
        int[] intIndices = indices.ToArray();
        mesh.vertices = vertices3;
        mesh.triangles = intIndices;




    }


 


}
