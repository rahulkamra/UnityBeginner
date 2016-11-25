using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	// Use this for initialization
    public static float DistanceTraveled;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Translate(5f * Time.deltaTime , 0f,0f);
	    DistanceTraveled = this.transform.localPosition.x;
    }
}
