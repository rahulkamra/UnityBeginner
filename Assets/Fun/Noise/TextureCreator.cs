using UnityEngine;
using System.Collections;
using System;

public class TextureCreator : MonoBehaviour {

	// Use this for initialization
    [Range(2,1024)]
    public int Resolution = 256;

    private Texture2D texture;
	void OnEnable()
    {
        if(texture == null)
        {
            texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, false);
            texture.name = "Main Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }

        FillTexture();
    }

    public void FillTexture()
    {
        if(texture.width != Resolution)
        {
            texture.Resize(Resolution, Resolution);
        }

        float stepSize = 1f / Resolution;
        for (int idx = 0; idx < Resolution; idx++)
        {
            for (int jdx = 0; jdx < Resolution; jdx++)
            {
                float xVal = (idx + 0.5f) * stepSize % 0.1f;
                float yVal = (jdx + 0.5f) * stepSize % 0.1f;

                texture.SetPixel(idx, jdx, new Color(xVal, yVal, 0f) * 10f) ;
            }
        }
        texture.Apply();
    }
    
    
}
