using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ListTester))]
public class ListTesterPropertyDrawer : Editor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorList.ShowList(serializedObject.FindProperty("Integers"),EditorListOptions.SHOW_SIZE);
        EditorList.ShowList(serializedObject.FindProperty("Vectors"), EditorListOptions.SHOW_LIST_LABEL | EditorListOptions.SHOW_ELEMENT_LABELS);
        EditorList.ShowList(serializedObject.FindProperty("ColorModels"), EditorListOptions.SHOW_SIZE | EditorListOptions.SHOW_LIST_LABEL);
        EditorList.ShowList(serializedObject.FindProperty("Transforms"));
        serializedObject.ApplyModifiedProperties();
    }
    
}
