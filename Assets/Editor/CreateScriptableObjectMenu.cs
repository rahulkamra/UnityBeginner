using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System;
using System.Linq;
using Utils;


public class CreateScriptableObjectMenu
{
    [MenuItem("Assets/Create/CreateScriptableObject")]
    public static void CreateScriptableObject()
    {
        //we need to get all the clases which extends ScriptableObject
        Type[] result = TypeUtility.getAllClassOfTypeInAssembly<ScriptableObject>("Assembly-CSharp");
        CreateScriptableWindow.show(result);
    }
	
}

class CreateScriptableWindow : EditorWindow
{
    private int selectedIndex;
    private static String[] options;
    private static Type[] types;
    private static Type[] classes
    {
        get
        {
            return types;
        }

        set
        {
            CreateScriptableWindow.types = value;
            CreateScriptableWindow.options = CreateScriptableWindow.types.Select(t => t.FullName).ToArray();

            /*for (int idx= 0; idx < value.Length; idx++)
            {
                options[idx] = value[idx].FullName;
            }*/

        }
    }
    public static void show(Type[] classes)
    {
        CreateScriptableWindow.classes = classes;
        EditorWindow newWindow = EditorWindow.GetWindow<CreateScriptableWindow>(true, "Create the Object", true);
        newWindow.ShowPopup();
    }

    public void OnGUI()
    {
        GUILayout.Label("ScriptableObject Class");
        
        selectedIndex = EditorGUILayout.Popup(selectedIndex, options);
        if(GUILayout.Button("Create"))
        {
           ScriptableObject tempObject = ScriptableObject.CreateInstance(types[selectedIndex]);

            
            string assetName = classes[selectedIndex] + ".asset";
           

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(tempObject.GetInstanceID(),
              ScriptableObject.CreateInstance<EditNameEdit>(), assetName, AssetPreview.GetMiniThumbnail(tempObject), null);



            Close();
        }
    }


}
class EditNameEdit:EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
    }
}
