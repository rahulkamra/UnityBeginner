using UnityEngine;
using System.Collections;

public class ParticleSystemManager : MonoBehaviour {

	// Use this for initialization

    public ParticleSystem[] ParticleSystems;

	void Start ()
    {
	    GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START,OnGameStart);
        GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_END, OnGameEnd);
        OnGameEnd(null);
    }


    public void OnGameStart(EventPayLoad Event)
    {
        for(int idx = 0 ; idx < ParticleSystems.Length ; idx++)
        {
            ParticleSystems[idx].Clear();;
            ParticleSystems[idx].enableEmission = true;
        }
    }

    public void OnGameEnd(EventPayLoad Event)
    { 
        for(int idx = 0; idx<ParticleSystems.Length ; idx++)
        {
          ParticleSystems[idx].enableEmission = false;
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
