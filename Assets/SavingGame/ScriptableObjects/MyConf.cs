using UnityEngine;
using System.Collections;
using System;


public class MyConf : ScriptableObject {

    public int varA;
    public string varB;
    public string varC;

    //for custom editor please read
    //http://answers.unity3d.com/questions/292634/how-to-reference-monobehaviour-in-scriptableobject.html
    public UnityEngine.Object component;

}
