using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VerletCircleCollider))]
public class VerletCircleColliderEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Take out this if statement to set the value using setter when ever you change it in the inspector.
        // But then it gets called a couple of times when ever inspector updates
        // By having a button, you can control when the value goes through the setter and getter, your self.
        VerletCircleCollider getterSetter = (VerletCircleCollider)target;
        if (GUILayout.Button("Use setters/getters"))
        {
            if (target.GetType() == typeof(VerletCircleCollider))
            {
                
                getterSetter.radius = getterSetter.radius;
                DrawCircle(getterSetter);
                SceneView.RepaintAll();
            }
        }

        if (GUILayout.Button("AddAccelration"))
        {
            getterSetter.ax = 10f;
            getterSetter.ay = 10f;
        }

        if (GUILayout.Button("AddVelocity"))
        {
            getterSetter.x -= 0.04f;
            getterSetter.y -= 0.02f;
        }
    }


    private void OnSceneGUI()
    {
        VerletCircleCollider collider = this.target as VerletCircleCollider;
        if (collider == null)
            return;

        DrawCircle(collider);
    }


    private void DrawCircle(VerletCircleCollider collider)
    {
        Handles.DrawWireDisc(collider.transform.position, Vector3.forward, collider.radius);
    }
}
