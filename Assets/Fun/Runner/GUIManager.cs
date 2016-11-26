using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {

	// Use this for initialization
    public Text TxtGameOver;
    public Text TxtInstruction;
    public Text TxtRunner;

    public static EventDispatcher EventDispatcher = new EventDispatcher();

    void Start ()
    {
        TxtGameOver.enabled = false;
        EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START,OnGameStart);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Jump"))
	    {
            EventDispatcher.dispatchEvent(RunnerEvents.EVENT_GAME_START,null);
        }
	}

    public void OnGameStart(Event Event)
    {
        TxtGameOver.enabled = false;
        TxtInstruction.enabled = false;
        TxtRunner.enabled = false;
        this.enabled = false;
    }
}
