using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

    // Use this for initialization
    
    public Material material;
    public int maxDepth = 0;
    public float childScale = 0f;
    private int depth = 0;

    private Material[,] materials;
    public Mesh[] meshes;
    public float spawnProbability;

    public float maxRotationSpeed;
    public float rotationSpeed;

    void Start ()
    {
        this.gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0,meshes.Length -1)];
        this.gameObject.AddComponent<MeshRenderer>().material = material;
        if(this.materials == null)
        {
            initMaterials();
        }
        this.gameObject.GetComponent<MeshRenderer>().material = this.materials[depth,Random.Range(0,3)];
        this.rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private void initMaterials()
    {
        this.materials = new Material[maxDepth +1 , 3];
        for(int idx = 0; idx <= maxDepth; idx++)
        {
            float t = idx / (maxDepth - 1.0f);
            t *= t;
         
            materials[idx, 0] = new Material(material);
            materials[idx, 0].color = Color.Lerp(Color.white, Color.yellow, t);

            materials[idx, 1] = new Material(material);
            materials[idx, 1].color = Color.Lerp(Color.white, Color.cyan, t);

            materials[idx, 2] = new Material(material);
            materials[idx, 2].color = Color.Lerp(Color.white, Color.green, t);

        }

        materials[maxDepth , 0].color = Color.magenta;
        materials[maxDepth , 1].color = Color.red;


    }
    private static Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left , Vector3.forward, Vector3.back};
    private static Quaternion[] orientations = { Quaternion.identity , Quaternion.Euler(new Vector3(0, 0, 90)) , Quaternion.Euler(new Vector3(0, 0, 180)) , Quaternion.Euler(new Vector3(0, 0, -90)) , Quaternion.Euler(90,0,0), Quaternion.Euler(-90, 0, 0) };

    private IEnumerator CreateChildren()
    {
        for(int idx = 0; idx < directions.Length; idx++)
        {
            if(Random.value < spawnProbability)
            {
                new GameObject("Child").AddComponent<Fractal>().init(this, idx);
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            }  
        }
    }
	
    private void init(Fractal parent ,int childIndex)
    {
        Vector3 direction = directions[childIndex];
        Quaternion rotation = orientations[childIndex];

        meshes = parent.meshes;
        this.material = parent.material;
        this.maxDepth = parent.maxDepth;
        this.depth = parent.depth + 1;
        this.transform.parent = parent.transform ;
        this.childScale = parent.childScale;
        this.transform.localScale = new Vector3(this.childScale, this.childScale, this.childScale);
        this.transform.localPosition = direction * (0.5f + 0.5f * childScale);
        this.transform.localRotation = rotation;
        this.materials = parent.materials;
        this.spawnProbability = parent.spawnProbability;
        this.maxRotationSpeed = parent.maxRotationSpeed;
      
    }

	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(new Vector3(0, Time.deltaTime * this.rotationSpeed, 0));
	}
}
