using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	// Use this for initialization
    public static float DistanceTraveled;
    public float Acceleration;
    public Vector3 JumpVelocity;
    public Vector3 BoostVelocity;
    public float GameOverY;

    private bool IsTouchingPlatform;
    private Rigidbody _rigidBody;
    private Vector3 _startPosition;
    private static int _boosts = 0;


    void Start ()
    {
       this._rigidBody = GetComponent<Rigidbody>();
       GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_START,OnGameStart);
       GameManager.EventDispatcher.AddEventListner(RunnerEvents.EVENT_GAME_END, OnGameEnd);
       this._startPosition = transform.localPosition;
       OnGameEnd(null);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetButtonDown("Jump"))
	    {
            if (IsTouchingPlatform)
            {
                this._rigidBody.AddForce(JumpVelocity, ForceMode.VelocityChange);
                this.IsTouchingPlatform = false;
            }
            else if(_boosts > 0)
            {
                this._rigidBody.AddForce(BoostVelocity, ForceMode.VelocityChange);
                _boosts--;
                GameManager.SetBoosts(_boosts);
            }
        }
	    
	    DistanceTraveled = this.transform.localPosition.x;
        GameManager.SetDistance(DistanceTraveled);
        if (this.transform.localPosition.y < GameOverY)
	    {
            GameManager.EventDispatcher.dispatchEvent(RunnerEvents.EVENT_GAME_END,null);
	    }
    }

    void FixedUpdate()
    {
        if (IsTouchingPlatform)
        {
            this._rigidBody.AddForce(Acceleration,0f,0f,ForceMode.Acceleration);
        }  
    }

    void OnCollisionEnter()
    {
        IsTouchingPlatform = true;
    }

    void OnCollisionExit()
    {
        IsTouchingPlatform = false;
    }


    void OnGameStart(EventPayLoad Event)
    {
        DistanceTraveled = 0f;
        this.transform.localPosition = _startPosition;
        this.GetComponent<Renderer>().enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.enabled = true;

        GameManager.SetDistance(DistanceTraveled);
        GameManager.SetBoosts(_boosts);
    }

    void OnGameEnd(EventPayLoad Event)
    {
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.enabled = false;
    }

    public static void AddBoost()
    {
        _boosts++;
        GameManager.SetBoosts(_boosts);
    }

}
