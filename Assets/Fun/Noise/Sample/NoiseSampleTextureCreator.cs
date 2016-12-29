using UnityEngine;
using System.Collections;

public class NoiseSampleTextureCreator : MonoBehaviour
{

    // Use this for initialization
    [Range(4,1024)]
    public int Resolution = 256;

    [Range(0,10)]
    public float Increment = 0.1f;

    [Range(0, 10)]
    public int Persistence = 1;

    [Range(0, 10)]
    public int Octaves = 1;

    public int Segmentation = 10;

    private int col;
    private int row;


    private Texture2D texture;

    private void OnEnable()
    {
        if (texture == null)
        {
            texture = new Texture2D(Resolution, Resolution);
            texture.name = "MainTexture";
            GetComponent<MeshRenderer>().material.mainTexture = texture;
        }
        col = (int)Mathf.Floor(Resolution / Segmentation);
        row = (int)Mathf.Floor(Resolution / Segmentation);

        FillTexture();
        
    }


    public void FillTexture()
    {
        if(texture && texture.width != Resolution)
        {
            texture.Resize(Resolution, Resolution);
        }

        float xOff = 0;
        float yOff = 0;
        PerlinNoise perlin = new PerlinNoise();

        for (int x = 0; x < Resolution; x++)
        {
            yOff = 0;
            for (int y = 0; y < Resolution; y++)
            {

                float noise = perlin.OctavePerlin(xOff,yOff,0f,Octaves,Persistence);//Mathf.PerlinNoise(xOff, yOff);               
                texture.SetPixel(x, y, new Color(noise, noise, noise));
                yOff += Increment;
            }
            xOff += Increment;
        }
        
        texture.Apply();
    }


    public float getNoise(float x , float y ,int octaves)
    {
        float returnValue = 0f;
        for(int idx = 0; idx < octaves; idx++)
        {
           // returnValue += Mathf.PerlinNoise(x, y) * ;
        }
        return returnValue;
    }
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}