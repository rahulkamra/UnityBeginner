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

    }
}
