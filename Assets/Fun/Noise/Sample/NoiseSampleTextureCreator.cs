using UnityEngine;

using System.Collections;

public class NoiseSampleTextureCreator : MonoBehaviour
{

   
   
    [Range(0,0.2f)]
    public float Increment = 0.1f;

    [Range(0, 10)]
    public int Persistence = 1;

    [Range(0, 10)]
    public int Octaves = 1;

    [Range(0, 1)]
    public float VelocityMul = 0.01f;

    public int Segments = 10;

   

    [Range(0,0.1f)]
    public float ZChange = 0.01f;

    [Range(0, 10000)]
    public int NumParticles = 100;


    private Texture2D texture;
    private ArrayList LinesToDraw = new ArrayList();

    private float zOffset = 0.01f;

    private SimpleParticleSystem particleSystem;
    private Vector3[,] flowField;

    private int rows;
    private int cols;

    private void OnEnable()
    {

        cols = Segments;
        rows = Segments;

        Redraw();

        ParticleSystem _particleSystem = this.GetComponent<ParticleSystem>();

        flowField = new Vector3[rows, cols];
        particleSystem = new SimpleParticleSystem();
        particleSystem.Init(_particleSystem,rows,cols);
        particleSystem.InitParticles(NumParticles);

    }


    public void Redraw()
    {
        if (flowField == null)
            return;

        LinesToDraw = new ArrayList();

       
        PerlinNoise perlin = new PerlinNoise();
        
        for(int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //float noise = Mathf.PerlinNoise(row * Increment, col * Increment);
                float noise = perlin.OctavePerlin(row * Increment, col * Increment, zOffset, Octaves, Persistence);
                Vector2 vector = RadianToVector2(Mathf.PI * 2 * noise);
                flowField[row,col] = vector;

                Vector2 from = new Vector2(row, col);
                DrawLine(from, from + vector.normalized);
            }
        }

        
    }
    
    void DrawLine(Vector2 from , Vector2 to)
    {
        Vector2[] data = { from, to};
        LinesToDraw.Add(data);
    }


    void Update()
    {
        for(int idx = 0; idx < LinesToDraw.Count; idx++)
        {
            Vector2[] values = (Vector2[])LinesToDraw[idx];
            Debug.DrawLine(values[0], values[1], Color.red);
        }

        Redraw();
        particleSystem.Update(flowField, rows,cols, VelocityMul);
        zOffset += ZChange;
    }



    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }


}