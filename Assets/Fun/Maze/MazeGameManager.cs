using UnityEngine;
using System.Collections;

public class MazeGameManager : MonoBehaviour {

	// Use this for initialization
    public Maze Maze;

    private Maze mazeInstance;

	void Start ()
	{
	    BeginGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        RestartGame();
	    }
	
	}

    void RestartGame()
    {
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }

    void BeginGame()
    {
        mazeInstance =  Instantiate(Maze);
        StartCoroutine(mazeInstance.Generate());
    }


}
