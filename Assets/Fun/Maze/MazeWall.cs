using UnityEngine;
public class MazeWall : MazeCellEdge
{


    public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        base.Initialize(cell, otherCell, direction);
        this.transform.GetChild(0).GetComponent<Renderer>().material = cell.Room.MazeRoomSettings.WallMaterial;
    }

}

