using System;
using UnityEngine;
using System.Collections;


public class Grapher2 : MonoBehaviour
{

    [Range(10, 100)]
    public int Resolution = 10;
    private ParticleSystem.Particle[] points;

    private int _currentResolution;

    public enum FunctionOptions
    {
        Linear, Exponential, Parabola, Sine, Ripple
    }
    public FunctionOptions function;

    public delegate float FunctionDelegate(Vector3 value , float time);

    public FunctionDelegate[] FunctionsDelegates =
    {
        Linear,Exponential,Parabola,Sine,Ripple
    };
    // Use this for initialization
    void Start()
    {
        this._currentResolution = Resolution;
        CreatePoints();
    }

    private void CreatePoints()
    {
        float increment = 1.0f/(_currentResolution - 1);
        points = new ParticleSystem.Particle[_currentResolution * _currentResolution];
        int index = 0;
        for (int idx = 0; idx < _currentResolution; idx++)
        {
            for (int idz = 0; idz < _currentResolution; idz++)
            {
               
                float x = increment * idx;
                float z = increment * idz;
                points[index].position = new Vector3(x, 0, z);
                points[index].color = new Color(x, 0f, z);
                points[index].size = 0.1f;
                index++;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Resolution != _currentResolution || points == null)
        {
            this._currentResolution = Resolution;
            CreatePoints();
        }

        FunctionDelegate functionDelegate = FunctionsDelegates[(int)function];
        for (int idx = 0; idx < points.Length ; idx++)
        {
            Vector3 position = points[idx].position;
            position.y = functionDelegate(position, Time.timeSinceLevelLoad);
            points[idx].position = position;

            Color color = points[idx].color;
            color.g = position.y;
            points[idx].color = color;

        }
        this.GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }


    static float Linear(Vector3 value, float time)
    {
        return value.x;
    }

    static float Exponential(Vector3 value, float time)
    {
        return value.x * value.x;
    }

    static float Parabola(Vector3 value, float time)
    {
        value.x = 2 * value.x - 1;
        value.z = 2 * value.z - 1;
        return 1 - value.x * value.x * value.z * value.z;
    }

    static float Sine(Vector3 value, float time)
    {
        //value is from zero - 1 , we need to map it from 0 - 2PI
        return 0.50f +
        0.25f * Mathf.Sin(4f * Mathf.PI * value.x + 4f * time) * Mathf.Sin(2f * Mathf.PI * value.z + time) +
        0.10f * Mathf.Cos(3f * Mathf.PI * value.x + 5f * time) * Mathf.Cos(5f * Mathf.PI * value.z + 3f * time) +
        0.15f * Mathf.Sin(Mathf.PI * value.x + 0.6f * time);

    }

    private static float Ripple(Vector3 value, float time)
    {
        value.x -= 0.5f;
        value.z -= 0.5f;
        float squareRadius = value.x * value.x + value.z * value.z;
        return 0.5f + Mathf.Sin(15f * Mathf.PI * squareRadius - 2f * time) / (2f + 100f * squareRadius);
    }

  
}
