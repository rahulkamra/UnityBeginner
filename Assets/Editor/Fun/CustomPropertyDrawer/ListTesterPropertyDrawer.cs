using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ListTester))]
public class ListTesterPropertyDrawer : Editor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorList.ShowList(serializedObject.FindProperty("Integers"));
        EditorList.ShowList(serializedObject.FindProperty("Vectors"));
        EditorList.ShowList(serializedObject.FindProperty("ColorModels"));
        EditorList.ShowList(serializedObject.FindProperty("Transforms"));
        serializedObject.ApplyModifiedProperties();
    }
    
}
