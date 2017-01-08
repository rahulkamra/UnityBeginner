using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VerletBoxCollider : MonoBehaviour
{
    private void Start()
    {
        initPoints();
    }
    [SerializeField]
    private float _width = 1;
    
    public float width
    {
        get
        {
           return  _width;
        }

        set
        {
            this._width = value;
            initPoints();
        }
    }

    [SerializeField]
    private float _height = 1;
    
    public float height
    {
        get
        {
            return _height;
        }

        set
        {
            this._height = value;
            initPoints();
        }
    }

    [HideInInspector]
    public Vector2[] Points;

    private void initPoints()
    {
        Points = new Vector2[4];
        Points[0] = new Vector2(- _width / 2f, - _height / 2f);
        Points[1] = new Vector2(_width / 2f,- _height / 2f);
        Points[2] = new Vector2(_width / 2f, _height / 2f);
        Points[3] = new Vector2(- _width / 2f,_height / 2f);
       
    }
    
}
