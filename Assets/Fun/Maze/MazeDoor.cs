using UnityEngine;
using System.Collections;

public class MazeDoor : MazePassage
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        base.Initialize(cell, otherCell, direction);
        //now we have to check if the edge exist bettween cell - othercell or other cell -- cell
        for(int idx = 0; idx < transform.childCount; idx++)
        {
            Transform child = transform.GetChild(idx);
            if(child.name != "Hinge")
            {
                child.GetComponent<Renderer>().material = cell.Room.MazeRoomSettings.WallMaterial;
            }
        }
    }
}
