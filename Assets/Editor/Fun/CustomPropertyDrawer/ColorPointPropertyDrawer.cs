using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomPropertyDrawer(typeof(ColorPointModel))]
public class ColorPointPropertyDrawer : PropertyDrawer
{
    private float _lineHeight = 16f;
    private float _lineGap = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);
        Rect currentPosition = EditorGUI.PrefixLabel(position, label);

        if(position.height > _lineHeight)
        {
            //means it is two lines
            position.height = _lineHeight;
            EditorGUI.indentLevel = oldIndentLevel + 1;
            currentPosition = EditorGUI.IndentedRect(position);
            currentPosition.y += _lineHeight + _lineGap;
        }
        currentPosition.width *= 0.75f;
        EditorGUI.indentLevel = oldIndentLevel;
        EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("Position"),GUIContent.none);

        currentPosition.x += currentPosition.width;
        currentPosition.width = currentPosition.width / 3f;

        EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(currentPosition, property.FindPropertyRelative("Color"), new GUIContent("C"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return label != GUIContent.none && Screen.width < 333 ? 2*_lineHeight + _lineGap : _lineHeight;
    }

}
