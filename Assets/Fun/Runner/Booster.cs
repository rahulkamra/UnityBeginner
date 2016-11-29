using UnityEngine;
using System.Collections;

public class Booster : MonoBehaviour {

	// Use this for initialization
    public Vector3 Offset;
    public Vector3 RotationVelocity;
    public float RecycleOffset;
    public float SpawnChance;

    private bool isInit = false;
	// Update is called once per frame
	void Update ()
    {
	    if (transform.localPosition.x + RecycleOffset < Runner.DistanceTraveled)
	    {
	        gameObject.SetActive(false);
	        return;
	    }
	
        transform.Rotate(RotationVelocity * Time.deltaTime);
	}

    void Start()
    {
        init();
        
    }
    public void SpawnIfAvailable(Vector3 position)
    {
        if (gameObject.activeInHierarchy || SpawnChance <= Random.Range(0, 100f))
            return;

        transform.localPosition = position + Offset;
        gameObject.SetActive(true);
    }

    private void init()
    {
        GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START, OnGameStart);
        GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_END, OnGameEnd);
        gameObject.SetActive(false);
        isInit = true;
    }
    public void OnGameStart(EventPayLoad Event)
    {
        this.gameObject.SetActive(false);
    }

    public void OnGameEnd(EventPayLoad Event)
    {
        this.gameObject.SetActive(false);

    }

    void OnTriggerEnter()
    {
        Runner.AddBoost();
        this.gameObject.SetActive(false);
    }


}
