using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

    // Use this for initialization
    private MarchingSquares squares;

	void Start () {
        squares = this.GetComponent<MarchingSquares>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 20;
            pos = Camera.main.ScreenToWorldPoint(pos);
            squares.AddHole(pos.x, pos.y);
            Debug.Log(pos);
        }
    }
}
