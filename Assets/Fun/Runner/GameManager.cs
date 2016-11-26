using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    public Text TxtGameOver;
    public Text TxtInstruction;
    public Text TxtRunner;

    public static EventDispatcher EventDispatcher = new EventDispatcher();

    private void Start()
    {
        TxtGameOver.enabled = false;
        EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START, OnGameStart);
        EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_END, OnGameEnd);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            EventDispatcher.dispatchEvent(RunnerEvents.EVENT_GAME_START, null);
        }
    }

    public void OnGameStart(Event Event)
    {
        TxtGameOver.enabled = false;
        TxtInstruction.enabled = false;
        TxtRunner.enabled = false;
        this.enabled = false;
    }

    public void OnGameEnd(Event Event)
    {
        TxtGameOver.enabled = true;
        TxtInstruction.enabled = true;
        this.enabled = true;
    }
}