using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private MazeCell currentCell;

    // Use this for initialization

    private MazeDirection currentDirection;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            move(currentDirection);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            move(currentDirection.GetOpposite());
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A))
        {
            move(currentDirection.GetCounterClockWiseDirection());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            move(currentDirection.GetClockWiseDirection());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(currentDirection.GetCounterClockWiseDirection());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(currentDirection.GetClockWiseDirection());
        }

    }

    public void SetCell(MazeCell mazeCell)
    {
        this.currentCell = mazeCell;
        this.transform.localPosition = mazeCell.transform.localPosition;
    }

    private void Rotate(MazeDirection direction)
    {
        //we need to rotate to this direction
        this.transform.localRotation =  direction.ToRotation();
        this.currentDirection = direction;
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
