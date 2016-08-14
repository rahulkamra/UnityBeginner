using UnityEngine;
using System.Collections;

public class BootScriptScriptableObjects : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        string assetPath = Application.dataPath + "/SavingGame/ScriptableObjects/Instances/EnemyA.asset";
        Object asset = Resources.Load(assetPath);
        Debug.Log(asset);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
