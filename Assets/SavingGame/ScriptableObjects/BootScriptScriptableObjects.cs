using UnityEngine;
using System.Collections;
using Models;

public class BootScriptScriptableObjects : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        string assetPath = string.Concat("file://", Application.dataPath , "/Bundles/scriptableobjects.unity3d");
        Object asset = Resources.Load(assetPath);
        IEnumerator assetBundle = loadAssetBundle(assetPath);
        StartCoroutine(assetBundle);
        Debug.Log(asset);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator loadAssetBundle(string assetPath)
    {
        WWW www = new WWW(assetPath);
        yield return www;

        if (www.error != null)
            Debug.LogError(www.error);

        Debug.Log("Loading Asset from " + assetPath);
        AssetBundle bundle = www.assetBundle;

        Debug.Log(bundle);
        AssetBundleRequest request = bundle.LoadAssetAsync("EnemyA", typeof(EnemyConf));
        yield return request;

        EnemyConf enemy = request.asset as EnemyConf;
        //Debug.Log(enemy.);
        //return null;
    }
}
