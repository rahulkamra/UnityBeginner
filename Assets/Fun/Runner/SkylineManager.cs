using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkylineManager : MonoBehaviour {

	// Use this for initialization

    public Transform Prefab;
    public int NumObjects;
    public Vector3 StartPosition;
    public float RecycleOffset;

    public Vector3 MinSize = new Vector3(1, 1, 1);
    public Vector3 MaxSize = new Vector3(3, 3, 3);

    
    protected Vector3 nextPosition;

    protected Queue<Transform> Objects;

    void Start ()
    {
        Objects = new Queue<Transform>(NumObjects);
        GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START, OnGameStart);
        GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_END, OnGameEnd);
        this.enabled = false;

        for (int idx = 0; idx < NumObjects; idx++)
        {
            Transform transform = (Transform)Instantiate(Prefab,new Vector3(0,0,-100),Quaternion.identity);
            Objects.Enqueue(transform);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Runner.DistanceTraveled - Objects.Peek().localPosition.x > RecycleOffset)
	    {
	        //means we need to push pack again
	        Transform transform = Objects.Dequeue();
            Recycle(transform);
	    }
	}

    protected virtual void Recycle(Transform transform)
    {

        Vector3 scale = new Vector3
       (
           Random.Range(MinSize.x, MaxSize.x),
           Random.Range(MinSize.y, MaxSize.y),
           Random.Range(MinSize.z, MaxSize.z)
        );
        Vector3 position = nextPosition;

        position.x += scale.x* 0.5f;
        position.y += scale.y* 0.5f;

        transform.localScale = scale;
        transform.localPosition = position;
        

        this.nextPosition.x += scale.x;
        Objects.Enqueue(transform);
    }

    void OnGameStart(EventPayLoad Event)
    {
        this.enabled = true;
        nextPosition = StartPosition;
        int len = Objects.Count;
        for (int idx = 0; idx < len; idx++)
        {
            Transform transform = Objects.Dequeue();
            Recycle(transform);
        }
    }

    void OnGameEnd(EventPayLoad Event)
    {
        this.enabled = false;
    }
}
