using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour {

    private List<LineModel> lineModel = new List<LineModel>();
	
    public void AddLineToDraw(LineModel model)
    {
        this.lineModel.Add(model);
    }

    public void ClearAll()
    {
        lineModel = new List<LineModel>();
    }
	// Update is called once per frame
	void Update ()
    {
	    for(int i = 0; i < lineModel.Count; i++)
        {
            LineModel model =  lineModel[i];
            Debug.DrawLine(model.from, model.to, model.color);
        }
	}



}

public class LineModel
{
    public LineModel()
    {

    }
    public LineModel(Vector3 from , Vector3 to , Color color)
    {
        this.from = from;
        this.to = to;
        this.color = color;
    }

    public Vector3 from;
    public Vector3 to;
    public Color color;

}
