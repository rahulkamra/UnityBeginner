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
        int voxelX = (int)((point.x + halfSize) / voxelSize);
        int voxelY = (int)((point.y + halfSize) / voxelSize);

        int chunkX = voxelX / VoxelResolution;
        int chunkY = voxelY / VoxelResolution;
        
        voxelX -= chunkX * VoxelResolution;
        voxelY -= chunkY * VoxelResolution;
        
        chunks[chunkY * ChunkResolution + chunkX].setVoxel(voxelX, voxelY, true);
    }

}
