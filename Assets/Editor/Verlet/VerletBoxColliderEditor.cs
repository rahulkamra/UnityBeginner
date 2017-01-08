using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VerletBoxCollider))]
class VerletBoxColliderEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Take out this if statement to set the value using setter when ever you change it in the inspector.
        // But then it gets called a couple of times when ever inspector updates
        // By having a button, you can control when the value goes through the setter and getter, your self.
        if (GUILayout.Button("Use setters/getters"))
        {
            if (target.GetType() == typeof(VerletBoxCollider))
            {
                VerletBoxCollider getterSetter = (VerletBoxCollider)target;
                getterSetter.width = getterSetter.width;
                DrawLines(getterSetter);
                SceneView.RepaintAll();
            }
        }
    }

    private void OnSceneGUI()
    {
        VerletBoxCollider collider = this.target as VerletBoxCollider;
        if (collider == null)
            return;

        DrawLines(collider);
    }


    private void DrawLines(VerletBoxCollider collider)
    {
        for (int idx = 0; idx < collider.Points.Length - 1; idx++)
        {
            Handles.DrawLine(collider.transform.TransformPoint(collider.Points[idx]), collider.transform.TransformPoint(collider.Points[idx + 1]));
        }
        Handles.DrawLine(collider.transform.TransformPoint(collider.Points[collider.Points.Length - 1]), collider.transform.TransformPoint(collider.Points[0]));
    }


}

