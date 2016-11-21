using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {

    Rigidbody body;
    // Use this for initialization
    public float forceMultiplier;
	void Awake ()
    {
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        body.AddForce(-transform.localPosition * forceMultiplier);
	}
}
