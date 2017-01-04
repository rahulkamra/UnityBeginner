using UnityEngine;
using System.Collections;


public class BasicMetaBallCreator : MonoBehaviour {


    public int NumBalls = 4;

    public Rect Bounds = new Rect(-50, -50, 100, 100);
    public int Segmentation = 20;

    public GameObject MetaBallDebugPrefab;
    public float MaxSpeed = 1;
    public float BallRadius = 1f;
    public bool ShowDebug = false;

    

    protected ArrayList balls;
    protected ArrayList debugGameObjects;


    // Use this for initialization
    void Start ()
    {

        onStart();
    }
	
    protected virtual void onStart()
    {
        balls = new ArrayList();
        debugGameObjects = new ArrayList();

        for (int i = 0; i < NumBalls; i++)
        {
            Metaball ball = new Metaball
            (
                new Vector2
                (
                    Random.Range(Bounds.xMin, Bounds.xMax),
                    Random.Range(Bounds.yMin, Bounds.yMax)
                )
            );
            ball.Radius = this.BallRadius;
            ball.Velocity = RadianToVector2(Random.Range(0f, 1f) * 2 * Mathf.PI) * MaxSpeed;
            balls.Add(ball);
        }


    }
    
    void Update ()
    {
        updatePositions();
        OnUpdate();

        if (ShowDebug)
        {
            ApplyPositionToDebug();
            DrawBoundry();
        }
    }

    protected virtual void OnUpdate()
    {

    }


    private void ApplyPositionToDebug()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Metaball ball = (Metaball)balls[i];
            GameObject g = getDebugGameObject(i);
            g.transform.position = ball.Position;
        }
    }


    private GameObject getDebugGameObject(int index)
    {
        if (index >= debugGameObjects.Count)
        {
            GameObject gameObject = Instantiate(MetaBallDebugPrefab);
            gameObject.transform.parent = transform.parent;
            Vector3 bounds = gameObject.GetComponent<Renderer>().bounds.size;

            float realXBound = 2f * BallRadius;
            float scale = realXBound / bounds.x;
            scale = gameObject.transform.localScale.x * scale;
            gameObject.transform.localScale = new Vector3(scale, scale, 0);
            debugGameObjects.Add(gameObject);
            return gameObject;
        }
        else
        {
            return (GameObject)debugGameObjects[index];
        }
    }

    private void DrawBoundry()
    {
        Color rectColor = Color.red;

        Debug.DrawLine(new Vector3(Bounds.xMin, Bounds.yMin), new Vector3(Bounds.xMax, Bounds.yMin), rectColor);
        Debug.DrawLine(new Vector3(Bounds.xMin, Bounds.yMin), new Vector3(Bounds.xMin, Bounds.yMax), rectColor);

        Debug.DrawLine(new Vector3(Bounds.xMax, Bounds.yMin), new Vector3(Bounds.xMax, Bounds.yMax), rectColor);
        Debug.DrawLine(new Vector3(Bounds.xMin, Bounds.yMax), new Vector3(Bounds.xMax, Bounds.yMax), rectColor);

    }


    private void updatePositions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            Metaball ball = (Metaball)balls[i];

            ball.Position = ball.Position + ball.Velocity;

            float xOffset = 0f;
            float yOffset = 0f;

            if (ball.Position.x > Bounds.xMax - xOffset || ball.Position.x < Bounds.xMin + xOffset)
            {
                ball.Velocity.x = -ball.Velocity.x;
            }

            if (ball.Position.y > Bounds.yMax - yOffset || ball.Position.y < Bounds.yMin + yOffset)
            {
                ball.Velocity.y = -ball.Velocity.y;
            }
        }

    }

    public Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

}
