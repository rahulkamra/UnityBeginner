using UnityEngine;
using System.Collections;

public abstract class MazeCellEdge : MonoBehaviour
{

    [HideInInspector]
    public MazeCell Cell, OtherCell;

    [HideInInspector]
    public MazeDirection Direction;

    public virtual void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
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
