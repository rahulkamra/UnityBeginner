using UnityEngine;
using System.Collections;

public class TempUI : MonoBehaviour {

    // Use this for initialization

    public Test test;
	void Start ()
    {
	
	}

    // Update is called once per frame
    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,50,50),"Hello"))
        {
            test.AddAHole();
        }
    }


}
