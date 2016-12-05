using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private MazeCell currentCell;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(MazeDirection.North);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move(MazeDirection.South);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(MazeDirection.West);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(MazeDirection.East);
        }
    }

    public void SetCell(MazeCell mazeCell)
    {
        this.currentCell = mazeCell;
        this.transform.localPosition = mazeCell.transform.localPosition;
    }

    private void move(MazeDirection direction)
    {
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if(edge is MazePassage)
        {
            SetCell(edge.OtherCell);
        }
    }


    
}
