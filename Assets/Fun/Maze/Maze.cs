using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

	// Use this for initialization

    public int SizeX;
    public int SizeZ;
    public MazeCell CellPrepab;
    public float GenerationDelay;

    private MazeCell[,] Cells;


    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Generate()
    {
        
        Cells = new MazeCell[SizeX, SizeZ];
        for (int row = 0; row < SizeX; row++)
        {
            for (int col = 0; col < SizeZ; col++)
            {
                yield return new WaitForSeconds(GenerationDelay);
                CreateCell(row,col);
            }
        }

    }

    private void CreateCell(int x, int z)
    {
        MazeCell newCell = Instantiate(CellPrepab) as MazeCell;
        Cells[x, z] = newCell;
        newCell.name = "Maze Cell " + x + ", " + z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(x - SizeX * 0.5f + 0.5f, 0f, z - SizeZ * 0.5f + 0.5f);
    }
}
