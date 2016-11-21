using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public Nucleon[] neucleons;
    public float spawnTime;
    public float distance;
    

    float timePassed = 0;
    float lastSpawnTime = 0;
    void FixedUpdate ()
    {
        this.timePassed =   this.timePassed + Time.deltaTime;
        if(timePassed - lastSpawnTime > spawnTime)
        {
            lastSpawnTime = timePassed;
            spawnAnObject();
        }
	}
    
    private void spawnAnObject()
    {
        Nucleon prefab = neucleons[Random.Range(0, neucleons.Length)];
        Nucleon obj = Instantiate<Nucleon>(prefab);//Instantiate(prefab);
        
        obj.transform.localPosition = Random.onUnitSphere * distance;
        
    }


}
