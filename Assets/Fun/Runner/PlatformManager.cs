using UnityEngine;
using System.Collections;

public class PlatformManager : SkylineManager {

	// Use this for initialization
    public float MinY;
    public float MaxY;

    public Vector3 MinGap;
    public Vector3 MaxGap;

    public Material[] Materials;
    public PhysicMaterial[] PhysicMaterials;

    public Booster Booster;


    override protected void Recycle(Transform transform)
    {

        Vector3 scale = new Vector3
        (
            Random.Range(MinSize.x, MaxSize.x),
            Random.Range(MinSize.y, MaxSize.y),
            Random.Range(MinSize.z, MaxSize.z)
         );
        Vector3 position = nextPosition;

        position.x += scale.x * 0.5f;
        position.y += scale.y * 0.5f;

        Booster.SpawnIfAvailable(position);

        transform.localScale = scale;
        transform.localPosition = position;


        this.nextPosition.x += scale.x;
        Objects.Enqueue(transform);

        nextPosition += new Vector3
        (
            Random.Range(MinGap.x, MaxGap.x),
            Random.Range(MinGap.y, MaxGap.y),
            Random.Range(MinGap.z, MaxGap.z)
         );

        if (nextPosition.y > MaxY)
        {
            nextPosition.y = MaxY - MaxGap.y;
        }
       else  if (nextPosition.y < MinY)
        {
            nextPosition.y = MinY + MaxGap.y;
        }

        //we need to add material here
        int randomIndex = Random.Range(0, Materials.Length);
        transform.GetComponent<Renderer>().material = Materials[randomIndex];
        transform.GetComponent<BoxCollider>().material = PhysicMaterials[randomIndex];
    }
}
