using UnityEngine;
using System.Collections;

public class MazeGameManager : MonoBehaviour {

	// Use this for initialization
    public Maze Maze;
    public Player PlayerPrefab;


    private Maze mazeInstance;
    private Player playerInstance;


    void Start ()
	{
	    StartCoroutine(BeginGame());
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
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        StartCoroutine(BeginGame());
    }

    IEnumerator BeginGame()
    {
        mazeInstance =  Instantiate(Maze);
        yield return StartCoroutine(mazeInstance.Generate());
        createPlayerInstance();
    }

    void createPlayerInstance()
    {
        this.playerInstance = Instantiate(PlayerPrefab);
        this.playerInstance.transform.parent = mazeInstance.transform;
        IntVector2 coordinates = mazeInstance.RandomCoordinates;
        MazeCell cell =  mazeInstance.GetMazeCell(coordinates);
        this.playerInstance.SetCell(cell);
        
    }


}
