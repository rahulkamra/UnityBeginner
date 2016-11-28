using UnityEngine;
using UnityEditor;



class EditorList
{

    public static void ShowList(SerializedProperty list , bool showSize = true , bool showListLabel = true)
    {
       // EditorGUILayout.PropertyField(list, true);
        
        if(showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel++;
        }
        
        if(list.isExpanded)
        {
            
            if (showSize)
            {
                SerializedProperty SizeArrayProperty = list.FindPropertyRelative("Array.size");
                EditorGUILayout.PropertyField(SizeArrayProperty);
            }

            for (int idx = 0; idx < list.arraySize; idx++)
            {
                SerializedProperty eachChildProperty  = list.GetArrayElementAtIndex(idx);
                EditorGUILayout.PropertyField(eachChildProperty);
            }
        }

        if(showListLabel)
            EditorGUI.indentLevel--;
    }
}

