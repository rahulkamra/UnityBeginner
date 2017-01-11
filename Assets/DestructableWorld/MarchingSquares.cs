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

    private List<Vector2> Holes;
    private List<EdgeCollider2D> edgeColliders;

   // public GameObject[] Prefabs;
   // private Dictionary<string,GameObject> cache;


    void Start()
    {
        cols = Mathf.FloorToInt(Segmentation.x);
        rows = Mathf.FloorToInt(Segmentation.y);

        xStep = Bounds.width / cols;
        yStep = Bounds.height / rows;

        edgeColliders = new List<EdgeCollider2D>();
       // cache = new Dictionary<string, GameObject>();
        Holes = new List<Vector2>();


       // for (int p = 0;p < Prefabs.Length; p++)
      //  {
       //     cache[Prefabs[p].name] = Prefabs[p];
        //}


        initMarchedArray();

        GameObject lineDrawerObj = GameObject.Find("LineDrawer");
        this.lineDrawer =  lineDrawerObj.GetComponent<LineDrawer>();

        
        RefreshEverything();  
       // AddHole(-9f, -9f);
       // AddHole(-1f, -1f);
       // AddHole(-1f, 0f);
       // AddHole(-1f, 1f);
       // AddHole(-1f, 2f);
       // AddHole(-1f, 4f);
       // AddHole(-1f, 7f);
       // AddHole(-1f, 9f);
        RefreshEverything();

       
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
    
    public void AddHole(float x , float y)
    {
        this.Holes.Add(new Vector2(x, y));
        RefreshMarchingSquare(new Vector2(x, y));
        RefreshEverything();
    }

    private void RefreshEverything()
    {
        if(lineDrawer)
            lineDrawer.ClearAll();
        RenderBlocks();
        RenderGrid();
        CreateCollider();
    }

    private void RefreshMarchingSquare(Vector2 hole)
    {
      
            Vector2 localCoordinate = hole;

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


    
    
    private void CreateCollider()
    {
        foreach(EdgeCollider2D collider in this.edgeColliders)
        {
            Destroy(collider);
        }

        List <List<Cell>> result  = new CreateCollider().Create(DotArray);
        foreach (List<Cell> eachResult in result)
        {
            createEachCollider(eachResult);
        }
    }

    private void createEachCollider(List<Cell> colliders)
    {
        
        EdgeCollider2D collider = this.gameObject.AddComponent<EdgeCollider2D>();
        this.edgeColliders.Add(collider);
        List<Vector2> points = new List<Vector2>();

        for (int idx = 0; idx < colliders.Count; idx++ )
        {
            Cell eachCell = colliders[idx];

            LineModel lineModel = getLineModel(eachCell.row, eachCell.col);

            if(points.Count > 0)
            {
               Vector2 lastPoint = points[points.Count - 1];
                
               if(AlmostEqual(lastPoint.x , lineModel.from.x) && AlmostEqual(lastPoint.y , lineModel.from.y))
               {
                    points.Add(lineModel.from);
                    points.Add(lineModel.to);
                }
                else if (AlmostEqual(lastPoint.x , lineModel.to.x) && AlmostEqual(lastPoint.y , lineModel.to.y))
                {

                    points.Add(lineModel.to);
                    points.Add(lineModel.from);
                }
                else
                {
                    Debug.LogError("Unknown shape"  +  "LastPoint " + lastPoint + "  Line From"  + lineModel.from + "  Libne To" + lineModel.to) ;
                }

            }
            else
            {
                points.Add(lineModel.from);
                points.Add(lineModel.to);
            }
            
            
           

            //Debug.Log(lineModel.from.ToString());
          //  Debug.Log(lineModel.to.ToString());
            
        }
      
        collider.points = points.ToArray();
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
        if(lineDrawer)
            lineDrawer.AddLineToDraw(lineModel);
    }

   
    private void RenderGrid()
    {
        if (lineDrawer == null)
            return;


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

    public static bool AlmostEqual(float x, float y)
    {
        double epsilon = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) * 1E-2;
        return Mathf.Abs(x - y) <= epsilon;
    }
}


