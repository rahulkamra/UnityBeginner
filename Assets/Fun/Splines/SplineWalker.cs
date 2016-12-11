using UnityEngine;
using System.Collections;

public class SplineWalker : MonoBehaviour {

	// Use this for initialization
    public BezierSpline Spline;
    public float Duration;
    public SplineWalkerMode Mode;
   


    private float progress;
    private bool goingForward;


    
	
	// Update is called once per frame
	void Update ()
	{
	    if (goingForward)
	    {
            progress += Time.deltaTime / Duration;
            if (progress > 1f)
	        {
	            if (Mode == SplineWalkerMode.Once)
	            {
                    progress = 1f;
                }
                else if (Mode == SplineWalkerMode.Loop)
	            {
                    progress -= 1f;
                   
	            }
	            else
	            {
	                goingForward = false;
                    progress = 2f - progress;
                }
	        }
            
        }
	    else
	    {
            progress -= Time.deltaTime / Duration;
	        if (progress < 0f)
	        {
	            progress = -progress;
	            goingForward = true;
	        }
        }

	    
	    if (progress > 1)
	        progress = 1;


        Vector3 position = Spline.GetPoint(progress);
	    transform.localPosition = position;
        transform.LookAt(transform.localPosition  +Spline.GetDirection(progress));
    }
}
