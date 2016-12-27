using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor
{

    private TextureCreator creator;
    public override void OnInspectorGUI()
    {
        creator =  this.target as TextureCreator;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if(EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            creator.FillTexture();
        }
    }

    private void OnEnable()
    {
        creator = this.target as TextureCreator;
        Undo.undoRedoPerformed += RefreshTexture;
    }

    private void OnDisable()
    {
        creator = this.target as TextureCreator;
        Undo.undoRedoPerformed -= RefreshTexture;
    }

    private void RefreshTexture()
    {
        if (Application.isPlaying)
            creator.FillTexture();
    }

}
