using System;
using UnityEngine;
using System.Collections;


public class Grapher3 : MonoBehaviour
{

    [Range(10, 30)]
    public int Resolution = 10;
    private ParticleSystem.Particle[] _points;

    private int _currentResolution;

    public enum FunctionOptions
    {
        Linear, Exponential, Parabola, Sine, Ripple
    }

    public FunctionOptions Function;
    public bool Absolute;
    public float Threshold = 0.5f;


    public delegate float FunctionDelegate(Vector3 value, float time);

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
        float increment = 1.0f / (_currentResolution - 1);
        _points = new ParticleSystem.Particle[_currentResolution * _currentResolution * _currentResolution];
        int index = 0;
        for (int idx = 0; idx < _currentResolution; idx++)
        {
            for (int idz = 0; idz < _currentResolution; idz++)
            {
                for (int idy = 0; idy < _currentResolution; idy++)
                {
                    float x = increment * idx;
                    float z = increment * idz;
                    float y = increment * idy;

                    _points[index].position = new Vector3(x, y, z);
                    _points[index].color = new Color(x, y, z);
                    _points[index].size = 0.1f;
                    index++;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Resolution != _currentResolution || _points == null)
        {
            this._currentResolution = Resolution;
            CreatePoints();
        }

        FunctionDelegate functionDelegate = FunctionsDelegates[(int)Function];
        for (int idx = 0; idx < _points.Length; idx++)
        {
            Vector3 position = _points[idx].position;
        

            Color color = _points[idx].color;
            float value = functionDelegate(position, Time.timeSinceLevelLoad);
            if (Absolute)
            {
                color.a = value >= Threshold ? 1f : 0f;
            }
            else
            {
                color.a = value;
            }
            
            _points[idx].color = color;

        }
        this.GetComponent<ParticleSystem>().SetParticles(_points, _points.Length);
    }


    static float Linear(Vector3 value, float time)
    {
        return 1- value.x - value.y - value.z + 0.5f * Mathf.Sin(time);
    }

    static float Exponential(Vector3 value, float time)
    {
        return 1 - value.x*value.x - value.y*value.y - value.z*value.z + 0.5f*Mathf.Sin(time);
    }

    static float Parabola(Vector3 value, float time)
    {
        value.x = 2 * value.x - 1;
        value.z = 2 * value.z - 1;
        return 1 - value.x * value.x  -  value.z * value.z + 0.5f * Mathf.Sin(time);
    }

    private static float Sine(Vector3 value, float time)
    {
        float x = Mathf.Sin(2*Mathf.PI*value.x);
        float y = Mathf.Sin(2*Mathf.PI*value.y);
        float z = Mathf.Sin(2*Mathf.PI*value.z + (value.y > 0.5f ? time : -time));
        return x*x*y*y*z*z;
    }

    private static float Ripple(Vector3 value, float time)
    {
        value.x -= 0.5f;
        value.z -= 0.5f;
        value.y -= 0.5f;
        float squareRadius = value.x * value.x + value.z * value.z + value.y * value.y;
        return  Mathf.Sin(4f*Mathf.PI*squareRadius - 2f*time);
    }


}
