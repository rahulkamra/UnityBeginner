using UnityEngine;
using System.Collections;

public class MarchingSquares : MonoBehaviour
{

    public Rect Bounds;
    public Vector2 Segmentation;

    private uint[,] MarchedArray;
    private Edge[,] MarchedEdgeArray;

    private float xStep;
    private float yStep;

    private int cols;
    private int rows;

    private LineDrawer lineDrawer;
    void Start()
    {
        cols = Mathf.FloorToInt(Segmentation.x);
        rows = Mathf.FloorToInt(Segmentation.y);

        xStep = Bounds.width / cols;
        yStep = Bounds.height / rows;

        initMarchedArray();

        GameObject lineDrawerObj = GameObject.Find("LineDrawer");
        this.lineDrawer =  lineDrawerObj.GetComponent<LineDrawer>();

        RenderEdges();
        RenderGrid();
    }


    private void initMarchedArray()
    {
        MarchedArray = new uint[rows,cols];
        MarchedEdgeArray = new Edge[rows, cols];


        for (int y = 0; y < cols; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                if(x == 0)
                {
                    setMarchedValue(x, y, 0110);
                }

                if (x == rows-1)
                {
                    setMarchedValue(x, y, 1001);
                }

                if (y == 0)
                {
                    setMarchedValue(x, y, 0011);
                }

                if (y == cols - 1)
                {
                    setMarchedValue(x, y, 1100);
                }
            }
        }
    }



    //X,y is the center of the block
    //Same pattern as https://en.wikipedia.org/wiki/Marching_squares
    public Edge getEdge(float x,float y,uint value)
    {

        Edge edge = new Edge();
       

        switch (value)
        {
            case 1110:
                edge.from.x = x - xStep/2f;
                edge.from.y = y;

                edge.to.x = x;
                edge.to.y = y - yStep / 2f;
                break;


            case 1001:
            case 0110:
                edge.from.x = x;
                edge.from.y = y - yStep / 2f;

                edge.to.x = x;
                edge.to.y = y + yStep / 2f;
                break;

            case 1100:
            case 0011:
                edge.from.x = x - xStep / 2f;
                edge.from.y = y;

                edge.to.x = x + xStep / 2f;
                edge.to.y = y;
                break;

            default:
                Debug.LogError("Unhandled case");
                break;

        }
        
        return edge;
    }

 



    public void RenderEdges()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                if (MarchedEdgeArray[x, y] != null)
                {
                    Edge edge = MarchedEdgeArray[x, y];
                    RenderEdge(edge);
                }
            }
        }
    }

    private Vector2 GetSquareCoodinates(int x , int y)
    {
        return new Vector2(Bounds.xMin + x * xStep + xStep * 0.5f , Bounds.yMin + y * yStep + yStep * 0.5f);
    }


    private void RenderEdge(Edge edge)
    {
        lineDrawer.AddLineToDraw(new LineModel(new Vector3(edge.from.x , edge.from.y), new Vector3(edge.to.x, edge.to.y),Color.red));
    }

    private void setMarchedValue(int x , int y , uint value)
    {
        MarchedArray[x, y] = value;
        Vector2 localCoordinates =  GetSquareCoodinates(x, y);
        Edge edge =  getEdge(localCoordinates.x, localCoordinates.y, value);
        MarchedEdgeArray[x, y] = edge;
    }

    private void RenderGrid()
    {
        for (int r = 0; r <= rows; r++)
        {
            lineDrawer.AddLineToDraw(new LineModel(
                new Vector3(Bounds.xMin, Bounds.yMin + yStep * r),
                new Vector3(Bounds.xMax, Bounds.yMin + yStep * r), Color.white));
        }

        for (int c = 0; c <= cols; c++)
        {
            lineDrawer.AddLineToDraw(new LineModel(
                new Vector3(Bounds.xMin + xStep * c, Bounds.yMin),
                new Vector3(Bounds.xMin + xStep * c, Bounds.yMax), Color.white));
        }
    }
}

public class Edge
{
    public Vector2 from = new Vector2();
    public Vector2 to = new Vector2();

}
