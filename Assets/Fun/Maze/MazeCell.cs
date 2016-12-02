using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MazeCell : MonoBehaviour {

   
	// Use this for initialization
    public IntVector2 Coordinate;
    private int initializedEdgeCount = 0;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];


    public MazeCellEdge GetEdge(MazeDirection direction)
    {
        return this.edges[(int)direction];
    }

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        this.edges[(int) direction] = edge;
        initializedEdgeCount++;
    }

    public bool IsFulltInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
            
        }
    }

    public MazeDirection RandomUnitializedDirection()
    {
        List<MazeDirection> uninitEdges = new List<MazeDirection>();
        for (int idx = 0; idx < MazeDirections.Count; idx++)
        {
            if (this.edges[idx] != null)
            {
                continue;
            }
            else
            {
                uninitEdges.Add((MazeDirection)idx);
            }
        }

        try
        {
            return uninitEdges[Random.Range(0, uninitEdges.Count)];
        }
        catch (Exception)
        {
            Debug.LogError("Error in getting Directions");
            throw;
        }
       
    }
}
