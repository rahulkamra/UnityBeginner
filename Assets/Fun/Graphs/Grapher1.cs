using UnityEngine;
using System.Collections;


public class Grapher1 : MonoBehaviour
{

    [Range(10,100)]
    public int Resolution = 10;
    private ParticleSystem.Particle[] points;

    private int _currentResolution;

    public enum FunctionOptions
    {
        Linear,Exponential,Parabola,Sine
    }
    public FunctionOptions function;

    public delegate float FunctionDelegate(float value);

    public FunctionDelegate[] FunctionsDelegates =
    {
        Linear,Exponential,Parabola,Sine
    };
	// Use this for initialization
    void Start()
    {
        this._currentResolution = Resolution;
        CreatePoints();
    }

    private void CreatePoints()
    {
        points = new ParticleSystem.Particle[_currentResolution];
        for (int idx = 0; idx < _currentResolution; idx++)
        {
            float x = 1.0f / (_currentResolution - 1) * idx;
            points[idx].position = new Vector3(x, x, 0f);
            points[idx].color = new Color(x, 0f, 0f);
            points[idx].size = 0.1f;
        }
    }

    // Update is called once per frame
    void Update ()
    {
	    if (this.Resolution != _currentResolution || points == null)
	    {
	        this._currentResolution = Resolution;
            CreatePoints();
	    }

        FunctionDelegate functionDelegate = FunctionsDelegates[(int)function];
        for (int idx = 0; idx < _currentResolution; idx++)
        {
            Vector3 position = points[idx].position;
            position.y = functionDelegate(position.x);
            points[idx].position = position;

            Color color = points[idx].color;
            color.g = position.y;
            points[idx].color = color;
            
        }
        this.GetComponent<ParticleSystem>().SetParticles(points,points.Length);
	}


    static float Linear(float value)
    {
        return value;
    }

    static float Exponential(float value)
    {
        return value * value;
    }

    static float Parabola(float value)
    {
        value = 2 * value - 1;
        return value * value;
    }

    static float Sine(float value)
    {
        //value is from zero - 1 , we need to map it from 0 - 2PI
        value = (value + Time.timeSinceLevelLoad)* Mathf.PI * 2;
        return Mathf.Sin(value);

    }

}
