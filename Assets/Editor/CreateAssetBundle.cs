using UnityEngine;
using UnityEditor;
using System.Collections;
using Utils;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class CreateAssetBundle : MonoBehaviour {


    [MenuItem("Assets/Create/CreateAssetBundle")]
    public static void CreateScriptableObject()
    {
        //Debug.logger
        string[] assetBundles = AssetDatabase.GetAllAssetBundleNames();
        CreateAssetBundleWindow.show(assetBundles);
    }
}

class CreateAssetBundleWindow : EditorWindow
{
    public string[] bundles;
    public int selectedIndex;
    private const string DEFAULT_PATH = "Assets/Bundles/";

    public static void show(string[] assetBundles)
    {
        CreateAssetBundleWindow window = EditorWindow.GetWindow<CreateAssetBundleWindow>(true, "CreateAssetBundle", true);
        window.bundles = assetBundles;
        window.ShowPopup();
    }

    public void OnGUI()
    {
        GUILayout.Label("AssetBundles");
        selectedIndex = EditorGUILayout.Popup(selectedIndex, bundles);
        if (GUILayout.Button("Create"))
        {
            string assetBundleName = bundles[selectedIndex];
            string selectedPath = EditorUtility.SaveFilePanel("Save File", DEFAULT_PATH, assetBundleName, "unity3d");
            if (selectedPath.Length != 0)
            {
                string fileName = selectedPath;

                AssetBundleBuild build = new AssetBundleBuild();

                build.assetBundleName = Path.GetFileName(selectedPath);
                build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                
                List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
                assetBundleBuilds.Add(build);
                
                BuildPipeline.BuildAssetBundles(Path.GetDirectoryName(selectedPath) , assetBundleBuilds.ToArray() , BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            }
            
        }
    }

}
