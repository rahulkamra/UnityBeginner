using UnityEngine;
using System.Collections;

public abstract class MazeCellEdge : MonoBehaviour
{

    public MazeCell Cell, OtherCell;
    public MazeDirection Direction;

    public void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        this.Cell = cell;
        this.OtherCell = otherCell;
        this.Direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation();
    }

   
}
