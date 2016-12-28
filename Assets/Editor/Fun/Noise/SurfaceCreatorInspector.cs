using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SurfaceCreator))]
public class SurfaceCreatorInspector : Editor
{

    private SurfaceCreator creator;
    public override void OnInspectorGUI()
    {
        creator = this.target as SurfaceCreator;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            creator.Refresh();
        }
    }

    private void OnEnable()
    {
        creator = this.target as SurfaceCreator;
        Undo.undoRedoPerformed += RefreshTexture;
    }

    private void OnDisable()
    {
        creator = this.target as SurfaceCreator;
        Undo.undoRedoPerformed -= RefreshTexture;
    }

    private void RefreshTexture()
    {
        if (Application.isPlaying)
            creator.Refresh();
    }

}
