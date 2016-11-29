using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(Star))]
public class StarInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty Points =  serializedObject.FindProperty("Points");
        SerializedProperty Frequency = serializedObject.FindProperty("Frequency");

        int numPoints = Points.arraySize* Frequency.intValue;
        if (numPoints < 3)
        {
            EditorGUILayout.HelpBox("ATleast 3 Points Needed", MessageType.Warning); 
        }
        else
        {
            EditorGUILayout.HelpBox("Num Points : " + numPoints, MessageType.Info);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Center"));
        EditorList.ShowList(Points, EditorListOptions.DEFAULT | EditorListOptions.ADD_BUTTONS);
        EditorGUILayout.PropertyField(Frequency);
        
        
        if(serializedObject.ApplyModifiedProperties() || Event.current.type == EventType.ValidateCommand)
        {
            
            foreach(Star star in serializedObject.targetObjects)
            {
                star.UpdateMesh();
            }
        }

    }
}
