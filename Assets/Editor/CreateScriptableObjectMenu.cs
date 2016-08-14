using UnityEngine;
using UnityEditor;
using System.Collections;
using SavingGame.ScriptableObjects;
using System;
using System.Collections.Generic;
using Utils;


public class CreateScriptableObjectMenu
{
    [MenuItem("Assets/Create/CreateScriptableObject")]
    public static void CreateScriptableObject()
    {
        //we need to get all the clases which extends ScriptableObject
        Type[] result = TypeUtility.getAllClassOfTypeInAssembly<ScriptableObject>("Assembly-CSharp");
        


    }
	
}
