using UnityEngine;
using System.Collections;

public class TextureCreator : MonoBehaviour {

	// Use this for initialization
    public int Resolution = 256;

    private Texture2D texture;
	void OnEnable()
    {
        texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24,true);
	    texture.name = "Main Texture";
	    GetComponent<MeshRenderer>().material.mainTexture = texture;
	    fillTexture();
    }


    void fillTexture()
    {
        for (int idx = 0; idx < Resolution; idx++)
        {
            for (int jdx = 0; jdx < Resolution; jdx++)
            {
                texture.SetPixel(idx,jdx,new Color((float)idx/Resolution, (float)jdx / Resolution, 0f));
            }
        }
        texture.Apply();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
