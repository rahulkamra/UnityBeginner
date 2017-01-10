using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarchingSquares : MonoBehaviour
{

    public Rect Bounds;
    public Vector2 Segmentation;

    private bool[,] DotArray;
    
    private float xStep;
    private float yStep;

    private int cols;
    private int rows;

    private LineDrawer lineDrawer;


    public float HoleRadius = 2f;
    public List<Vector2> Holes;
    public List<EdgeCollider2D> edgeColliders;

    void Start()
    {
        cols = Mathf.FloorToInt(Segmentation.x);
        rows = Mathf.FloorToInt(Segmentation.y);

        xStep = Bounds.width / cols;
        yStep = Bounds.height / rows;

        edgeColliders = new List<EdgeCollider2D>();

        initMarchedArray();

        GameObject lineDrawerObj = GameObject.Find("LineDrawer");
        this.lineDrawer =  lineDrawerObj.GetComponent<LineDrawer>();




        RefreshEverything();  
        AddHole(-9f, -9f);
        AddHole(-1f, -1f);
        RefreshEverything();

        //GetComponent<EdgeCollider2D>().points;

        // 
    }


    private void initMarchedArray()
    {
        DotArray = new bool[rows + 1, cols + 1];

        for (int col = 0; col <= cols; col++)
        {
            for (int row = 0; row <= rows; row++)
            {
                if(col == 0)
                {
                    DotArray[row, col] = true;
                }

                if (col == cols)
                {
                    DotArray[row, col] = true;
                }

                if (row == rows)
                {
                    DotArray[row, col] = true;
                }

                if (row == 0)
                {
                    DotArray[row, col] = true;
                }
            }
        }
    }
    
    private void AddHole(float x , float y)
    {
        this.Holes.Add(new Vector2(x, y));
    }

    private void RefreshEverything()
    {
        lineDrawer.ClearAll();
        RefreshMarchingSquare();
        RenderBlocks();
        RenderGrid();
        CreateCollider();
    }

    private void RefreshMarchingSquare()
    {
        for(int h = 0; h < this.Holes.Count; h ++)
        {
            Vector2 localCoordinate = this.Holes[h];

            Vector2 gridCoordinate = GetRowCols(localCoordinate.x, localCoordinate.y);
            
            int xGridChange = Mathf.CeilToInt(HoleRadius / xStep);
            int yGridChange = Mathf.CeilToInt(HoleRadius / yStep);


            int minXGrid = Clamp((int)gridCoordinate.x - xGridChange - 1, 0, cols - 1);
            int maxXGrid = Clamp((int)gridCoordinate.x + xGridChange + 1, 0, cols - 1);

            int minYGrid = Clamp((int)gridCoordinate.y - yGridChange - 1, 0, rows - 1);
            int maxYGrid = Clamp((int)gridCoordinate.y + yGridChange, 0, rows - 1);

            for (int col = minXGrid; col < maxXGrid; col++)
            {
                for (int row = minYGrid; row < maxYGrid; row++)
                {
                    processCellCircleHole(row, col, localCoordinate, HoleRadius);
                }
            }
        }
       
    }
  

    private void processCellCircleHole(int row, int col , Vector2 origin , float radius)
    {
        bool tlBit = DotArray[row + 1, col];
        bool trBit = DotArray[row + 1, col + 1];
        bool brBit = DotArray[row, col + 1];
        bool blBit = DotArray[row, col];

        
        Vector2 localCoordinates = GetLocalCoordinates(row, col);

        if(!tlBit) 
        {
            Vector2 tl = new Vector2(localCoordinates.x - xStep * 0.5f, localCoordinates.y + yStep * 0.5f);
            tlBit = Vector2.Distance(tl, origin) <= radius;
            DotArray[row + 1, col] = tlBit;
        }

        if (!trBit)
        {
            Vector2 tr = new Vector2(localCoordinates.x + xStep * 0.5f, localCoordinates.y + yStep * 0.5f);
            trBit = Vector2.Distance(tr, origin) <= radius;
            DotArray[row + 1, col + 1] = trBit;
        }

        if (!brBit)
        {
            Vector2 br = new Vector2(localCoordinates.x + xStep * 0.5f, localCoordinates.y - yStep * 0.5f);
            brBit = Vector2.Distance(br, origin) <= radius;
            DotArray[row, col + 1] = brBit;
        }

        if (!blBit)
        {
            Vector2 bl = new Vector2(localCoordinates.x - xStep * 0.5f, localCoordinates.y - yStep * 0.5f);
            blBit = Vector2.Distance(bl, origin) <= radius;
            DotArray[row, col] = blBit;
        }
        
    }


    private uint getValue(int row , int col)
    {
        bool tlBit = DotArray[row + 1, col];
        bool trBit = DotArray[row + 1, col + 1];
        bool brBit = DotArray[row, col + 1];
        bool blBit = DotArray[row, col];

        uint value = 0;

        if (tlBit)
            value += 1000;
        if (trBit)
            value += 100;
        if (brBit)
            value += 10;
        if (blBit)
            value += 1;

        return value;
    }

    //X,y is the center of the block
    //Same pattern as https://en.wikipedia.org/wiki/Marching_squares
    public LineModel getLineModel(int row ,int col)
    {
        Vector2 localCooridnates =  GetLocalCoordinates(row, col);
        uint value = getValue(row , col);
        float x = localCooridnates.x;
        float y = localCooridnates.y;

        LineModel edge = new LineModel();
       
        switch (value)
        {
            case 1110:
            case 0001:
                edge.from.x = x - xStep/2f;
                edge.from.y = y;

                edge.to.x = x;
                edge.to.y = y - yStep / 2f;
                break;


            case 1101:
            case 0010:
                edge.from.x = x;
                edge.from.y = y - yStep * 0.5f;

                edge.to.x = x + xStep * 0.5f;
                edge.to.y = y;
                break;

            case 1011:
            case 0100:
                edge.from.x = x;
                edge.from.y = y + yStep * 0.5f;

                edge.to.x = x + xStep * 0.5f;
                edge.to.y = y;
                break;

            case 0111:
            case 1000:
                edge.from.x = x - xStep * 0.5f;
                edge.from.y = y;

                edge.to.x = x;
                edge.to.y = y + yStep * 0.5f;
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
                //Debug.LogError("Unhandled case");
                break;

        }
        
        return edge;
    }


    public void CreateCollider()
    {
        for(int c = 0; c < edgeColliders.Count; c++)
        {
            Destroy(edgeColliders[c]);
        }


        List<Vector2> points = new List<Vector2>();
        for (int y = 0; y < cols; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                LineModel lineModel =  getLineModel(x, y);
                EdgeCollider2D collider = this.gameObject.AddComponent<EdgeCollider2D>();
                this.edgeColliders.Add(collider);

                Vector2[] colliderPoints = new Vector2[2];

                colliderPoints[0] = lineModel.from;
                colliderPoints[1] = lineModel.to;
                collider.points = colliderPoints;
            }
        }
    }


    public void RenderBlocks()
    {
        for (int y = 0; y < cols; y++)
        {
            for (int x = 0; x < rows; x++)
            {

                RenderBlock(x,y);
            }
        }
    }

    private Vector2 GetRowCols(float x , float y)
    {
        return new Vector2(Mathf.FloorToInt((x - Bounds.xMin) / xStep), Mathf.FloorToInt((y - Bounds.yMin) / yStep));
    }

    private Vector2 GetLocalCoordinates(int row , int col)
    {
        return new Vector2(Bounds.xMin + col * xStep + xStep * 0.5f , Bounds.yMin + row * yStep + yStep * 0.5f);
    }


    private void RenderBlock(int row , int col)
    {
        LineModel lineModel =  getLineModel(row, col);
        lineModel.color = Color.red;
        lineDrawer.AddLineToDraw(lineModel);
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


    public static int Clamp(int value, int min, int max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }
}


