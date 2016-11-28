using UnityEngine;
using UnityEditor;



class EditorList
{

    private static GUIContent DuplicateButtonContent = new GUIContent("+", "Duplicate");
    private static GUIContent DeleteButtonContent = new GUIContent("-", "Remove");
    private static GUIContent MoveButtonContent = new GUIContent("\u21b4", "Move Down");
    private static GUIContent AddButtonContent = new GUIContent("+", "Add Button");

    public static void ShowList(SerializedProperty list , EditorListOptions options = EditorListOptions.DEFAULT)
    {
        if(!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + "  is neither an array or a list", MessageType.Error);
            return;
        }
        bool showListLabel      = (options & EditorListOptions.SHOW_LIST_LABEL) != 0;
        bool showSize           = (options & EditorListOptions.SHOW_SIZE) != 0;
        
        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel++;
        }
        
        if(list.isExpanded)
        {
            SerializedProperty SizeArrayProperty = list.FindPropertyRelative("Array.size");
            if (showSize)
            { 
                EditorGUILayout.PropertyField(SizeArrayProperty);
            }
            if(SizeArrayProperty.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not Showing List as They have Different Size",MessageType.Info); 
            }
            else
            {
                ShowElements(list, options);
            }
            
        }
        
        if(showListLabel)
            EditorGUI.indentLevel--;
    }

    private static void ShowElements( SerializedProperty list, EditorListOptions options)
    {
        bool showElementLabels = (options & EditorListOptions.SHOW_ELEMENT_LABELS) != 0;
        bool showButtons = (options & EditorListOptions.ADD_BUTTONS) != 0;

        for (int idx = 0; idx < list.arraySize; idx++)
        {
            if (showButtons)
            {
                GUILayout.BeginHorizontal();
            }
            SerializedProperty eachChildProperty = list.GetArrayElementAtIndex(idx);
            if (showElementLabels)
                EditorGUILayout.PropertyField(eachChildProperty);
            else
                EditorGUILayout.PropertyField(eachChildProperty, GUIContent.none);
            if (showButtons)
            {
                ShowButtons(list, idx);
                GUILayout.EndHorizontal();
            }
        }

        if (showButtons && list.arraySize == 0)
        {
            ShowGlobalAddButton(list);
        }
    }

    private static void ShowGlobalAddButton(SerializedProperty list)
    {
        if(GUILayout.Button(AddButtonContent))
        {
            //we need to add one item
            list.InsertArrayElementAtIndex(0);
        }
    }


    private static GUILayoutOption MinButtonWidth = GUILayout.Width(20f);
    private static void ShowButtons(SerializedProperty list, int index)
    {
        if(GUILayout.Button(MoveButtonContent,EditorStyles.miniButtonLeft, MinButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }

        if(GUILayout.Button(DuplicateButtonContent, EditorStyles.miniButtonLeft, MinButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }

        if(GUILayout.Button(DeleteButtonContent, EditorStyles.miniButtonLeft, MinButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if(oldSize == list.arraySize)
                list.DeleteArrayElementAtIndex(index);
        }
    }
}



enum EditorListOptions
{
    NONE = 0,
    SHOW_LIST_LABEL = 1,
    SHOW_SIZE  = 2,
    SHOW_ELEMENT_LABELS = 4,
    ADD_BUTTONS = 8,

    DEFAULT = SHOW_SIZE | SHOW_LIST_LABEL | SHOW_ELEMENT_LABELS,
    ALL = DEFAULT| ADD_BUTTONS
}

