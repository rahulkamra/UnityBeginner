using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject
{
    public int SettingsIndex;
    public MazeRoomSettings MazeRoomSettings;

    private List<MazeCell> cells = new List<MazeCell>();

    public void Add(MazeCell cell)
    {
        cell.Room = this;
        cells.Add(cell);   
    }

}

