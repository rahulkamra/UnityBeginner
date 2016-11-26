using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

	// Use this for initialization
    public static float DistanceTraveled;
    public float Acceleration;
    public Vector3 JumpVelocity;

    private bool IsTouchingPlatform;
    private Rigidbody _rigidBody;

   void Start ()
    {
        this._rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (IsTouchingPlatform && Input.GetButtonDown("Jump"))
	    {
            this._rigidBody.AddForce(JumpVelocity,ForceMode.VelocityChange);
	        this.IsTouchingPlatform = false;
	    }
	    DistanceTraveled = this.transform.localPosition.x;
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
}
