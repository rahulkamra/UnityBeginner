using UnityEngine;
using System.Collections;
using System;

public class ClockAnimator : MonoBehaviour {

    // Use this for initialization

    public Transform seconds;
    public Transform minutes;
    public Transform hours;

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        DateTime time = DateTime.Now;
        float seconds = time.Second;

        double secondsAngle = - (time.Second / 60.0f) * 360;
        double minutesAngle = -(time.Minute / 60.0f) * 360;
        double hoursAngle = -(time.Hour / 12.0f) * 360;


        this.seconds.transform.rotation  = Quaternion.Euler(new Vector3(0, 0, (float)secondsAngle));
        this.minutes.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (float)minutesAngle));
        this.hours.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (float)hoursAngle));
 
    }
}
