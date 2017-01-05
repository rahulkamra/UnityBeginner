using UnityEngine;
using System.Collections;

[SelectionBase]
public class VoxelGrid : MonoBehaviour {

    // Use this for initialization

    public int Resolution;
    public GameObject VoxelPrefab;

    private bool[] voxels;
    private float voxelSize;

    public void Initialize(int resolution , float Size)
    {
        this.Resolution = resolution;
        voxelSize = Size / Resolution;
        voxels = new bool[Resolution * Resolution];
        createVoxels();
    }

    void createVoxels()
    {
        for (int i = 0, y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                createVoxel(i,x,y);
            }
        }
    }

    void createVoxel(int index , int x , int y)
    {
        GameObject obj = Instantiate(VoxelPrefab);
        obj.transform.parent = this.transform;
        obj.transform.localPosition = new Vector3((x + 0.5f) * voxelSize, (y + 0.5f) * voxelSize);
        obj.transform.localScale = Vector3.one * voxelSize * 0.9f;
    }

    public void setVoxel(int x , int y , bool state)
    {
        voxels[y * Resolution + x] = state;
    }

    
}
