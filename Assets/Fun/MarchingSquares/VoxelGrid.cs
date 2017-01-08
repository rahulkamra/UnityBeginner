using UnityEngine;
using System.Collections;

[SelectionBase]
public class VoxelGrid : MonoBehaviour {

    // Use this for initialization

    public int Resolution;
    public GameObject VoxelPrefab;

    private bool[] voxels;
    private float voxelSize;
    private Material[] voxelMaterials;

    public void Initialize(int resolution , float Size)
    {
        this.Resolution = resolution;
        voxelSize = Size / Resolution;
        voxels = new bool[Resolution * Resolution];
        voxelMaterials = new Material[voxels.Length];
        createVoxels();
        setVoxelColors();
    }

    void createVoxels()
    {
        for (int i = 0, y = 0; y < Resolution; y++)
        {
            for (int x = 0; x < Resolution; x++ , i++)
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
        voxelMaterials[index] = obj.GetComponent<MeshRenderer>().material;
    
    }

    public void Apply(VoxelStencil stencil)
    {
        int xStart = stencil.XStart;
        if (xStart < 0)
            xStart = 0;

        int xEnd = stencil.XEnd;
        if (xEnd >= Resolution)
            xEnd = Resolution - 1;

        int yStart = stencil.YStart;
        if (yStart < 0)
            yStart = 0;

        int yEnd = stencil.YEnd;
        if (yEnd >= Resolution)
            yEnd = Resolution - 1;

        for (int y = yStart; y <= yEnd; y++)
        {
            int  i = y * Resolution + xStart;
            for (int x = xStart; x <= xEnd; x++ , i++)
            {
                voxels[i] = stencil.Apply(x, y);
            }
        }
        setVoxelColors();
    }


    private void setVoxelColors()
    {
        for(int i = 0; i < voxels.Length; i++)
        {
            voxelMaterials[i].color = voxels[i] ? Color.black : Color.white;
        }
    }
    
}
