using UnityEngine;
using System.Collections;

public class VoxelMap : MonoBehaviour
{
    public float Size = 2f;

    public int VoxelResolution = 8;
    public int ChunkResolution = 2;

    public VoxelGrid VoxelGridPrefab;


    private VoxelGrid[] chunks;

   

}
