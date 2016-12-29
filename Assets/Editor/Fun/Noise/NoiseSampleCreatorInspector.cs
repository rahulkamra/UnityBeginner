using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NoiseSampleTextureCreator))]
public class NoiseSampleCreatorInspector : Editor
{

    private NoiseSampleTextureCreator creator;
    public override void OnInspectorGUI()
    {
        creator = this.target as NoiseSampleTextureCreator;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            creator.FillTexture();
        }
    }

    private void OnEnable()
    {
        creator = this.target as NoiseSampleTextureCreator;
        Undo.undoRedoPerformed += RefreshTexture;
    }

    private void OnDisable()
    {
        creator = this.target as NoiseSampleTextureCreator;
        Undo.undoRedoPerformed -= RefreshTexture;
    }

    private void RefreshTexture()
    {
        if (Application.isPlaying)
            creator.FillTexture();
    }

}
