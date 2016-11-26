using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // Use this for initialization
    public Text TxtGameOver;
    public Text TxtInstruction;
    public Text TxtRunner;
    public Text TxtBoost;
    public Text TxtDistance;

    public static EventDispatcher EventDispatcher = new EventDispatcher();
    public static GameManager Instance;
    private void Start()
    {
        Instance = this;
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
        TxtBoost.enabled = true;
        TxtDistance.enabled = true;
    }

    public static void SetBoosts(int boosts)
    {
        Instance.TxtBoost.text = boosts.ToString();
    }
    public static void SetDistance(float distance)
    {
        Instance.TxtDistance.text = distance.ToString("f0");
    }

    public void OnGameEnd(Event Event)
    {
        TxtGameOver.enabled = true;
        TxtInstruction.enabled = true;
        this.enabled = true;
        TxtBoost.enabled = true;
        TxtDistance.enabled = true;
    }
}