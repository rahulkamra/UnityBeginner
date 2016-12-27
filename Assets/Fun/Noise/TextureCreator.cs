using UnityEngine;
using System.Collections;
using System;

public class TextureCreator : MonoBehaviour {

	// Use this for initialization
    [Range(2,1024)]
    public int Resolution = 256;

    [Range(2, 1024)]
    public int Frequency = 512;

    [Range(1, 3)]
    public int Dimention = 3;

    public NoiseMthodType MethodType;

    private Texture2D texture;

   


    void OnEnable()
    {
        
       
        if (texture == null)
        {
            texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, false);
            texture.name = "Main Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
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

        Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
        Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
        Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));

        UnityEngine.Random.InitState(42);

        float stepSize = 1f / Resolution;
        for (int idx = 0; idx < Resolution; idx++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, stepSize * (idx + 0.5f));
            Vector3 point1 = Vector3.Lerp(point10, point11, stepSize * (idx + 0.5f));

            for (int jdx = 0; jdx < Resolution; jdx++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, stepSize * (jdx + 0.5f));
               
                NoiseMethod method = Noise.NoiseMethods[(int)MethodType][Dimention - 1];
                float sample = method(point, Frequency);

                if(MethodType == NoiseMthodType.Perlin)
                    sample = (sample + 1) * 0.5f;
                
                texture.SetPixel(jdx, idx, Color.white * sample) ;
            }
        }
        texture.Apply();
    }

    
    private void Update()
    {
        if(transform.hasChanged)
        {
            transform.hasChanged = false;
            FillTexture();
        }
    }

}
