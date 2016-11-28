using UnityEngine;
using UnityEditor;



class EditorList
{

    public static void ShowList(SerializedProperty list , EditorListOptions options = EditorListOptions.DEFAULT)
    {
        // EditorGUILayout.PropertyField(list, true);
        bool showListLabel      = (options & EditorListOptions.SHOW_LIST_LABEL) != 0;
        bool showSize           = (options & EditorListOptions.SHOW_SIZE) != 0;
        bool showElementLabels  = (options & EditorListOptions.SHOW_ELEMENT_LABELS) != 0;

        if (showListLabel)
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
                if(showElementLabels)
                    EditorGUILayout.PropertyField(eachChildProperty);
                else
                    EditorGUILayout.PropertyField(eachChildProperty,GUIContent.none);
            }
        }

        if(showListLabel)
            EditorGUI.indentLevel--;
    }
}

enum EditorListOptions
{
    NONE = 0,
    SHOW_LIST_LABEL = 1,
    SHOW_SIZE  = 2,
    SHOW_ELEMENT_LABELS = 4,

    DEFAULT = SHOW_SIZE | SHOW_LIST_LABEL | SHOW_ELEMENT_LABELS
}

