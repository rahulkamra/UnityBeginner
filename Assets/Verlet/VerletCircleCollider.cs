using UnityEngine;
using System.Collections;

public class VerletCircleCollider : MonoBehaviour {

    void Awake()
    {
        this.x = this.transform.position.x;
        this.y = this.transform.position.y;
        this.px = this.x;
        this.py = this.y;
        GameObject.Find("PhysicsSystem").GetComponent<VerletPhysicsSystem>().AddCircleCollider(this);

        this.x += Random.Range(-1f, 1f) * 0.1f;
        this.y += Random.Range(-1f, 1f) * 0.01f;
    }

    [SerializeField]
    private float _radius = 1;


    public float radius
    {
        get
        {
            return _radius;
        }

        set
        {
            this._radius = value;
        }
    }


    [HideInInspector]
    public float px;
    [HideInInspector]
    public float py;
    [HideInInspector]

    public float ax;
    [HideInInspector]
    public float ay;
    [HideInInspector]

    public float x;
    [HideInInspector]
    public float y;

    public float friction;
    public float mass = 1f;

    public float dynamicFriction = 0.8f;
    

    [HideInInspector]
    public float lastVX;
    [HideInInspector]
    public float lastVY;


    public void Apply()
    {
        try
        {
            this.transform.position = new Vector3(x, y, this.transform.position.z);
        }
        catch(UnityException e)
        {
            Debug.Log("a");
        }
        
    }

    private void Update()
    {
      //  this.x = this.transform.position.x;
       // this.y = this.transform.position.y;
    }


    public float getDistance(VerletCircleCollider collider)
    {
        return Vector2.Distance(new Vector2(collider.x, collider.y), new Vector2(this.x, this.y));
    }

}
