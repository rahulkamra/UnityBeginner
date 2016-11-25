using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	// Use this for initialization
    public static float DistanceTraveled;
    public float Acceleration;

    private bool IsTouchingPlatform;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    DistanceTraveled = this.transform.localPosition.x;
    }

    void FixedUpdate()
    {
        if (IsTouchingPlatform)
        {
            GetComponent<Rigidbody>().AddForce(Acceleration,0f,0f,ForceMode.Acceleration);
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
}
