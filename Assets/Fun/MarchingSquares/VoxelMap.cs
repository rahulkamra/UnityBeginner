using UnityEngine;
using System.Collections;

public class VoxelMap : MonoBehaviour
{
    public float Size = 2f;

    //This is the the resolution of the small part
    public int VoxelResolution = 8;

    //this is the resolution of how many small part.
    public int ChunkResolution = 2;

    public VoxelGrid VoxelGridPrefab;


    private VoxelGrid[] chunks;
    private float chunkSize, voxelSize, halfSize;

    private static string[] fillTypeNames = {"Filled" , "Empty" };
    private static string[] radiusName = { "0", "1", "2", "3", "4" , "5"};
    private int fillTypeIndex , radiusIndex;


    private void Awake()
    {
        halfSize = Size * 0.5f;
        chunkSize = Size / ChunkResolution;
        voxelSize = chunkSize / VoxelResolution;

        chunks = new VoxelGrid[ChunkResolution * ChunkResolution];
        for(int i = 0 , y= 0; y < ChunkResolution; y++)
        {
            for (int x = 0; x < ChunkResolution; x++, i++)
            {
                createChunk(i,x,y);
            }
        }

        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(Size, Size);

    }


    private void createChunk(int i , int x , int y)
    {
        VoxelGrid chunk = Instantiate(VoxelGridPrefab) as VoxelGrid;
        chunk.Initialize(VoxelResolution, chunkSize);
        chunk.transform.parent = this.transform;
        chunk.transform.localPosition = new Vector3(x * chunkSize - halfSize, y * chunkSize - halfSize);
        chunks[i] = chunk;
    }



    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hitInfo))
            {
                if(hitInfo.collider.gameObject == gameObject)
                {
                    editVoxel(transform.InverseTransformDirection(hitInfo.point));
                }
            }

        }    
    }


    private void editVoxel(Vector3 point)
    {
        int centerX = (int)((point.x + halfSize) / voxelSize);
        int centerY = (int)((point.y + halfSize) / voxelSize);

        int xStart = (centerX - radiusIndex) / VoxelResolution;
        int xEnd = (centerX + radiusIndex) / VoxelResolution;

        int yStart = (centerY - radiusIndex) / VoxelResolution;
        int yEnd = (centerY + radiusIndex) / VoxelResolution;


        if (xStart < 0)
            xStart = 0;

        if (xEnd >= ChunkResolution)
            xEnd = ChunkResolution - 1;

        if (yStart < 0)
            yStart = 0;

        if (yEnd >= ChunkResolution)
            yEnd = ChunkResolution - 1;

        
        VoxelStencil stencil = new VoxelStencil();
        stencil.Initialize(fillTypeIndex == 0,radiusIndex);

        int voxelYOffset = yStart * VoxelResolution;
        for (int y = yStart; y <= yEnd; y++)
        {
            int i = y * ChunkResolution + xStart;
            int voxelXOffset = xStart * VoxelResolution;
            for (int x = xStart; x <= xEnd; x++, i++)
            {
                stencil.SetCenter(centerX - voxelXOffset, centerY - voxelYOffset);
                chunks[i].Apply(stencil);
                voxelXOffset += VoxelResolution;
            }
            voxelYOffset += VoxelResolution;
        }
    }


    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(4f ,4f , 150f ,500f));
        GUILayout.Label("Fill Type");
        fillTypeIndex = GUILayout.SelectionGrid(fillTypeIndex, fillTypeNames, fillTypeNames.Length);
        GUILayout.Label("Radius");
        radiusIndex = GUILayout.SelectionGrid(radiusIndex, radiusName, radiusName.Length);
        GUILayout.EndArea();
    }

}
